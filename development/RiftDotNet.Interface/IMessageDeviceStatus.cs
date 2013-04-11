namespace RiftDotNet
{
	/// <summary>
	/// Sent when we receive a device status changes.
	/// </summary>
	public interface IMessageDeviceStatus
		: IMessage
	{
		/// <summary>
		/// A handle to the device.
		/// Allows to retrieve its description.
		/// </summary>
		IDeviceHandle DeviceHandle { get; }
	}
}