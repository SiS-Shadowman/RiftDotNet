#pragma once

#include "RiftDotNet.h"
#include "Message.h"




namespace RiftDotNet
{
	namespace Platform
	{
		public ref class MessageLatencyTestButton
			: public Message
		{
		public:

			MessageLatencyTestButton(const OVR::MessageLatencyTestButton& message)
				: Message(message)
			{}
		};
	}
}