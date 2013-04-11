#pragma once

#include "RiftDotNet.h"
#include "Message.h"

using namespace SharpDX;




namespace RiftDotNet
{
	namespace Platform
	{
		public ref class MessageBodyFrame sealed
			: public Message
			, public IMessageBodyFrame
		{
		public:

			MessageBodyFrame(const OVR::MessageBodyFrame& message)
				: Message(message)
			{
			}

			property Vector3 Acceleration
			{
				virtual Vector3 get() { return _acceleration; }
			}

			property Vector3 RotationRate
			{
				virtual Vector3 get() { return _rotationRate; }
			}

			property Vector3 MagneticField
			{
				virtual Vector3 get() { return _magneticField; }
			}

			property float Temperature
			{
				virtual float get() { return _temperature; }
			}

			property float TimeDelta
			{
				virtual float get() { return _timeDelta; }
			}

		private:

			Vector3 _acceleration;
			Vector3 _rotationRate;
			Vector3 _magneticField;
			float    _temperature;
			float    _timeDelta;
		};
	}
}