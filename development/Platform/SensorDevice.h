#pragma once

#include <OVR_Device.h>

#include "RiftDotNet.h"
#include "DeviceBase.h"
#include "Helper.h"




namespace RiftDotNet
{
	namespace Platform
	{
		public ref class SensorDevice sealed
			: public DeviceBase
			, public ISensorDevice
		{
		public:

			SensorDevice(OVR::SensorDevice* native)
				: DeviceBase(native)
			{}

			property CoordinateFrame CoordinateFrame
			{
				virtual RiftDotNet::CoordinateFrame get()
				{ 
					return (RiftDotNet::CoordinateFrame)_native->GetCoordinateFrame();
				}
				virtual void set(RiftDotNet::CoordinateFrame f)
				{
					_native->SetCoordinateFrame((OVR::SensorDevice::CoordinateFrame)f);
				}
			}

			property SensorRange Range
			{
				virtual SensorRange get()
				{
					OVR::SensorRange tmp;
					_native->GetRange(&tmp);
					return Helper::FromNative(tmp);
				}

				virtual void set(SensorRange range)
				{
					if (!_native->SetRange(Helper::ToNative(range)))
					{
						throw gcnew Exception("Unable to change the SensorRange: Maybe the parameters are out of range?");
					}
				}
			}

			property OVR::SensorDevice* Native
			{
				OVR::SensorDevice* get() { return _native; }
			}

		private:

			OVR::SensorDevice* _native;
		};
	}
}