#pragma once

#include <OVR_Device.h>
#include <OVR_DeviceImpl.h>

#include "RiftDotNet.h"
#include "System.h"
#include "DeviceBase.h"
#include "DeviceHandle.h"
#include "DeviceEnumerable.h"
#include "HMDDevice.h"
#include "SensorDevice.h"
#include "ValueWrapper.h"

using namespace System::Collections::Generic;



namespace RiftDotNet
{
	namespace Platform
	{
		public ref class DeviceManager
			: public DeviceBase
			, public IDeviceManager
		{
		private:

			static log4net::ILog^ Log = log4net::LogManager::GetLogger(System::Reflection::MethodBase::GetCurrentMethod()->DeclaringType);

		public:

			static DeviceManager^ Create()
			{
				auto native = OVR::DeviceManager::Create();
				return gcnew DeviceManager(native);
			}

			DeviceManager(OVR::DeviceManager* native)
				: DeviceBase(native)
			{}

			~DeviceManager()
			{
				
			}

			property RiftDotNet::IDeviceInfo^ Info
			{
				virtual RiftDotNet::IDeviceInfo^ get() override
				{
					if (IsDisposed)
						throw gcnew ObjectDisposedException("IHMDDevice");

					OVR::DeviceInfo info;
					GetNative<OVR::DeviceManager>()->GetDeviceInfo(&info);
					return gcnew RiftDotNet::Platform::DeviceInfo(info);
				}
			}

			/// The enumeration of all sensor devices
			property DisposableArray<IDeviceHandle<ISensorDevice^, ISensorInfo^>^>^ SensorDevices
			{
				virtual DisposableArray<IDeviceHandle<ISensorDevice^, ISensorInfo^>^>^ get()
				{
					if (IsDisposed)
						throw gcnew ObjectDisposedException("IDeviceHandle");

					auto enumerator = GetNative<OVR::DeviceManager>()->EnumerateDevices<OVR::SensorDevice>(true);
					auto ret = gcnew List<IDeviceHandle<ISensorDevice^, ISensorInfo^>^>();

					Log->DebugFormat("Enumerating ISensorDevices");
					Log->DebugFormat("DeviceEnumerator<>.IsAvailable: {0}", enumerator.IsAvailable());
					Log->DebugFormat("DeviceEnumerator<>.DeviceType: {0}", (DeviceType)enumerator.GetType());

					while(enumerator.GetType() != OVR::Device_None)
					{
						Log->DebugFormat("Found a device, adding handle to enumeration...");
						auto wrapper = new EnumeratorWrapper<OVR::DeviceEnumerator<OVR::SensorDevice>>(enumerator);
						ret->Add(gcnew TypedDeviceHandle<ISensorDevice^,ISensorInfo^>(wrapper));

						if (!enumerator.Next())
							break;

						Log->DebugFormat("DeviceEnumerator<>.IsAvailable: {0}", enumerator.IsAvailable());
						Log->DebugFormat("DeviceEnumerator<>.DeviceType: {0}", (DeviceType)enumerator.GetType());
					}

					if (ret->Count == 0)
						Log->InfoFormat("Unable to find any ISensorDevices");

					return gcnew DisposableArray<IDeviceHandle<ISensorDevice^, ISensorInfo^>^>(ret->ToArray());
				}
			}

			/// The enumeration of all HMD devices.
			property DisposableArray<IDeviceHandle<IHMDDevice^, IHMDInfo^>^>^ HMDDevices
			{
				virtual DisposableArray<IDeviceHandle<IHMDDevice^, IHMDInfo^>^>^ get()
				{
					if (IsDisposed)
						throw gcnew ObjectDisposedException("IDeviceHandle");

					auto enumerator = GetNative<OVR::DeviceManager>()->EnumerateDevices<OVR::HMDDevice>(true);
					auto ret = gcnew List<IDeviceHandle<IHMDDevice^,IHMDInfo^>^>();

					Log->DebugFormat("Enumerating ISensorDevices");
					Log->DebugFormat("DeviceEnumerator<>.IsAvailable: {0}", enumerator.IsAvailable());
					Log->DebugFormat("DeviceEnumerator<>.DeviceType: {0}", (DeviceType)enumerator.GetType());

					while(enumerator.GetType() != OVR::Device_None)
					{
						Log->DebugFormat("Found a device, adding handle to enumeration...");
						auto wrapper = new EnumeratorWrapper<OVR::DeviceEnumerator<OVR::HMDDevice>>(enumerator);
						ret->Add(gcnew TypedDeviceHandle<IHMDDevice^,IHMDInfo^>(wrapper));

						if (!enumerator.Next())
							break;

						Log->DebugFormat("DeviceEnumerator<>.IsAvailable: {0}", enumerator.IsAvailable());
						Log->DebugFormat("DeviceEnumerator<>.DeviceType: {0}", (DeviceType)enumerator.GetType());
					}

					if (ret->Count == 0)
						Log->InfoFormat("Unable to find any IHMDDevices");

					return gcnew DisposableArray<IDeviceHandle<IHMDDevice^, IHMDInfo^>^>(ret->ToArray());
				}
			}

		private:
		};
	}
}