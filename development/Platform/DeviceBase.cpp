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
		DeviceBase::DeviceBase(OVR::DeviceBase* native)
		{
			if (native == nullptr)
				throw gcnew ArgumentNullException("native");

			_native = native;
			_equalityHandle = IntPtr(native);

			if (Log->IsDebugEnabled)
			{
				Log->DebugFormat("Wrapping device '{0:x}', Type: {1}, RefCount: {2}",
					reinterpret_cast<std::size_t>(native),
					Type,
					RefCount);
			}
		}

		DeviceBase::!DeviceBase()
		{
			if (_native != nullptr)
			{
				Log->DebugFormat("Disposing device '{0:x}', Type: {1}, RefCount (before): {2}",
					reinterpret_cast<std::size_t>(_native),
					Type,
					RefCount);

				// So that we can actually delete our handler-wrapper
				_native->SetMessageHandler(nullptr);

				// And finally release the device
				_native->Release();
			}

			if (_handler != nullptr)
			{
				delete _handler;
				_handler = nullptr;
			}

			_native = nullptr;
			_handler = nullptr;
		}

		DeviceBase::~DeviceBase()
		{
			this->!DeviceBase();
		}

		RiftDotNet::MessageHandler^ DeviceBase::MessageHandler::get()
		{
			if (IsDisposed)
				throw gcnew ObjectDisposedException("IDevice");

			auto native = dynamic_cast<Platform::MessageHandler*>(_native->GetMessageHandler());
			if (native == nullptr)
				return nullptr;

			return native->ManagedHandler()->Managed;
		}

		void DeviceBase::MessageHandler::set(RiftDotNet::MessageHandler^ handler)
		{
			if (IsDisposed)
				throw gcnew ObjectDisposedException("IDevice");

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

			if (Log->IsInfoEnabled)
			{
				if (handler != nullptr)
					Log->InfoFormat("Attaching message handler '{1:x}' to device '{0:x}'",
					reinterpret_cast<std::size_t>(_native),
					reinterpret_cast<std::size_t>(native));
				else
					Log->InfoFormat("Detaching message handler from device '{0:x}'", reinterpret_cast<std::size_t>(_native));
			}

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