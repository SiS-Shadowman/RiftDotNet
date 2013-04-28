namespace RiftDotNet.Test.Dummy
{
	public sealed class MessageDeviceStatus
		: Message
		, IMessageDeviceStatus
	{
		public IDeviceHandle DeviceHandle { get; set; }
	}
}