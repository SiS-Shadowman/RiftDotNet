#pragma once

#include "DeviceHandle.h"

#include "RiftDotNet.h"





namespace RiftDotNet
{
	namespace Platform
	{
		generic <typename T> where T : IDevice
		public ref class TypedDeviceHandle sealed
			: public DeviceHandle
			, public IDeviceHandle<T>
		{
		public:

			TypedDeviceHandle(OVR::DeviceHandle* native)
				: DeviceHandle(native)
				, _type(T::typeid)
			{
				Type^ type = GetType((RiftDotNet::DeviceType)native->GetType());
				if (_type != type)
				{
					throw gcnew ArgumentException();
				}
			}

			static Type^ GetType(RiftDotNet::DeviceType type);

			virtual T CreateDevice() new
			{
				return DeviceBase::Create<T>(Native->CreateDevice());
			}

		private:

			const Type^ _type;
		};
	}
}