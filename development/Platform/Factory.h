#pragma once

#include "RiftDotNet.h"
#include "DeviceManager.h"
#include "SensorFusion.h"
#include "HMDInfo.h"
#include "SensorInfo.h"
#include "UnitTestDetector.h"




namespace RiftDotNet
{
	namespace Platform
	{
		/// <summary>
		/// Internal IFactory implementation.
		/// There can only be one factory and it takes care of initializing the
		/// OVR system and creates the necessary objects.
		/// </summary>
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

			static property Factory^ Instance { Factory^ get() { return _theOne; } }

			static Factory()
			{
				// For a reason I cannot fathom, the fucking
				// resharper/nunit test runner refuses to load log4net.
				// Well, if it aint gonna behave, then nobody is getting logging...
				if (UnitTestDetector::IsRunningFromNunit)
				{
					OVR::System::Init();
				}
				else
				{
					// Yeah, who's gonna delete that?
					auto log = new Log4Net();
					OVR::System::Init(log);
				}

				_theOne = gcnew Factory();
			}

		private:

			Factory()
			{}

		private:

			static Factory^ _theOne;
		};
	}
}