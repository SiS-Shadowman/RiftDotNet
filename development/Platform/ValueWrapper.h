#pragma once

#include <OVR_DeviceHandle.h>

#include "RiftDotNet.h"




namespace RiftDotNet
{
	namespace Platform
	{
		class HandleWrapper
		{
		public:

			virtual ~HandleWrapper()
			{}

			virtual OVR::DeviceHandle& Handle() = 0;
		};

		class DeviceHandleWrapper
			: public HandleWrapper
		{
		public:

			DeviceHandleWrapper(const OVR::DeviceHandle& value)
				: Value(value)
			{}

			~DeviceHandleWrapper()
			{}

			OVR::DeviceHandle& Handle()
			{
				return Value;
			}

			OVR::DeviceHandle Value;
		};

		template <typename T>
		class EnumeratorWrapper
			: public HandleWrapper
		{
		public:

			EnumeratorWrapper(const T& value)
				: Value(value)
			{}

			~EnumeratorWrapper()
			{}

			OVR::DeviceHandle& Handle()
			{
				return Value;
			}

			T Value;
		};
	}
}