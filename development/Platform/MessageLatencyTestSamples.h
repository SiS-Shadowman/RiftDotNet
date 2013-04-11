#pragma once

#include "RiftDotNet.h"
#include "Message.h"
#include "Helper.h"

using namespace SharpDX;




namespace RiftDotNet
{
	namespace Platform
	{
		public ref class MessageLatencyTestSamples
			: public Message
		{
		public:

			MessageLatencyTestSamples(const OVR::MessageLatencyTestSamples& message)
				: Message(message)
			{
				auto samples = message.Samples;
				_samples = gcnew array<Color>((int)samples.GetSize());
				for(int i = 0; i < _samples->Length; ++i)
				{
					_samples[i] = Helper::FromNative(samples[i]);
				}
			}

			property array<Color>^ Samples
			{
				virtual array<Color>^ get() { return _samples; }
			}

		private:

			array<Color>^ _samples;
		};
	}
}