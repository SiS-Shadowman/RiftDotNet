#pragma once

#include <OVR_Device.h>

#include "RiftDotNet.h"

using namespace System;




namespace RiftDotNet
{
	namespace Platform
	{
		public ref class DeviceBase abstract
			: public IDevice
		{
		public:

			DeviceBase(OVR::DeviceBase* native)
			{
				if (native == nullptr)
					throw gcnew ArgumentNullException("native");

				_native = native;
			}

			~DeviceBase()
			{
				if (_native != nullptr)
					_native->Release();

				_native = nullptr;
			}

			property DeviceType Type
			{
				virtual DeviceType get() { return (DeviceType)_native->GetType(); }
			}

			property IDevice^ Parent
			{
				virtual IDevice^ get()
				{
					auto native = _native->GetParent();
					return Create(native);
				}
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
				OVR::DeviceBase* get() { return _native; }
			}

			template <typename T>
			T* GetNative()
			{
				return static_cast<T*>(_native);
			}

		private:

			OVR::DeviceBase* _native;
		};
	}
}