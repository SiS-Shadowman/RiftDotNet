#include "stdafx.h"
#include "TypedDeviceHandle.h"

#include "DeviceManager.h"
#include "HMDDevice.h"
#include "SensorDevice.h"

namespace RiftDotNet
{
	namespace Platform
	{
		generic <typename TDevice, typename TInfo>
		where TDevice : IDevice
		where TInfo : IDeviceInfo
		Type^ TypedDeviceHandle<TDevice,TInfo>::GetType(RiftDotNet::DeviceType type)
		{
			switch(type)
			{
			case RiftDotNet::DeviceType::None:
				return void::typeid;

			case RiftDotNet::DeviceType::Manager:
				return DeviceManager::typeid;

			case RiftDotNet::DeviceType::HMD:
				return HMDDevice::typeid;

			case RiftDotNet::DeviceType::Sensor:
				return SensorDevice::typeid;

			default:
				throw gcnew ArgumentException();
			}
		}
	}
}