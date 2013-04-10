#pragma once

#include <OVR_Device.h>

#include "RiftDotNet.h"
#include "DeviceBase.h"
#include "SensorDevice.h"




namespace RiftDotNet
{
	namespace Platform
	{
		public ref class HMDDevice sealed
			: public DeviceBase
			, public IHMDDevice
		{
		public:

			HMDDevice(OVR::HMDDevice* native)
				: DeviceBase(native)
			{}

			property ISensorDevice^ Sensor
			{
				virtual ISensorDevice^ get()
				{
					return Create<SensorDevice^>(GetNative<OVR::HMDDevice>()->GetSensor());
				}
			}
		};
	}
}
