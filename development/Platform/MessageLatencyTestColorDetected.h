#pragma once

#include <OVR_Device.h>

#include "RiftDotNet.h"
#include "Message.h"
#include "Helper.h"

using namespace System;
using namespace SharpDX;




namespace RiftDotNet
{
	namespace Platform
	{
		public ref class MessageLatencyTestColorDetected
			: public Message
		{
		public:

			MessageLatencyTestColorDetected(const OVR::MessageLatencyTestColorDetected& message)
				: Message(message)
			{
				_elapsed = TimeSpan::FromMilliseconds(message.Elapsed);
				_detectedValue = Helper::FromNative(message.DetectedValue);
				_targetValue = Helper::FromNative(message.TargetValue);
			}

			property TimeSpan Elapsed { virtual TimeSpan get() { return _elapsed; } }
			property Color DetectedValue { virtual Color get() { return _detectedValue; } }
			property Color TargetValue { virtual Color get() { return _targetValue; } }

		private:

			TimeSpan _elapsed;
			Color _detectedValue;
			Color _targetValue;
		};
	}
}