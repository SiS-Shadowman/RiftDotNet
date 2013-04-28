namespace RiftDotNet
{
	/// <summary>
	/// SensorDevice is an interface to sensor data.
	/// Install a MessageHandler of SensorDevice instance to receive MessageBodyFrame
	/// notifications.
	/// </summary>
	public interface ISensorDevice
		: IDevice
	{
		new ISensorInfo Info { get; }

		/// <summary>
		/// CoordinateFrame defines whether messages come in the coordinate frame
		/// of the sensor device or HMD, which has a different internal sensor.
		/// Sensors obtained form the HMD will automatically use HMD coordinates.
		/// </summary>
		CoordinateFrame CoordinateFrame { get; set; }

		/// <summary>
		/// Sets maximum range settings for the sensor described by SensorRange.    
		/// The function will fail if you try to pass values outside Maximum supported
		/// by the HW, as described by SensorInfo.
		/// </summary>
		/// <remarks>
		/// These may not exactly match the values applied through set.
		/// </remarks>
		SensorRange Range
		{
			get;
			set;
		}
	}
}