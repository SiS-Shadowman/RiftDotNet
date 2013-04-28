#pragma once

#include <OVR_Device.h>

#include "RiftDotNet.h"

using namespace System;
using namespace System::ComponentModel;




namespace RiftDotNet
{
	namespace Platform
	{
		ref class Helper
		{
		public:

			static RiftDotNet::CoordinateFrame FromNative(OVR::SensorDevice::CoordinateFrame frame)
			{
				switch(frame)
				{
				case OVR::SensorDevice::Coord_Sensor:
					return RiftDotNet::CoordinateFrame::Sensor;

				case OVR::SensorDevice::Coord_HMD:
					return RiftDotNet::CoordinateFrame::HMD;

				default:
					throw gcnew InvalidEnumArgumentException("frame", (int)frame, OVR::SensorDevice::CoordinateFrame::typeid);
				}
			}

			static OVR::SensorDevice::CoordinateFrame ToNative(RiftDotNet::CoordinateFrame frame)
			{
				
				switch(frame)
				{
				case OVR::SensorDevice::Coord_Sensor:
					return OVR::SensorDevice::Coord_Sensor;

				case OVR::SensorDevice::Coord_HMD:
					return OVR::SensorDevice::Coord_HMD;

				default:
					throw gcnew InvalidEnumArgumentException("frame", (int)frame, OVR::SensorDevice::CoordinateFrame::typeid);
				}
			}

			static SharpDX::Color FromNative(OVR::Color color)
			{
				return SharpDX::Color(color.R, color.G, color.B, color.A);
			}

			static MessageType FromNative(OVR::MessageType type)
			{
				switch(type)
				{
				case OVR::Message_None:
					return MessageType::None;

				case OVR::Message_BodyFrame:
					return MessageType::BodyFrame;

				case OVR::Message_DeviceAdded:
					return MessageType::DeviceAdded;

				case OVR::Message_DeviceRemoved:
					return MessageType::DeviceRemoved;

				case OVR::Message_LatencyTestColorDetected:
					return MessageType::LatencyTestColorDetected;

				case OVR::Message_LatencyTestSamples:
					return MessageType::LatencyTestSamples;

				case OVR::Message_LatencyTestStarted:
					break;

				case OVR::Message_LatencyTestButton:
					return MessageType::LatencyTestButton;

				default:
					throw gcnew ArgumentException(String::Format("Unknown message type: {0}", (int)type));
				}

				// Why does the compiler not detect the exception?!
				// As if the default label above will magically be jumped over
				// when executing this code
				// (╯°□°）╯︵ ┻━┻
				return MessageType::None;
			}

			static OVR::SensorRange ToNative(SensorRange range)
			{
				OVR::SensorRange ret;
				ret.MaxAcceleration = range.MaxAcceleration;
				ret.MaxRotationRate = range.MaxRotationRate;
				ret.MaxMagneticField = range.MaxMagneticField;
				return ret;
			}

			static SensorRange FromNative(OVR::SensorRange range)
			{
				SensorRange ret;
				ret.MaxAcceleration = range.MaxAcceleration;
				ret.MaxRotationRate = range.MaxRotationRate;
				ret.MaxMagneticField = range.MaxMagneticField;
				return ret;
			}
		};
	}
}