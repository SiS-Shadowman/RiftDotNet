#include "stdafx.h"
#include "MessageHandler.h"
#include "MessageBodyFrame.h"
#include "MessageDeviceStatus.h"
#include "MessageLatencyTestButton.h"
#include "MessageLatencyTestColorDetected.h"
#include "MessageLatencyTestSamples.h"
#include "MessageLatencyTestStarted.h"




namespace RiftDotNet
{
	namespace Platform
	{
		IMessage^ MessageHandler::CreateMessage(const OVR::Message& message)
		{
			switch(message.Type)
			{
				case OVR::Message_None:
					return nullptr;

				case OVR::Message_BodyFrame:
					return gcnew MessageBodyFrame(static_cast<const OVR::MessageBodyFrame&>(message));

				case OVR::Message_DeviceAdded:
				case OVR::Message_DeviceRemoved:
					return gcnew MessageDeviceStatus(static_cast<const OVR::MessageDeviceStatus&>(message));

				case OVR::Message_LatencyTestColorDetected:
					return gcnew MessageLatencyTestColorDetected(static_cast<const OVR::MessageLatencyTestColorDetected&>(message));

				case OVR::Message_LatencyTestSamples:
					return gcnew MessageLatencyTestSamples(static_cast<const OVR::MessageLatencyTestSamples&>(message));

				case OVR::Message_LatencyTestStarted:
					return gcnew MessageLatencyTestStarted(static_cast<const OVR::MessageLatencyTestStarted&>(message));

				case OVR::Message_LatencyTestButton:
					return gcnew MessageLatencyTestButton(static_cast<const OVR::MessageLatencyTestButton&>(message));

				default:
					throw gcnew ArgumentException(String::Format("Unknown message type: {0}", (int)message.Type));
			}
		}
	}
}