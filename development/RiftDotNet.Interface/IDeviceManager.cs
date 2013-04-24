namespace RiftDotNet
{
	/// <summary>
	///     DeviceManager maintains and provides access to devices supported by OVR, such as
	///     HMDs and sensors. A single instance of DeviceManager is normally created at
	///     program startup, allowing devices to be enumerated and created.
	/// </summary>
	public interface IDeviceManager
		: IDevice
	{
		/// <summary>
		/// The list of all sensor devices, one for each sensor, currently
		/// attached to this system.
		/// </summary>
		DisposableArray<IDeviceHandle<ISensorDevice, ISensorInfo>> SensorDevices { get; }

		/// <summary>
		/// The list of all HMD devices, one for each HMD, currently
		/// attached to this system.
		/// </summary>
		DisposableArray<IDeviceHandle<IHMDDevice, IHMDInfo>> HMDDevices { get; }
	}
}