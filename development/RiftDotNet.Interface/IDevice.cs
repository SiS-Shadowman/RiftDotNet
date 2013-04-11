using System;

namespace RiftDotNet
{
	/// <summary>
	/// DeviceBase is the base class for all OVR Devices. It provides the following basic
	/// functionality:
	///   - Reports device type, manager, and associated parent (if any).
	///   - Supports installable message handlers, which are notified of device events.
	///   - Device objects are created through IDeviceHandle.CreateDevice
	///   - Created devices are reference counted, starting with RefCount of 1.
	///   - Device is resources are cleaned up when it is Released, although its handles
	///     may survive longer if referenced.
	/// </summary>
	/// <remarks>
	/// Two IDevice instances are considered equal if they describe the same (physical) device.
	/// </remarks>
	public interface IDevice
		: IDisposable
		, IEquatable<IDevice>
	{
		/// <summary>
		/// The actual type of device this is.
		/// </summary>
		DeviceType Type { get; }

		/// <summary>
		/// ???
		/// TOOD: Explain parent-child relationship
		/// </summary>
		IDevice Parent { get; }
	}
}