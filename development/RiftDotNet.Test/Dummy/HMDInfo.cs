namespace RiftDotNet.Test.Dummy
{
	public sealed class HMDInfo
		: IHMDInfo
	{
		public DeviceType InfoClassType { get { return DeviceType.HMD; } }
		public DeviceType Type { get { return DeviceType.HMD;} }
		public string ProductName { get; set; }
		public string Manufacturer { get; set; }
		public uint Version { get; set; }
		public uint HResolution { get; set; }
		public uint VResolution { get; set; }
		public float HScreenSize { get;  set; }
		public float VScreenSize { get; set; }
		public float VScreenCenter { get; set; }
		public float EyeToScreenDistance { get; set; }
		public float LensSeparationDistance { get; set; }
		public float InterpupillaryDistance { get; set; }
		public float[] DistortionK { get; set; }
		public int DesktopX { get; set; }
		public int DesktopY { get; set; }
		public object DisplayDevice { get; set; }
	}
}