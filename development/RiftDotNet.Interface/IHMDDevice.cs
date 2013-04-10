namespace RiftDotNet
{
	public interface IHMDDevice
		: IDevice
	{
		ISensorDevice Sensor { get; }
	}
}