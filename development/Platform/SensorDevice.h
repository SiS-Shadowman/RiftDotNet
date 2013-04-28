#pragma once

#include <OVR_Device.h>

#include "RiftDotNet.h"
#include "DeviceBase.h"
#include "Helper.h"
#include "SensorInfo.h"




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
					if (IsDisposed)
						throw gcnew ObjectDisposedException("ISensorDevice");

					auto tmp = _native->GetCoordinateFrame();
					return Helper::FromNative(tmp);
				}
				virtual void set(RiftDotNet::CoordinateFrame f)
				{
					if (IsDisposed)
						throw gcnew ObjectDisposedException("ISensorDevice");

					auto tmp = Helper::ToNative(f);
					_native->SetCoordinateFrame(tmp);
				}
			}

			property RiftDotNet::IDeviceInfo^ Info
			{
				virtual RiftDotNet::IDeviceInfo^ get() override { return Info1; }
			}

			property RiftDotNet::ISensorInfo^ Info1
			{
				virtual RiftDotNet::ISensorInfo^ get() = RiftDotNet::ISensorDevice::Info::get
				{
					if (IsDisposed)
						throw gcnew ObjectDisposedException("IHMDDevice");

					OVR::SensorInfo info;
					GetNative<OVR::SensorDevice>()->GetDeviceInfo(&info);
					return gcnew RiftDotNet::Platform::SensorInfo(info);
				}
			}

			property SensorRange Range
			{
				virtual SensorRange get()
				{
					if (IsDisposed)
						throw gcnew ObjectDisposedException("ISensorDevice");

					OVR::SensorRange tmp;
					_native->GetRange(&tmp);
					return Helper::FromNative(tmp);
				}

				virtual void set(SensorRange range)
				{
					if (IsDisposed)
						throw gcnew ObjectDisposedException("ISensorDevice");

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