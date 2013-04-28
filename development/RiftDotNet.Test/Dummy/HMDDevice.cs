namespace RiftDotNet.Test.Dummy
{
	public sealed class HMDDevice
		: IHMDDevice
	{
		private readonly IDevice _parent;
		private readonly ISensorDevice _sensor;
		private bool _isDisposed;
		private readonly HMDInfo _info;

		public HMDDevice(IDevice parent, ISensorDevice sensor, HMDInfo info)
		{
			_sensor = sensor;
			_parent = parent;
			_info = info;
		}

		#region IHMDDevice Members

		public void Dispose()
		{
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
			get { return _parent; }
		}

		public MessageHandler MessageHandler { get; set; }

		IDeviceInfo IDevice.Info { get{return _info;} }

		public IHMDInfo Info
		{
			get { return _info; }
		}

		public bool IsDisposed
		{
			get { return _isDisposed; }
		}

		public uint RefCount
		{
			get { return 1; }
		}

		public ISensorDevice Sensor
		{
			get { return _sensor; }
		}

		#endregion
	}
}