#pragma once

#include <OVR_Device.h>

#include "RiftDotNet.h"
#include "DeviceBase.h"
#include "SensorDevice.h"
#include "HMDInfo.h"




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

			property RiftDotNet::IDeviceInfo^ Info
			{
				virtual RiftDotNet::IDeviceInfo^ get() override { return Info1; }
			}

			property RiftDotNet::IHMDInfo^ Info1
			{
				virtual RiftDotNet::IHMDInfo^ get() = RiftDotNet::IHMDDevice::Info::get
				{
					if (IsDisposed)
						throw gcnew ObjectDisposedException("IHMDDevice");

					OVR::HMDInfo info;
					GetNative<OVR::HMDDevice>()->GetDeviceInfo(&info);
					return gcnew RiftDotNet::Platform::HMDInfo(info);
				}
			}

			property ISensorDevice^ Sensor
			{
				virtual ISensorDevice^ get()
				{
					if (IsDisposed)
						throw gcnew ObjectDisposedException("IHMDDevice");

					return Create<SensorDevice^>(GetNative<OVR::HMDDevice>()->GetSensor());
				}
			}
		};
	}
}
