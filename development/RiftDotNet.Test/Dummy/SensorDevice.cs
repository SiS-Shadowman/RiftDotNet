namespace RiftDotNet.Test.Dummy
{
	public sealed class SensorDevice
		: ISensorDevice
	{
		private readonly IDevice _parent;
		private readonly SensorInfo _info;
		private bool _isDisposed;

		public SensorDevice(IDevice parent, SensorInfo info)
		{
			_parent = parent;
			_info = info;
		}

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
			get { return DeviceType.Sensor; }
		}

		public IDevice Parent
		{
			get { return _parent; }
		}

		public MessageHandler MessageHandler { get; set; }

		IDeviceInfo IDevice.Info { get{return _info;} }

		public ISensorInfo Info
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

		public CoordinateFrame CoordinateFrame { get; set; }

		public SensorRange Range { get; set; }
	}
}