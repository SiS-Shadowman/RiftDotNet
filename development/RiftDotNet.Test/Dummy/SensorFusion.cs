using System;
using SharpDX;

namespace RiftDotNet.Test.Dummy
{
	public class SensorFusion : ISensorFusion
	{
		private ISensorDevice _sensorDevice;
		private bool _isDisposed;
		private Quaternion _orientation;
		private Quaternion _predicted;
		private Vector3 _acceleration;
		private Vector3 _angularVelocity;

		public SensorFusion(ISensorDevice sensorDevice)
		{
			if (sensorDevice == null)
				throw new ArgumentNullException();

			_sensorDevice = sensorDevice;
		}

		#region ISensorFusion Members

		public void Dispose()
		{
			_isDisposed = true;
		}

		public bool IsDisposed
		{
			get { return _isDisposed; }
		}

		public bool IsAttachedToSensor
		{
			get { return _sensorDevice != null; }
		}

		public bool IsGravityEnabled
		{
			get { return true; }
		}

		public ISensorDevice AttachedDevice
		{
			get { return _sensorDevice; }
			set { _sensorDevice = value; }
		}

		public void Reset()
		{
			_orientation = Quaternion.Identity;
			_predicted = Quaternion.Identity;
			_acceleration = Vector3.Zero;
			_angularVelocity = Vector3.Zero;
		}

		public Quaternion Orientation
		{
			get { return _orientation; }
		}

		public Quaternion PredictedOrientation
		{
			get { return _predicted; }
		}

		public Vector3 Acceleration
		{
			get { return _acceleration; }
		}

		public Vector3 AngularVelocity
		{
			get { return _angularVelocity; }
		}

		public float AccelGain { get; set; }

		public float YawMultiplier { get; set; }

		public bool IsPredictionEnabled { get; set; }

		public TimeSpan PredictionTime { get; set; }

		#endregion
	}
}