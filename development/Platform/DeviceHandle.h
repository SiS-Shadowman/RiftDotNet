#pragma once

#include <OVR_DeviceHandle.h>

#include "RiftDotNet.h"
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

			property bool IsDisposed
			{
				virtual bool get()
				{
					return _native == nullptr;
				}
			}

			property bool IsCreated
			{
				virtual bool get() sealed
				{
					if (_native == nullptr)
						throw gcnew ObjectDisposedException("IDeviceHandle");

					return _native->IsCreated();
				}
			}

			property bool IsAvailable
			{
				virtual bool get() sealed
				{
					if (_native == nullptr)
						throw gcnew ObjectDisposedException("IDeviceHandle");

					return _native->IsAvailable();
				}
			}

			property DeviceType DeviceType
			{
				virtual RiftDotNet::DeviceType get() sealed
				{
					if (_native == nullptr)
						throw gcnew ObjectDisposedException("IDeviceHandle");

					return (RiftDotNet::DeviceType)_native->GetType();
				}
			}

			property IDeviceInfo^ DeviceInfo
			{
				virtual IDeviceInfo^ get() sealed;
			}

			virtual IDevice^ CreateDevice() sealed
			{
				if (_native == nullptr)
					throw gcnew ObjectDisposedException("IDeviceHandle");

				return DeviceBase::Create(_native->CreateDevice());
			}

		protected:

			property OVR::DeviceHandle* Native { OVR::DeviceHandle* get() { return _native; } }

		private:

			void GetDeviceInfo(OVR::DeviceInfo& info);

		private:

			OVR::DeviceHandle* _native;
		};
	}
}