using System;
using SharpDX;

namespace RiftDotNet
{
	/// <summary>
	/// SensorFusion class accumulates Sensor notification messages to keep track of
	/// orientation, which involves integrating the gyro and doing correction with gravity.
	/// Orientation is reported as a quaternion, from which users can obtain either the
	/// rotation matrix or Euler angles.
	///
	/// The class can operate in two ways:
	///  - By user manually passing MessageBodyFrame messages to the OnMessage() function. 
	///  - By attaching SensorFusion to a SensorDevice, in which case it will
	///    automatically handle notifications from that device.
	/// </summary>
	public interface ISensorFusion
		: IDisposable
	{
		/// <summary>
		/// Whether or not this object has already been disposed of.
		/// </summary>
		/// <remarks>
		/// When disposed, all properties and methods besides Dispose() and
		/// IsDisposed will throw an ObjectDisposedException, when invoked.
		/// </remarks>
		bool IsDisposed { get; }

		/// <summary>
		/// Returns true if this Sensor fusion object is attached to a sensor.
		/// </summary>
		bool IsAttachedToSensor { get; }

		/// <summary>
		/// ???
		/// </summary>
		bool IsGravityEnabled { get; }

		/// <summary>
		/// Attaches this SensorFusion to a sensor device, from which it will receive
		/// notification messages. If a sensor is attached, manual message notification
		/// is not necessary. Calling this function also resets SensorFusion state.
		/// </summary>
		ISensorDevice AttachedDevice { get; set; }

		///<summary>
		/// Resets the current orientation and acceleration.
		/// TODO: What about predicted orientation and angular velocity?!
		///</summary>
		void Reset();

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
	}
}