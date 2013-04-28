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
		/// Obtains a reference to the parent of this device.
		/// Every time this reference is obtained, it must be released as well.
		/// </summary>
		IDevice Parent { get; }

		/// <summary>
		/// The message handler installed on this device.
		/// </summary>
		MessageHandler MessageHandler { get; set; }

		/// <summary>
		/// Information about this device.
		/// </summary>
		IDeviceInfo Info { get; }

		/// <summary>
		/// Whether or not this object has already been disposed of.
		/// </summary>
		/// <remarks>
		/// When disposed, all properties and methods besides Dispose() and
		/// IsDisposed will throw an ObjectDisposedException, when invoked.
		/// </remarks>
		bool IsDisposed { get; }

		/// <summary>
		/// The reference count of the underlying native resource.
		/// </summary>
		/// <remarks>
		/// Returns 0 when this instance has been disposed of.
		/// However this does not mean that all instances to the same device
		/// have been disposed of.
		/// </remarks>
		UInt32 RefCount { get; }
	}
}