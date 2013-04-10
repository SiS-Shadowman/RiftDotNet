#pragma once

#include <OVR_Device.h>

#include "RiftDotNet.h"




namespace RiftDotNet
{
	namespace Platform
	{
		ref class Helper
		{
		public:

			static OVR::SensorRange ToNative(SensorRange range)
			{
				OVR::SensorRange ret;
				ret.MaxAcceleration = range.MaxAcceleration;
				ret.MaxRotationRate = range.MaxRotationRate;
				ret.MaxMagneticField = range.MaxMagneticField;
				return ret;
			}

			static SensorRange FromNative(OVR::SensorRange range)
			{
				SensorRange ret;
				ret.MaxAcceleration = range.MaxAcceleration;
				ret.MaxRotationRate = range.MaxRotationRate;
				ret.MaxMagneticField = range.MaxMagneticField;
				return ret;
			}
		};
	}
}