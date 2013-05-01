#pragma once

#include <OVR_DeviceHandle.h>
#include <OVR_Win32_Sensor.h>
#include "../Platform/ValueWrapper.h"
#include "DestructorTest.h"

using namespace RiftDotNet::Win32;
using namespace NUnit::Framework;

ref class DeviceHandleTest
{
public:

	void Test()
	{
		OVR::System::Init();
		auto factory = &OVR::Win32::SensorDeviceFactory::Instance;
		auto hidDesc = OVR::HIDDeviceDesc();
		auto sensorDesc = new OVR::Win32::SensorDeviceCreateDesc(factory, hidDesc);
		auto nativeHandle = OVR::DeviceHandle(sensorDesc);
		auto wrapper = new DeviceHandleWrapper(nativeHandle);
		//auto handle = gcnew DeviceHandle(wrapper);
	}
};

int Main(void)
{
	Link l;

	/*auto test = gcnew DeviceHandleTest();
	test->Test();*/
	return 0;
}