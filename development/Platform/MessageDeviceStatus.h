#pragma once

#include "RiftDotNet.h"
#include "Message.h"
#include "DeviceHandle.h"




namespace RiftDotNet
{
	namespace Platform
	{
		public ref class MessageDeviceStatus
			: public Message
			, public IMessageDeviceStatus
		{
		public:

			MessageDeviceStatus(const OVR::MessageDeviceStatus& message)
				: Message(message)
			{
				auto tmp = const_cast<OVR::MessageDeviceStatus&>(message);

				auto wrapper = new DeviceHandleWrapper(tmp.Handle);
				_handle = gcnew Platform::DeviceHandle(wrapper);
			}

			~MessageDeviceStatus()
			{
				if (_handle != nullptr)
				{
					delete _handle;
					_handle = nullptr;
				}
			}

			property IDeviceHandle^ DeviceHandle
			{
				virtual IDeviceHandle^ get()
				{
					return _handle;
				}
			}

		private:

			IDeviceHandle^ _handle;
		};
	}
}