#pragma once

#include "RiftDotNet.h"
#include "DeviceManager.h"
#include "SensorFusion.h"
#include "HMDInfo.h"
#include "SensorInfo.h"




namespace RiftDotNet
{
	namespace Platform
	{
		public ref class Factory sealed
			: public ITestingFactory
		{
		public:

			virtual log4net::ILog^ GetLogger()
			{
				return Rift::Log;
			}

			virtual IHMDInfo^ CreateHMDInfo()
			{
				return gcnew HMDInfo();
			}

			virtual ISensorInfo^ CreateSensorInfo()
			{
				return gcnew SensorInfo();
			}

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