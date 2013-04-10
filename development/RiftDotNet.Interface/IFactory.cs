namespace RiftDotNet
{
	public interface IFactory
	{
		IDeviceManager CreateDeviceManager();

		ISensorFusion CreateSensorFusion(ISensorDevice sensorDevice);
	}
}