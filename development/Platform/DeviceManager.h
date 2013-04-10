#pragma once

#include <OVR_Device.h>

#include "RiftDotNet.h"
#include "System.h"
#include "DeviceBase.h"
#include "DeviceHandle.h"
#include "DeviceEnumerable.h"
#include "HMDDevice.h"
#include "SensorDevice.h"

using namespace System::Collections::Generic;



namespace RiftDotNet
{
	namespace Platform
	{
		public ref class DeviceManager
			: public DeviceBase
			, public IDeviceManager
		{
		public:

			static DeviceManager^ Create()
			{
				SystemInitializer();
				auto native = OVR::DeviceManager::Create();
				return gcnew DeviceManager(native);
			}

			DeviceManager(OVR::DeviceManager* native)
				: DeviceBase(native)
			{}

			/// The enumeration of all sensor devices
			property array<IDeviceHandle<ISensorDevice^>^>^ SensorDevices
			{
				virtual array<IDeviceHandle<ISensorDevice^>^>^ get()
				{
					auto enumerator = GetNative<OVR::DeviceManager>()->EnumerateDevices<OVR::SensorDevice>(false);
					auto ret = gcnew List<IDeviceHandle<ISensorDevice^>^>();

					while(enumerator.Next())
					{
						auto clone = new OVR::DeviceHandle(enumerator);
						ret->Add(gcnew TypedDeviceHandle<ISensorDevice^>(clone));
					}

					return ret->ToArray();
				}
			}

			/// The enumeration of all HMD devices.
			property array<IDeviceHandle<IHMDDevice^>^>^ HMDDevices
			{
				virtual array<IDeviceHandle<IHMDDevice^>^>^ get()
				{
					auto enumerator = GetNative<OVR::DeviceManager>()->EnumerateDevices<OVR::HMDDevice>(false);
					auto ret = gcnew List<IDeviceHandle<IHMDDevice^>^>();

					while(enumerator.Next())
					{
						auto clone = new OVR::DeviceHandle(enumerator);
						ret->Add(gcnew TypedDeviceHandle<IHMDDevice^>(clone));
					}

					return ret->ToArray();
				}
			}

		private:
		};
	}
}