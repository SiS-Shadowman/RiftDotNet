using System;
using SharpDX;

namespace RiftDotNet
{
	/// <summary>
	/// Represents one head mounted display.
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

		bool IsAttached { get; }

		Quaternion Orientation { get; }

		Vector3 Acceleration { get; }

		Vector3 AngularVelocity { get; }

		void Reset();
	}
}
