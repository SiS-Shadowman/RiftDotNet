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

			TypedDeviceHandle(HandleWrapper* wrapper)
				: DeviceHandle(wrapper)
				, _type(TDevice::typeid)
			{
				if (wrapper == nullptr)
					throw gcnew ArgumentNullException("wrapper");

				Type^ type = GetType((RiftDotNet::DeviceType)wrapper->Handle().GetType());
				if (_type != type)
				{
					throw gcnew ArgumentException(
						String::Format("Expected type to be '{0}' but handle reported '{1}' ({2})",
						_type->Name,
						type->Name,
						((RiftDotNet::DeviceType)wrapper->Handle().GetType()).ToString()
						));
				}
			}

			static Type^ GetType(RiftDotNet::DeviceType type);

			virtual TDevice CreateDevice() new
			{
				return DeviceBase::Create<TDevice>(Native.CreateDevice());
			}

			property TInfo DeviceInfo
			{
				virtual TInfo get() new
				{
					return (TInfo)DeviceHandle::DeviceInfo;
				};
			}

		private:

			Type^ _type;
		};
	}
}