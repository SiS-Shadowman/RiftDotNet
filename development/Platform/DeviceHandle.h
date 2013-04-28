#pragma once

#include <OVR_DeviceHandle.h>

#include "RiftDotNet.h"
#include "DeviceBase.h"
#include "ValueWrapper.h"

using namespace System;




namespace RiftDotNet
{
	namespace Platform
	{
		public ref class DeviceHandle
			: public IDeviceHandle
		{
		private:

			static log4net::ILog^ Log = log4net::LogManager::GetLogger(System::Reflection::MethodBase::GetCurrentMethod()->DeclaringType);

		public:

			DeviceHandle(HandleWrapper* wrapper)
			{
				if (wrapper == nullptr)
					throw gcnew ArgumentNullException("wrapper");

				if (Log->IsDebugEnabled)
				{
					Log->DebugFormat("Wrapping device handle '{0:x}'",
						reinterpret_cast<std::size_t>(wrapper));
				}

				_native = wrapper;
			}

			!DeviceHandle()
			{
				if (_native != nullptr)
				{
					if (Log->IsDebugEnabled)
						Log->DebugFormat("Disposing DeviceHandle '{0:x}', Type: {1}",
						reinterpret_cast<std::size_t>(_native),
						DeviceType);

					delete _native;
					_native = nullptr;
				}
			}

			~DeviceHandle()
			{
				this->!DeviceHandle();
			}

		public:

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

					return _native->Handle().IsCreated();
				}
			}

			property bool IsAvailable
			{
				virtual bool get() sealed
				{
					if (_native == nullptr)
						throw gcnew ObjectDisposedException("IDeviceHandle");

					return _native->Handle().IsAvailable();
				}
			}

			property DeviceType DeviceType
			{
				virtual RiftDotNet::DeviceType get() sealed
				{
					if (_native == nullptr)
						throw gcnew ObjectDisposedException("IDeviceHandle");

					return (RiftDotNet::DeviceType)_native->Handle().GetType();
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

				return DeviceBase::Create(_native->Handle().CreateDevice());
			}

		protected:

			property OVR::DeviceHandle& Native { OVR::DeviceHandle& get() { return _native->Handle(); } }

		private:

			void GetDeviceInfo(OVR::DeviceInfo& info);

		private:

			HandleWrapper* _native;
		};
	}
}