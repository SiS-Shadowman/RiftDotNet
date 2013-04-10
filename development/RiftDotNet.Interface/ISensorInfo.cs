using System;

namespace RiftDotNet
{
	public interface ISensorInfo
		: IDeviceInfo
	{
		UInt16 VendorId { get; }
		UInt16 ProductId { get; }
		string SerialNumber { get; }
	}
}