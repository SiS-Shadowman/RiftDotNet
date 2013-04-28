namespace RiftDotNet.Test.Dummy
{
	public sealed class SensorInfo
		: ISensorInfo
	{
		#region ISensorInfo Members

		public DeviceType InfoClassType
		{
			get
			{
				return DeviceType.Sensor;
				;
			}
		}

		public DeviceType Type
		{
			get { return DeviceType.Sensor; }
		}

		public string ProductName { get; set; }
		public string Manufacturer { get; set; }
		public uint Version { get; set; }
		public ushort VendorId { get; set; }
		public ushort ProductId { get; set; }
		public string SerialNumber { get; set; }

		#endregion
	}
}