#pragma once

#include "RiftDotNet.h"
#include "DeviceManager.h"
#include "SensorFusion.h"




namespace RiftDotNet
{
	namespace Platform
	{
		public ref class Factory sealed
			: public IFactory
		{
		public:

			virtual IDeviceManager^ CreateDeviceManager()
			{
				return DeviceManager::Create();
			}

			virtual ISensorFusion^ CreateSensorFusion(ISensorDevice^ device)
			{
				return gcnew SensorFusion(device);
			}
		};
	}
}