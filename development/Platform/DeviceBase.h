#pragma once

#include <memory>
#include <OVR_Device.h>

#include "RiftDotNet.h"

using namespace System;




namespace RiftDotNet
{
	namespace Platform
	{
		class MessageHandler;

		public ref class DeviceBase abstract
			: public IDevice
		{
			static log4net::ILog^ Log = log4net::LogManager::GetLogger(System::Reflection::MethodBase::GetCurrentMethod()->DeclaringType);

		public:

			DeviceBase(OVR::DeviceBase* native);
			!DeviceBase();
			virtual ~DeviceBase();

			property DeviceType Type
			{
				virtual DeviceType get()
				{
					if (IsDisposed)
						throw gcnew ObjectDisposedException("IDevice");

					return (DeviceType)_native->GetType();
				}
			}

			property IDevice^ Parent
			{
				virtual IDevice^ get()
				{
					if (IsDisposed)
						throw gcnew ObjectDisposedException("IDevice");

					auto native = _native->GetParent();
					if (native == nullptr)
						return nullptr;

					native->AddRef();
					return Create(native);
				}
			}

			property bool IsDisposed
			{
				virtual bool get()
				{
					return _native == nullptr;
				}
			}

			property UInt32 RefCount
			{
				virtual UInt32 get()
				{
					if (IsDisposed)
						return 0;

					return _native->GetRefCount();
				}
			}

			property RiftDotNet::MessageHandler^ MessageHandler
			{
				virtual RiftDotNet::MessageHandler^ get();
				virtual void set(RiftDotNet::MessageHandler^ handler);
			}

			property RiftDotNet::IDeviceInfo^ Info
			{
				virtual RiftDotNet::IDeviceInfo^ get() abstract;
			}


			virtual bool Equals(Object^ other) override sealed
			{
				if (other == nullptr)
					return false;

				auto tmp = dynamic_cast<DeviceBase^>(other);
				if (tmp == nullptr)
					return false;

				// For now, I will assume that the ver same OVR::DeviceBase pointer
				// is used for the same device. But maybe we need to compare the device
				// id or something similar...
				return _equalityHandle == tmp->_equalityHandle;
			}

			virtual int GetHashCode() override sealed
			{
				return _equalityHandle.GetHashCode();
			}

			virtual bool Equals(IDevice^ other) sealed
			{
				return Equals((Object^)other);
			}

		internal:

			static DeviceBase^ Create(OVR::DeviceBase* native);

			generic<typename T> where T : IDevice
			static T Create(OVR::DeviceBase* native)
			{
				auto tmp = Create(native);
				return (T)tmp;
			}

			property OVR::DeviceBase* Native
			{
				OVR::DeviceBase* get()
				{
					if (IsDisposed)
						throw gcnew ObjectDisposedException("IDeviceBase");

					return _native;
				}
			}

			template <typename T>
			T* GetNative()
			{
				if (IsDisposed)
					throw gcnew ObjectDisposedException("IDeviceBase");

				return static_cast<T*>(_native);
			}

		private:

			IntPtr _equalityHandle;
			OVR::DeviceBase* _native;
			RiftDotNet::Platform::MessageHandler* _handler;
		};
	}
}