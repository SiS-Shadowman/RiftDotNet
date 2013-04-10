#pragma once

#include <OVR_DeviceHandle.h>

#include "DeviceBase.h"

using namespace System;




namespace RiftDotNet
{
	namespace Platform
	{
		public ref class DeviceHandle
			: public IDeviceHandle
		{
		public:

			DeviceHandle(OVR::DeviceHandle* native)
			{
				if (native == nullptr)
					throw gcnew ArgumentNullException("native");

				_native = native;
			}

			~DeviceHandle()
			{
				delete _native;
				_native = nullptr;
			}

			property bool IsCreated
			{
				virtual bool get() { return _native->IsCreated(); }
			}

			property bool IsAvailable
			{
				virtual bool get() { return _native->IsAvailable(); }
			}

			property DeviceType DeviceType
			{
				virtual RiftDotNet::DeviceType get() { return (RiftDotNet::DeviceType)_native->GetType(); }
			}

			virtual IDevice^ CreateDevice()
			{
				return DeviceBase::Create(_native->CreateDevice());
			}

		protected:

			property OVR::DeviceHandle* Native { OVR::DeviceHandle* get() { return _native; } }

		private:

			OVR::DeviceHandle* _native;
		};
	}
}