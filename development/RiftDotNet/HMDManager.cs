using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using SharpDX;

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

		public HMDManager()
		{
			_lock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
			_manager = Factory.CreateDeviceManager();
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
						handle.Reset();
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
			}
			finally
			{
				_lock.ExitWriteLock();
			}
		}

		#endregion

		private void DeviceChanged(IMessageDeviceStatus message)
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
					if (handle.DeviceType == DeviceType.HMD)
					{
						RemoveDevice((IDeviceHandle<IHMDDevice, IHMDInfo>) handle);
					}
				}
			}
		}

		private void AddDevice(IDeviceHandle<IHMDDevice, IHMDInfo> handle)
		{
			_lock.EnterWriteLock();
			try
			{
				var key = new DeviceKey(handle.DeviceInfo);
				DeviceResources resources;
				if (_nativeResources.TryGetValue(key, out resources))
				{
					// So this case should not really take place, but...
					RemoveDevice(resources);
				}

				resources = new DeviceResources(handle);
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
				var key = new DeviceKey(resources.Info);
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

		private void RemoveDevice(IDeviceHandle<IHMDDevice, IHMDInfo> handle)
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

		#region Nested type: DeviceKey

		private struct DeviceKey : IEquatable<DeviceKey>
		{
			public readonly DeviceType DeviceType;
			public readonly string Manufacturer;
			public readonly string ProductName;
			public readonly uint Version;

			public DeviceKey(IDeviceInfo deviceInfo)
			{
				DeviceType = deviceInfo.Type;
				ProductName = deviceInfo.ProductName;
				Manufacturer = deviceInfo.Manufacturer;
				Version = deviceInfo.Version;
			}

			#region IEquatable<DeviceKey> Members

			public bool Equals(DeviceKey other)
			{
				return DeviceType == other.DeviceType && string.Equals(ProductName, other.ProductName) &&
				       string.Equals(Manufacturer, other.Manufacturer) && Version == other.Version;
			}

			#endregion

			public override bool Equals(object obj)
			{
				if (ReferenceEquals(null, obj)) return false;
				return obj is DeviceKey && Equals((DeviceKey) obj);
			}

			public override int GetHashCode()
			{
				unchecked
				{
					var hashCode = (int) DeviceType;
					hashCode = (hashCode*397) ^ (ProductName != null ? ProductName.GetHashCode() : 0);
					hashCode = (hashCode*397) ^ (Manufacturer != null ? Manufacturer.GetHashCode() : 0);
					hashCode = (hashCode*397) ^ (int) Version;
					return hashCode;
				}
			}

			public static bool operator ==(DeviceKey left, DeviceKey right)
			{
				return left.Equals(right);
			}

			public static bool operator !=(DeviceKey left, DeviceKey right)
			{
				return !left.Equals(right);
			}
		}

		#endregion

		#region Nested type: DeviceResources

		private sealed class DeviceResources
			: IDisposable
		{
			internal readonly IHMDDevice Device;
			internal readonly ISensorFusion Fusion;
			internal readonly IHMDInfo Info;
			internal readonly ISensorDevice Sensor;

			public DeviceResources(IDeviceHandle<IHMDDevice, IHMDInfo> handle)
			{
				Info = handle.DeviceInfo;
				Device = handle.CreateDevice();
				Sensor = Device.Sensor;
				Fusion = Factory.CreateSensorFusion(Sensor);
			}

			#region IDisposable Members

			public void Dispose()
			{
				Fusion.Dispose();
				Sensor.Dispose();
				Device.Dispose();
			}

			#endregion
		}

		#endregion

		#region Nested type: HMD

		private sealed class HMD
			: IHMD
		{
			private IHMDInfo _info;
			private readonly ReaderWriterLockSlim _lock;
			private DeviceResources _resources;

			#region Properties

			private Vector3 _acceleration;
			private Vector3 _angularVelocity;
			private Quaternion _orientation;
			private bool _resetOutstanding;

			public HMD(IHMDInfo info, ReaderWriterLockSlim @lock)
			{
				if (info == null)
					throw new ArgumentNullException();

				if (@lock == null)
					throw new ArgumentNullException();

				_info = info;
				_lock = @lock;
			}

			#endregion

			internal DeviceResources Resources
			{
				set
				{
					_lock.EnterWriteLock();
					try
					{
						if (value == _resources)
							return;

						if (_resources != null)
						{
							Action<IHMD> fn = Detached;
							if (fn != null)
								fn(this);
						}

						_resources = value;

						if (_resources != null)
						{
							if (_resetOutstanding)
							{
								_resources.Fusion.Reset();
								_resetOutstanding = false;
							}

							// It may be possible that a newly attached device gets
							// a different port.
							_info = _resources.Info;

							Action<IHMD> fn = Attached;
							if (fn != null)
								fn(this);
						}
					}
					finally
					{
						_lock.ExitWriteLock();
					}
				}
			}

			#region IHMD Members

			public IHMDInfo Info
			{
				get { return _info; }
			}

			public event Action<IHMD> Attached;
			public event Action<IHMD> Detached;

			public bool IsAttached
			{
				get { return _resources != null; }
			}

			public Quaternion Orientation
			{
				get
				{
					// This needs to be locked because the expression "_resources.Fusion.XYZ"
					// is not atomic. It is very much possible that another thread sets the field
					// _resource to NULL.
					_lock.EnterReadLock();
					try
					{
						if (IsAttached)
							_orientation = _resources.Fusion.PredictedOrientation;

						return _orientation;
					}
					finally
					{
						_lock.ExitReadLock();
					}
				}
			}

			public Vector3 Acceleration
			{
				get
				{
					// This needs to be locked because the expression "_resources.Fusion.XYZ"
					// is not atomic. It is very much possible that another thread sets the field
					// _resource to NULL.
					_lock.EnterReadLock();
					try
					{
						if (IsAttached)
							_acceleration = _resources.Fusion.Acceleration;

						return _acceleration;
					}
					finally
					{
						_lock.ExitReadLock();
					}
				}
			}

			public Vector3 AngularVelocity
			{
				get
				{
					// This needs to be locked because the expression "_resources.Fusion.XYZ"
					// is not atomic. It is very much possible that another thread sets the field
					// _resource to NULL.
					_lock.EnterReadLock();
					try
					{
						if (IsAttached)
							_angularVelocity = _resources.Fusion.AngularVelocity;

						return _angularVelocity;
					}
					finally
					{
						_lock.ExitReadLock();
					}
				}
			}

			public void Reset()
			{
				_lock.EnterWriteLock();
				try
				{
					if (IsAttached)
						_resources.Fusion.Reset();
					else
						_resetOutstanding = true;

					_orientation = Quaternion.Identity;
					_acceleration = new Vector3();
					_angularVelocity = new Vector3();
				}
				finally
				{
					_lock.EnterWriteLock();
				}
			}

			#endregion
		}

		#endregion

		#region Nested type: InternalMessageHandler

		private sealed class InternalMessageHandler
			: MessageHandler
		{
			private readonly HMDManager _manager;

			public InternalMessageHandler(HMDManager manager)
			{
				_manager = manager;
			}

			public override void OnMessage(IMessage message)
			{
				switch (message.Type)
				{
					case MessageType.DeviceAdded:
					case MessageType.DeviceRemoved:
						_manager.DeviceChanged((IMessageDeviceStatus) message);
						break;
				}
			}

			public override bool SupportsMessageType(MessageType type)
			{
				return true;
			}
		}

		#endregion
	}
}