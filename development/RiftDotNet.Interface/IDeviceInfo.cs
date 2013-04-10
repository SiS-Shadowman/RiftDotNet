using System;

namespace RiftDotNet
{
	/// <summary>
	/// DeviceInfo describes a device and its capabilities, obtained by calling
	/// GetDeviceInfo. This base class only contains device-independent functionality;
	/// users will normally use a derived HMDInfo or SensorInfo classes for more
	/// extensive device info.
	/// </summary>
	public interface IDeviceInfo
		: IDisposable
	{
		/// <summary>
		/// // Type of device for which DeviceInfo is intended.
		/// This will be set to Device.HMD for IHMDInfo, note that this may be
		/// different form the actual device type since (Device.None) is valid.
		/// </summary>
		DeviceType InfoClassType { get; }

		/// <summary>
		/// Type of device this describes. This must be the same as InfoClassType when
		/// InfoClassType != Device.None.
		/// </summary>
		DeviceType Type { get; }

		/// <summary>
		/// Name string describing the product: "Oculus Rift DK1", etc.
		/// </summary>
		string ProductName { get; }

		/// <summary>
		/// Name string describing the manufacturer of the product.
		/// </summary>
		string Manufacturer { get; }

		/// <summary>
		/// Version number of the physical or virtual device.
		/// </summary>
		uint Version { get; }
	}
}