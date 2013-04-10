#include "stdafx.h"
#include "DeviceBase.h"
#include "DeviceManager.h"
#include "HMDDevice.h"
#include "SensorDevice.h"

using namespace System;




namespace RiftDotNet
{
	namespace Platform
	{
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