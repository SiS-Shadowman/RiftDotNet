using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading;

namespace RiftDotNet.Test.Dummy
{
	public sealed class DeviceManager
		: IDeviceManager
	{
		private readonly List<ISensorDevice> _sensors;
		private readonly List<IHMDDevice> _hmds;

		public DeviceManager()
		{
			_sensors = new List<ISensorDevice>();
			_hmds = new List<IHMDDevice>();
		}

		public void SetHmds(IEnumerable<IHMDDevice> devices)
		{
			var tmp = devices.ToList();
			_sensors.Clear();
			_hmds.Clear();

			foreach(var hmd in tmp)
			{
				_sensors.Add(hmd.Sensor);
				_hmds.Add(hmd);
			}
		}

		#region Simulation

		#region Fields

		private bool _exit;
		private MessageHandler _handler;
		private bool _isDisposed;
		private Thread _worker;

		#endregion

		/// <summary>
		/// 
		/// </summary>
		/// <param name="wait">The amount of time the background thread waits, before it adds the new dummy device</param>
		/// <param name="block"></param>
		public void SimulateAddOneDevice(TimeSpan wait, bool block = true)
		{
			Join();
			_worker = new Thread(() =>
				{
					Thread.Sleep(wait);

					var hmd = CreateHMD();
					var handle = new DeviceHandle<IHMDDevice, IHMDInfo>(hmd);
					var msg = new MessageDeviceStatus
					{
						Device = this,
						DeviceHandle = handle,
						Type = MessageType.DeviceAdded
					};

					_handler.OnMessage(msg);
				});
			_worker.Start();

			if (block)
				Join();
		}

		public void SimulateDeviceAddAndRemove(TimeSpan amount)
		{
			Join();
			_worker = new Thread(() => DeviceAddAndRemove(amount));
			_worker.Start();
			Join();
		}

		#region Device Add and Remove

		private void DeviceAddAndRemove(TimeSpan amount)
		{
			var sw = new Stopwatch();
			sw.Start();

			while (!_exit && sw.Elapsed < amount)
			{
				//
				// Let's add the device
				//
				var hmd = CreateHMD();
				var handle = new DeviceHandle<IHMDDevice, IHMDInfo>(hmd);
				var msg = new MessageDeviceStatus
					{
						Device = this,
						DeviceHandle = handle,
						Type = MessageType.DeviceAdded
					};

				_handler.OnMessage(msg);

				//
				// And remove it again
				//

				msg = new MessageDeviceStatus
					{
						Device = this,
						DeviceHandle = handle,
						Type = MessageType.DeviceRemoved
					};
				_handler.OnMessage(msg);
			}
		}

		[Pure]
		public SensorDevice CreateSensor()
		{
			var info = new SensorInfo
				{
					Manufacturer = "Test",
					ProductName = "Test",
					Version = 1,
					SerialNumber = Guid.NewGuid().ToString(),
				};
			var sensor = new SensorDevice(this, info)
				{
					CoordinateFrame = CoordinateFrame.HMD,
					Range = new SensorRange(1, 2, 3),
				};
			return sensor;
		}

		[Pure]
		public HMDDevice CreateHMD()
		{
			SensorDevice sensor = CreateSensor();
			var info = new HMDInfo
				{
					ProductName = "Test",
					Manufacturer = "Test",
					Version = 1,
				};

			var hmd = new HMDDevice(this, sensor, info);
			return hmd;
		}

		#endregion

		#endregion

		#region IDeviceManager Members

		public void Dispose()
		{
			Join();
			_isDisposed = true;
		}

		public bool Equals(IDevice other)
		{
			return ReferenceEquals(this, other);
		}

		public DeviceType Type
		{
			get { return DeviceType.HMD; }
		}

		public IDevice Parent
		{
			get { return null; }
		}

// ReSharper disable ConvertToAutoProperty
		public MessageHandler MessageHandler
// ReSharper restore ConvertToAutoProperty
		{
			get { return _handler; }
			set { _handler = value; }
		}

		public IDeviceInfo Info
		{
			get { return new DeviceInfo(); }
		}

		public bool IsDisposed
		{
			get { return _isDisposed; }
		}

		public uint RefCount
		{
			get { return 1; }
		}

		public DisposableArray<IDeviceHandle<ISensorDevice, ISensorInfo>> SensorDevices
		{
			get
			{
				return new DisposableArray<IDeviceHandle<ISensorDevice, ISensorInfo>>(
					(from sensor in _sensors
					select new DeviceHandle<ISensorDevice, ISensorInfo>(sensor)));
			}
		}

		public DisposableArray<IDeviceHandle<IHMDDevice, IHMDInfo>> HMDDevices
		{
			get
			{
				return new DisposableArray<IDeviceHandle<IHMDDevice, IHMDInfo>>(
					(from hmd in _hmds
						 select new DeviceHandle<IHMDDevice, IHMDInfo>(hmd)));
			}
		}

		#endregion

		private void Join()
		{
			if (_worker != null)
			{
				_exit = true;
				_worker.Join();
				_worker = null;
			}
		}
	}
}