using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace RiftDotNet
{
	/// <summary>
	/// This class manages all those tedious details of the Rift SDK.
	/// Only one instance of a manager should be needed and instantiated when the program starts
	/// (or when VR mode is to be entered).
	/// 
	/// The manager keeps track all all HMDs attached to the system and allows the user to retrieve
	/// objects representing those, in addition to be notified of device changes.
	/// 
	/// The manager supports multiple devices, and even keeps track of which object instances correspond
	/// to which device. Only one IHMD instance per physical device is ever created, meaning you, the user,
	/// can simply use that one instance, without worrying about device attachement/detachement.
	/// </summary>
	/// <remarks>
	/// Currently, a physical device is identified by manufacturer, product name and version, as reported by
	/// the SDK.
	/// </remarks>
	public sealed class HMDManager
		: IDisposable
	{
		private readonly IFactory _factory;
		private readonly Dictionary<DeviceKey, HMD> _devices;
		private readonly MessageHandler _handler;
		private readonly ReaderWriterLockSlim _lock;
		private readonly IDeviceManager _manager;
		private readonly Dictionary<DeviceKey, DeviceResources> _nativeResources;

		/// <summary>
		/// This event is fired when the given device is attached to the computer.
		/// </summary>
		/// <remarks>
		/// The event is fired from a different thread than the main thread.
		/// </remarks>
		public event Action<IHMD> DeviceAttached;

		/// <summary>
		/// This event is fired when the given device is detached from the computer.
		/// </summary>
		/// <remarks>
		/// The event is fired from a different thread than the main thread.
		/// </remarks>
		public event Action<IHMD> DeviceDetached;

		internal IDeviceManager Manager { get { return _manager; } }

		internal HMDManager(IFactory factory, IDeviceManager manager)
		{
			if (factory == null)
				throw new ArgumentNullException();

			if (manager == null)
				throw new ArgumentNullException();

			_factory = factory;
			_lock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
			_manager = manager;
			_handler = new InternalMessageHandler(this);
			_manager.MessageHandler = _handler;
			_nativeResources = new Dictionary<DeviceKey, DeviceResources>();
			_devices = new Dictionary<DeviceKey, HMD>();

			// Initially, we must to enumerate all devices, which are currently attached
			// to the computer.
			using (var handles = _manager.HMDDevices)
			{
				foreach (var handle in handles)
				{
					AddDevice(handle);
				}
			}
		}

		internal HMDManager(IFactory factory)
			: this(factory, factory.CreateDeviceManager())
		{}

		public HMDManager()
			: this(Factory.PlatformFactory)
		{}

		/// <summary>
		///     A reference to an HMD which is currently attached to this computer.
		///     In case multiple are attached, *any* is returned.
		/// In case no device has been attached to the computer *while this manager has been
		/// alove*, then null is returned.
		/// </summary>
		public IHMD AttachedDevice
		{
			get
			{
				_lock.EnterReadLock();
				try
				{
					return _devices.Values.FirstOrDefault(x => x.IsAttached);
				}
				finally
				{
					_lock.ExitReadLock();
				}
			}
		}

		/// <summary>
		/// The list of all devices, which have been connected to this computer
		/// at one point or the other.
		/// </summary>
		public List<IHMD> Devices
		{
			get
			{
				_lock.EnterReadLock();
				try
				{
					return new List<IHMD>(_devices.Values);
				}
				finally
				{
					_lock.ExitReadLock();
				}
			}
		}

		/// <summary>
		/// Returns *any* attached device to this computer.
		/// In case there's no device attached to yet, waits for the specified
		/// amount of time (or idenfinately, in case null is specified) and
		/// either returns a reference to the HMD, or null in case none
		/// was attached within the specified time.
		/// </summary>
		/// <param name="waitTime"></param>
		/// <returns></returns>
		public IHMD WaitForAttachedDevice(TimeSpan? waitTime)
		{
			// Warning to my dear reader: This method is ugly as fuck

			//
			// At first, we try to find a device which is attached.
			// If we succeed, then we can return that and be happy.
			// Otherwise, we subscribe to the DeviceAttached event
			// temporarily, and wait in this thread until the event
			// is fired, or until the specified time has elapsed.
			//

			IHMD hmd = AttachedDevice;
			if (hmd != null)
				return hmd;

			using (var handle = new AutoResetEvent(false))
			{
				Action<IHMD> fn = attachedHMD =>
				{
					// Invoked in case a device was indeed attached.
					hmd = attachedHMD;

					try
					{
						handle.Set();
					}
					catch (ObjectDisposedException)
					{
						
					}
				};

				DeviceAttached += fn;
				try
				{
					if (waitTime == null)
					{
						handle.WaitOne();
					}
					else
					{
						handle.WaitOne(waitTime.Value);
					}
				}
				finally
				{
					DeviceAttached -= fn;
				}
			}

			return hmd;
		}

		#region IDisposable Members

		public void Dispose()
		{
			_lock.EnterWriteLock();
			try
			{
				_handler.Dispose();

				foreach (var dev in _nativeResources.Values)
				{
					dev.Dispose();
				}
				_nativeResources.Clear();

				foreach (var hmd in _devices.Values)
				{
					hmd.Resources = null;
				}
				_devices.Clear();

				_manager.Dispose();

				DeviceAttached = null;
				DeviceDetached = null;
			}
			finally
			{
				_lock.ExitWriteLock();
			}
		}

		#endregion

		internal void DeviceChanged(IMessageDeviceStatus message)
		{
			if (message == null)
				return;

			if (message.Type == MessageType.DeviceAdded)
			{
				using (IDeviceHandle handle = message.DeviceHandle)
				{
					if (handle.DeviceType == DeviceType.HMD)
					{
						AddDevice((IDeviceHandle<IHMDDevice, IHMDInfo>) handle);
					}
				}
			}
			else if (message.Type == MessageType.DeviceRemoved)
			{
				using (IDeviceHandle handle = message.DeviceHandle)
				{
					if (handle.DeviceType == DeviceType.Sensor)
					{
						RemoveDevice((IDeviceHandle<ISensorDevice, ISensorInfo>) handle);
					}
				}
			}
		}

		private void AddDevice(IDeviceHandle<IHMDDevice, IHMDInfo> handle)
		{
			_lock.EnterWriteLock();
			try
			{
				var resources = new DeviceResources(_factory, handle);
				var key = resources.Key;
				_nativeResources.Add(key, resources);
				HMD hmd;

				if (!_devices.TryGetValue(key, out hmd))
				{
					// There's no HMD for this device yet: We need to create one
					hmd = new HMD(handle.DeviceInfo, _lock);
					_devices.Add(key, hmd);
				}

				hmd.Resources = resources;

				var fn = DeviceAttached;
				if (fn != null)
				{
					fn(hmd);
				}
			}
			finally
			{
				_lock.ExitWriteLock();
			}
		}

		private void RemoveDevice(DeviceResources resources)
		{
			_lock.EnterWriteLock();
			try
			{
				// We need to find the corresponding HMD entry and remove
				// those resources from it.
				HMD hmd;
				var key = resources.Key;
				if (_devices.TryGetValue(key, out hmd))
				{
					hmd.Resources = null;
				}

				_nativeResources.Remove(key);
				resources.Dispose();

				var fn = DeviceDetached;
				if (hmd != null && fn != null)
				{
					fn(hmd);
				}
			}
			finally
			{
				_lock.ExitWriteLock();
			}
		}

		private void RemoveDevice(IDeviceHandle<ISensorDevice, ISensorInfo> handle)
		{
			_lock.EnterWriteLock();
			try
			{
				var key = new DeviceKey(handle.DeviceInfo);
				DeviceResources resources;
				if (!_nativeResources.TryGetValue(key, out resources))
					return;

				RemoveDevice(resources);
			}
			finally
			{
				_lock.ExitWriteLock();
			}
		}

	}
}