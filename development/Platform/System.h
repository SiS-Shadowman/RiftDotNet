#pragma once

#include <Kernel/OVR_System.h>

#include "RiftDotNet.h"
#include "Log4Net.h"




namespace RiftDotNet
{
	namespace Platform
	{
		public ref class SystemInitializer sealed
		{
		public:

			static SystemInitializer()
			{
				auto log = new Log4Net();
				OVR::System::Init(log);
			}

		private:
		};
	}
}