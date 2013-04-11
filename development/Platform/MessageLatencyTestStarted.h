#pragma once

#include "RiftDotNet.h"
#include "Message.h"
#include "Helper.h"

using namespace SharpDX;



namespace RiftDotNet
{
	namespace Platform
	{
		public ref class MessageLatencyTestStarted
			: public Message
		{
		public:

			MessageLatencyTestStarted(const OVR::MessageLatencyTestStarted& message)
				: Message(message)
			{
				_targetValue = Helper::FromNative(message.TargetValue);
			}

			property Color TargetValue
			{
				virtual Color get() { return _targetValue; }
			}

		private:

			Color _targetValue;
		};
	}
}