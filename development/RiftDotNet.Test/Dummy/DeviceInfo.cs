namespace RiftDotNet.Test.Dummy
{
	public sealed class DeviceInfo : IDeviceInfo
	{
		public DeviceType InfoClassType { get; set; }
		public DeviceType Type { get; set; }
		public string ProductName { get; set; }
		public string Manufacturer { get; set; }
		public uint Version { get; set; }
	}
}