using System;
using System.Threading;
using SharpDX;

namespace RiftDotNet
{
	internal sealed class HMD
		: IHMD
	{
		private IHMDInfo _info;
		private readonly ReaderWriterLockSlim _lock;
		private DeviceResources _resources;

		private bool _resetOutstanding;

		#region Settings

		private bool _isPredictionEnabled;
		private TimeSpan _predictionTime;
		private float _yawMultiplier;
		private float _accelGain;

		#endregion

		#region Properties

		private Vector3 _acceleration;
		private Vector3 _angularVelocity;
		private Quaternion _orientation;
		private Quaternion _predictedOrientation;

		#endregion

		public HMD(IHMDInfo info, ReaderWriterLockSlim @lock)
		{
			if (info == null)
				throw new ArgumentNullException();

			if (@lock == null)
				throw new ArgumentNullException();

			_info = info;
			_lock = @lock;
		}

		internal DeviceResources Resources
		{
			set
			{
				_lock.EnterWriteLock();
				try
				{
					if (value == _resources)
						return;

					if (_resources != null)
					{
						Action<IHMD> fn = Detached;
						if (fn != null)
							fn(this);
					}

					_resources = value;

					if (_resources != null)
					{
						if (_resetOutstanding)
						{
							_resources.Fusion.Reset();
							_resetOutstanding = false;
						}

						// It may be possible that a newly attached device gets
						// a different port, display position, whatever...
						_info = _resources.Info;

						// This is even more interesting. The user may have changed
						// setting like yaw multiplier and whatnot: We want to apply
						// those settings again.
						_resources.Fusion.YawMultiplier = _yawMultiplier;
						_resources.Fusion.IsPredictionEnabled = _isPredictionEnabled;
						_resources.Fusion.PredictionTime = _predictionTime;
						_resources.Fusion.AccelGain = _accelGain;

						Action<IHMD> fn = Attached;
						if (fn != null)
							fn(this);
					}
				}
				finally
				{
					_lock.ExitWriteLock();
				}
			}
		}

		#region IHMD Members

		public IHMDInfo Info
		{
			get { return _info; }
		}

		public event Action<IHMD> Attached;
		public event Action<IHMD> Detached;

		public bool IsAttached
		{
			get { return _resources != null; }
		}

		public Quaternion Orientation
		{
			get
			{
				_lock.EnterWriteLock();
				try
				{
					if (IsAttached)
						_orientation = _resources.Fusion.Orientation;

					return _orientation;
				}
				finally
				{
					_lock.ExitWriteLock();
				}
			}
		}

		public Quaternion PredictedOrientation
		{
			get
			{
				_lock.EnterWriteLock();
				try
				{
					if (IsAttached)
						_predictedOrientation = _resources.Fusion.Orientation;

					return _predictedOrientation;
				}
				finally
				{
					_lock.ExitWriteLock();
				}
			}
		}

		public Vector3 Acceleration
		{
			get
			{
				_lock.EnterWriteLock();
				try
				{
					if (IsAttached)
						_acceleration = _resources.Fusion.Acceleration;

					return _acceleration;
				}
				finally
				{
					_lock.ExitWriteLock();
				}
			}
		}

		public Vector3 AngularVelocity
		{
			get
			{
				_lock.EnterWriteLock();
				try
				{
					if (IsAttached)
						_angularVelocity = _resources.Fusion.AngularVelocity;

					return _angularVelocity;
				}
				finally
				{
					_lock.ExitWriteLock();
				}
			}
		}

		public float AccelGain
		{
			get
			{
				_lock.EnterWriteLock();
				try
				{
					if (IsAttached)
						_accelGain = _resources.Fusion.AccelGain;

					return _accelGain;
				}
				finally
				{
					_lock.ExitWriteLock();
				}
			}
			set
			{
				_lock.EnterWriteLock();
				try
				{
					if (IsAttached)
						_resources.Fusion.AccelGain = value;

					_accelGain = value;
				}
				finally
				{
					_lock.ExitWriteLock();
				}
			}
		}

		public float YawMultiplier
		{
			get
			{
				_lock.EnterWriteLock();
				try
				{
					if (IsAttached)
						_accelGain = _resources.Fusion.AccelGain;

					return _accelGain;
				}
				finally
				{
					_lock.ExitWriteLock();
				}
			}
			set
			{
				_lock.EnterWriteLock();
				try
				{
					if (IsAttached)
						_resources.Fusion.AccelGain = value;

					_accelGain = value;
				}
				finally
				{
					_lock.ExitWriteLock();
				}
			}
		}

		public bool IsPredictionEnabled
		{
			get
			{
				_lock.EnterWriteLock();
				try
				{
					if (IsAttached)
						_isPredictionEnabled = _resources.Fusion.IsPredictionEnabled;

					return _isPredictionEnabled;
				}
				finally
				{
					_lock.ExitWriteLock();
				}
			}
			set
			{
				_lock.EnterWriteLock();
				try
				{
					if (IsAttached)
						_resources.Fusion.IsPredictionEnabled = value;

					_isPredictionEnabled = value;
				}
				finally
				{
					_lock.ExitWriteLock();
				}
			}
		}

		public TimeSpan PredictionTime
		{
			get
			{
				_lock.EnterWriteLock();
				try
				{
					if (IsAttached)
						_predictionTime = _resources.Fusion.PredictionTime;

					return _predictionTime;
				}
				finally
				{
					_lock.ExitWriteLock();
				}
			}
			set
			{
				_lock.EnterWriteLock();
				try
				{
					if (IsAttached)
						_resources.Fusion.PredictionTime = value;

					_predictionTime = value;
				}
				finally
				{
					_lock.ExitWriteLock();
				}
			}
		}

		public void Reset()
		{
			_lock.EnterWriteLock();
			try
			{
				if (IsAttached)
				{
					_resources.Fusion.Reset();
					_orientation = _resources.Fusion.Orientation;
					_predictedOrientation = _resources.Fusion.PredictedOrientation;
					_acceleration = _resources.Fusion.Acceleration;
					_angularVelocity = _resources.Fusion.AngularVelocity;
				}
				else
				{
					_resetOutstanding = true;
					_orientation = Quaternion.Identity;
					_predictedOrientation = Quaternion.Identity;
					_acceleration = new Vector3();
					_angularVelocity = new Vector3();
				}
			}
			finally
			{
				_lock.EnterWriteLock();
			}
		}

		#endregion
	}
}