#pragma once

#include <OVR_DeviceMessages.h>

#include "RiftDotNet.h"
#include "Helper.h"
#include "DeviceBase.h"




namespace RiftDotNet
{
	namespace Platform
	{
		public ref class Message abstract
			: public IMessage
		{
		public:

			Message(const OVR::Message& message)
				: _type(message.Type)
				, _device(message.pDevice)
			{}

			property MessageType Type
			{
				virtual MessageType get() sealed
				{
					return Helper::FromNative(_type);
				}
			}

			property IDevice^ Device
			{
				virtual IDevice^ get() sealed
				{
					return DeviceBase::Create(_device);
				}
			}

		private:

			OVR::MessageType _type;
			OVR::DeviceBase* _device;
		};
	}
}