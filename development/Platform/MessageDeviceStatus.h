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

				// It would be possible to avoid creating a DeviceHandle,
				// however there's no point because then another allocation
				// for OVR::DeviceHandle would be required.
				// The overhead of (possibly needlessly) creating a device handle
				// seems like the better alternative.
				_handle = gcnew Platform::DeviceHandle(&tmp.Handle);
			}

			property IDeviceHandle^ DeviceHandle
			{
				virtual IDeviceHandle^ get() { return _handle; }
			}

		private:

			IDeviceHandle^ _handle;
		};
	}
}