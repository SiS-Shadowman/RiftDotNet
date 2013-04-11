namespace RiftDotNet
{
	/// <summary>
	/// MessageType identifies the structure of the Message class; based on the message,
	/// casting can be used to obtain the exact value.
	/// </summary>
	public enum MessageType
	{
		// Used for unassigned message types.
		None = 0,

		/// <summary>
		/// A new device is detected by manager.
		/// </summary>
		DeviceAdded,

		/// <summary>
		/// Existing device has been plugged/unplugged.
		/// </summary>
		DeviceRemoved,

		/// <summary>
		/// Emitted by sensor at regular intervals.
		/// </summary>
		BodyFrame,

		LatencyTestSamples,
		LatencyTestColorDetected,
		LatencyTestStarted,
		LatencyTestButton,
	};
}