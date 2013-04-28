namespace RiftDotNet.Test.Dummy
{
	public class DeviceHandle<TDevice, TInfo>
		: IDeviceHandle<TDevice, TInfo>
		where TDevice : IDevice
		where TInfo : IDeviceInfo
	{
		private readonly TDevice _device;
		private readonly TInfo _info;
		private bool _isDisposed;
		private bool _isCreated;

		public DeviceHandle(TDevice device)
		{
			_device = device;
			_info = (TInfo) device.Info;
		}

		#region IDeviceHandle Members

		public void Dispose()
		{
			_isDisposed = true;
		}

		public bool IsCreated { get { return _isCreated; } }
		public bool IsAvailable { get { return true; } }
		public DeviceType DeviceType { get { return _device.Type; } }
		IDeviceInfo IDeviceHandle.DeviceInfo { get { return DeviceInfo; } }

		IDevice IDeviceHandle.CreateDevice()
		{
			return CreateDevice();
		}

		public TInfo DeviceInfo { get { return _info; } }

		public TDevice CreateDevice()
		{
			_isCreated = true;
			return _device;
		}

		public bool IsDisposed { get { return _isDisposed; } }

		#endregion
	}
}