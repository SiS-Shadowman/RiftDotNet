using System;

namespace RiftDotNet
{
	internal sealed class DeviceResources
		: IDisposable
	{
		public readonly IHMDDevice Device;
		public readonly ISensorFusion Fusion;
		public readonly IHMDInfo Info;
		public readonly ISensorDevice Sensor;
		public readonly DeviceKey Key;

		public DeviceResources(IFactory factory, IDeviceHandle<IHMDDevice, IHMDInfo> handle)
		{
			if (factory == null || handle == null)
				throw new ArgumentNullException();

			Info = handle.DeviceInfo;
			Device = handle.CreateDevice();
			Sensor = Device.Sensor;
			Fusion = factory.CreateSensorFusion(Sensor);

			if (Info == null)
				throw new ArgumentNullException();
			if (Device == null)
				throw new ArgumentNullException();
			if (Sensor == null)
				throw new ArgumentNullException();

			Key = new DeviceKey(Sensor.Info);
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
}