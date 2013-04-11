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
				return _native == tmp->_native;
			}

			virtual int GetHashCode() override sealed
			{
#ifdef _WIN64
				// TODO: Maybe use boost hashing?
				auto value = reinterpret_cast<unsigned long long>(_native);
				auto upper = (int)((value & 0xFFFFFFFF00000000) >> 32);
				auto lower = (int)(value & 0x00000000FFFFFFFF);
				auto hashed = upper ^ lower;
				return hashed;
#else
				static_assert(sizeof(void*) == 4, "Unknown platform");
				return reinterpret_cast<int>(_native);
#endif
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