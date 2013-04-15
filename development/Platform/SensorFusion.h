#pragma once

#include <OVR_SensorFusion.h>

#include "RiftDotNet.h"
#include "SensorDevice.h"

using namespace SharpDX;




namespace RiftDotNet
{
	namespace Platform
	{
		public ref class SensorFusion
			: public ISensorFusion
		{
		public:

			SensorFusion()
			{
				_native = new OVR::SensorFusion();

				IsPredictionEnabled = false;
			}

			SensorFusion(ISensorDevice^ device)
			{
				if (device != nullptr)
				{
					_native = new OVR::SensorFusion(((SensorDevice^)device)->Native);
				}
				else
				{
					_native = new OVR::SensorFusion();
				}
			}

			~SensorFusion()
			{
				delete _native;
				_native = nullptr;
			}
		
			/// <summary>
			/// Returns true if this Sensor fusion object is attached to a sensor.
			/// </summary>
			property bool IsAttachedToSensor
			{
				virtual bool get() { return _native->IsAttachedToSensor(); }
			}

			/// <summary>
			/// ???
			/// </summary>
			property bool IsGravityEnabled
			{
				virtual bool get() { return _native->IsGravityEnabled(); }
				virtual void set(bool b) { _native->SetGravityEnabled(b); }
			}

			/// <summary>
			/// Attaches this SensorFusion to a sensor device, from which it will receive
			/// notification messages. If a sensor is attached, manual message notification
			/// is not necessary. Calling this function also resets SensorFusion state.
			/// </summary>
			property ISensorDevice^ AttachedDevice
			{
				virtual ISensorDevice^ get() { return _sensor; }
				virtual void set(ISensorDevice^ value)
				{
					auto native = value != nullptr ? ((SensorDevice^)value)->Native : nullptr;
					if (!_native->AttachToSensor(native))
					{
						throw gcnew System::InvalidOperationException("Attaching this sensor to the given device was not possible. Mabe the sensor already has a device attached to it?");
					}

					_sensor = value;
				}
			}

			///<summary>
			/// Resets the current orientation and acceleration.
			///</summary>
			virtual void Reset()
			{
				_native->Reset();
			}
		
			///<summary>
			/// Obtain the current accumulated orientation. 
			///</summary>
			property Quaternion Orientation
			{
				virtual Quaternion get() { return FromNative(_native->GetOrientation()); }
			}
		
			///<summary>
			/// ???
			///</summary>
			property Quaternion PredictedOrientation
			{
				virtual Quaternion get() { return FromNative(_native->GetPredictedOrientation()); }
			}
		
			///<summary>
			/// Obtain the last absolute acceleration reading, in m/s^2.
			///</summary>
			property Vector3 Acceleration
			{
				virtual Vector3 get() { return FromNative(_native->GetAcceleration()); }
			}
		
			///<summary>
			/// Obtain the last angular velocity reading, in rad/s.
			///</summary>
			property Vector3 AngularVelocity
			{
				virtual Vector3 get() { return FromNative(_native->GetAngularVelocity()); }
			}

			///<summary>
			/// Gain used to correct gyro with accel. Default value is appropriate for typical use.
			///</summary>
			property float AccelGain
			{
				virtual float get() { return _native ->GetAccelGain(); }
				virtual void set(float f) { _native->SetAccelGain(f); }
			}
		
			///<summary>
			/// Multiplier for yaw rotation (turning); setting this higher than 1 (the default) can allow the game
			/// to be played without auxillary rotation controls, possibly making it more immersive. Whether this is more
			/// or less likely to cause motion sickness is unknown.
			///</summary>
			property float YawMultiplier
			{
				virtual float get() { return _native->GetYawMultiplier(); }
				virtual void set(float f) { _native->SetYawMultiplier(f); }
			}

			property bool IsPredictionEnabled
			{
				virtual bool get()
				{ 
					// TODO: I want to be able to retrieve that value from OVR::SensorFusion
					return _predictionEnabled;
				}
				virtual void set(bool b)
				{
					_predictionEnabled = b;
					PredictionTime = PredictionTime;
				}
			}

			property TimeSpan PredictionTime
			{
				virtual TimeSpan get() { return TimeSpan::FromMilliseconds(_native->GetPredictionDelta()); }
				virtual void set(TimeSpan value) { _native->SetPrediction((float)value.TotalMilliseconds, _predictionEnabled); }
			}

		private:

			static Vector3 FromNative(const OVR::Vector3f& other)
			{
				Vector3 v;
				v.X = other.x;
				v.Y = other.y;
				v.Z = other.z;
				return v;
			}

			static Quaternion FromNative(const OVR::Quatf& other)
			{
				Quaternion quat;
				quat.X = other.x;
				quat.Y = other.y;
				quat.Z = other.z;
				quat.W = other.w;
				return quat;
			}

		private:

			OVR::SensorFusion* _native;
			ISensorDevice^ _sensor;
			bool _predictionEnabled;
		};
	}
}