using System;
using SharpDX;

namespace RiftDotNet
{
	/// <summary>
	/// Represents a head mounted display.
	/// There will only ever be one instance *per* physical HMD. This instance
	/// remains valid, even when the physical HMD is detached and subsequently
	/// attached (as long as there is only one HMDManager).
	/// </summary>
	public interface IHMD
	{
		/// <summary>
		/// Information about this device.
		/// Can be used to find out which coordinates a window should be placed at, so
		/// that the content of the window is actually shown on the device's screen.
		/// </summary>
		IHMDInfo Info { get; }

		/// <summary>
		/// This event is emitted when the physical HMD has been attached
		/// to the computer (again). Orientation, acceleration and angular velocity
		/// will only be valid as long as the device is attached, afterwards they will
		/// return the last valid value.
		/// </summary>
		event Action<IHMD> Attached;

		/// <summary>
		/// This event is emitted when the physical HMD has been detached from
		/// the computer.
		/// </summary>
		event Action<IHMD> Detached;

		/// <summary>
		/// Whether or not the physical HMD is currently attached to the computer
		/// and recoginized by the software.
		/// </summary>
		bool IsAttached { get; }

		///<summary>
		/// Obtain the current accumulated orientation. 
		///</summary>
		Quaternion Orientation { get; }

		///<summary>
		/// ???
		///</summary>
		Quaternion PredictedOrientation { get; }

		///<summary>
		/// Obtain the last absolute acceleration reading, in m/s^2.
		///</summary>
		Vector3 Acceleration { get; }

		///<summary>
		/// Obtain the last angular velocity reading, in rad/s.
		///</summary>
		Vector3 AngularVelocity { get; }

		///<summary>
		/// Gain used to correct gyro with accel. Default value is appropriate for typical use.
		///</summary>
		float AccelGain { get; set; }

		///<summary>
		/// Multiplier for yaw rotation (turning); setting this higher than 1 (the default) can allow the game
		/// to be played without auxillary rotation controls, possibly making it more immersive. Whether this is more
		/// or less likely to cause motion sickness is unknown.
		///</summary>
		float YawMultiplier { get; set; }

		/// <summary>
		/// Whether or not orientation is to be predicted ahead of time, or not.
		/// </summary>
		bool IsPredictionEnabled { get; set; }

		/// <summary>
		/// The amount of time the orientation is predicted ahead, based
		/// on acceleration and angular velocity.
		/// </summary>
		/// <remarks>
		/// Should be a bit smaller than the average rendering latency.
		/// </remarks>
		TimeSpan PredictionTime { get; set; }

		///<summary>
		/// Resets the current orientation and acceleration.
		///</summary>
		void Reset();
	}
}
