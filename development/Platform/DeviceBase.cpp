#include "stdafx.h"
#include "DeviceBase.h"
#include "DeviceManager.h"
#include "HMDDevice.h"
#include "SensorDevice.h"
#include "MessageHandler.h"
#include "MessageHandlerImpl.h"

using namespace System;




namespace RiftDotNet
{
	namespace Platform
	{
		DeviceBase::~DeviceBase()
		{
			if (_native != nullptr)
			{
				// So that we can actually delete our handler-wrapper
				_native->SetMessageHandler(nullptr);
			}

			if (_handler != nullptr)
			{
				delete _handler;
				_handler = nullptr;
			}

			if (_native != nullptr)
				_native->Release();

			_native = nullptr;
			_handler = nullptr;
		}

		RiftDotNet::MessageHandler^ DeviceBase::MessageHandler::get()
		{
			auto native = dynamic_cast<Platform::MessageHandler*>(_native->GetMessageHandler());
			if (native == nullptr)
				return nullptr;

			return native->ManagedHandler()->Managed;
		}

		void DeviceBase::MessageHandler::set(RiftDotNet::MessageHandler^ handler)
		{
			// The gist is as follows:
			// The very same handler can be installed on multiple devices, however it's only
			// necessary to create exactly *one* native OVR::MessageHandler per instance.
			// Hence we will re-use the existing wrapper, because it does exactly that.
			auto wrapper = handler != nullptr
				? (handler->Impl != nullptr ? dynamic_cast<RiftDotNet::Platform::MessageHandlerImpl^>(handler->Impl) : gcnew RiftDotNet::Platform::MessageHandlerImpl(handler))
				: nullptr;

			auto native = wrapper != nullptr
				? wrapper->GetNative()
				: nullptr;

			_native->SetMessageHandler(native);
		}

		DeviceBase^ DeviceBase::Create(OVR::DeviceBase* native)
		{
			if (native == nullptr)
				return nullptr;

			switch(native->GetType())
			{
			case OVR::Device_Manager:
				return gcnew DeviceManager((OVR::DeviceManager*)native);

			case OVR::Device_HMD:
				return gcnew HMDDevice((OVR::HMDDevice*)native);

			case OVR::Device_Sensor:
				return gcnew SensorDevice((OVR::SensorDevice*)native);

			case OVR::Device_LatencyTester:
				throw gcnew NotImplementedException();

			default:
				throw gcnew ArgumentException(String::Format("Unknown device type: {0}", (int)native->GetType()));
			}
		}
	}
}