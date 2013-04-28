namespace RiftDotNet
{
	public interface IHMDDevice
		: IDevice
	{
		new IHMDInfo Info { get; }
		ISensorDevice Sensor { get; }
	}
}