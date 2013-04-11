using SharpDX;

namespace RiftDotNet
{
	/// <summary>
	/// Sensor BodyFrame notification.
	/// Sensor uses Right-Handed coordinate system to return results, with the following
	/// axis definitions:
	///  - Y Up positive
	///  - X Right Positive
	///  - Z Back Positive
	/// Rotations a counter-clockwise (CCW) while looking in the negative direction
	/// of the axis. This means they are interpreted as follows:
	///  - Roll is rotation around Z, counter-clockwise (tilting left) in XY plane.
	///  - Yaw is rotation around Y, positive for turning left.
	///  - Pitch is rotation around X, positive for pitching up.
	/// </summary>
	public interface IMessageBodyFrame
		: IMessage
	{
		/// <summary>
		/// Acceleration in m/s^2.
		/// </summary>
		Vector3 Acceleration { get; }

		/// <summary>
		/// Angular velocity in rad/s^2.
		/// </summary>
		Vector3 RotationRate { get; }

		/// <summary>
		/// Magnetic field strength in Gauss.
		/// </summary>
		Vector3 MagneticField { get; }

		/// <summary>
		/// Temperature reading on sensor surface, in degrees Celsius.
		/// </summary>
		float Temperature { get; }

		/// <summary>
		/// Time passed since last Body Frame, in seconds.
		/// </summary>
		float TimeDelta { get; }
	}
}