#pragma once

#include "RiftDotNet.h"
#include "DeviceHandle.h"






namespace RiftDotNet
{
	namespace Platform
	{
		generic <typename TDevice, typename TInfo>
		where TDevice : IDevice
		where TInfo : IDeviceInfo
		public ref class TypedDeviceHandle sealed
			: public DeviceHandle
			, public IDeviceHandle<TDevice, TInfo>
		{
		public:

			TypedDeviceHandle(OVR::DeviceHandle* native)
				: DeviceHandle(native)
				, _type(TDevice::typeid)
			{
				Type^ type = GetType((RiftDotNet::DeviceType)native->GetType());
				if (_type != type)
				{
					throw gcnew ArgumentException();
				}
			}

			static Type^ GetType(RiftDotNet::DeviceType type);

			virtual TDevice CreateDevice() new
			{
				return DeviceBase::Create<TDevice>(Native->CreateDevice());
			}

			property TInfo DeviceInfo
			{
				virtual TInfo get() new
				{
					return (TInfo)DeviceHandle::DeviceInfo;
				};
			}

		private:

			const Type^ _type;
		};
	}
}