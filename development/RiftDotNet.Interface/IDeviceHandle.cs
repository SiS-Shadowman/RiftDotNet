using System;

namespace RiftDotNet
{
	/// <summary>
	/// A handle for a device attached to this computer.
	/// It is created during device enumeration and can be used in order
	/// to create device objects.
	/// </summary>
	public interface IDeviceHandle
		: IDisposable
	{
		/// <summary>
		/// Whether or not an object representing this device has been created already.
		/// </summary>
		bool IsCreated { get; }

		/// <summary>
		/// Whether or not this device is available.
		/// ???
		/// </summary>
		bool IsAvailable { get; }

		/// <summary>
		/// The type of this device.
		/// For now, this can either be a sensor or an HMD device.
		/// </summary>
		DeviceType DeviceType { get; }

		/// <summary>
		/// The description of this device.
		/// For now, this can either be an ISensorInfo or IHMDInfo implementation
		/// (corresponds with DeviceType).
		/// </summary>
		IDeviceInfo DeviceInfo { get; }

		/// <summary>
		/// Creates a device object in order to control this device.
		/// </summary>
		/// <returns></returns>
		IDevice CreateDevice();

		/// <summary>
		/// Whether or not this object has already been disposed of.
		/// </summary>
		/// <remarks>
		/// When disposed, all properties and methods besides Dispose() and
		/// IsDisposed will throw an ObjectDisposedException, when invoked.
		/// </remarks>
		bool IsDisposed { get; }
	}

	/// <summary>
	/// Generic Interface, typed to a specific device.
	/// </summary>
	/// <typeparam name="TDevice"></typeparam>
	/// <typeparam name="TDeviceInfo"></typeparam>
	public interface IDeviceHandle<out TDevice, out TDeviceInfo>
		: IDeviceHandle
		where TDevice : IDevice
	{
		new TDevice CreateDevice();

		new TDeviceInfo DeviceInfo { get; }
	}
}