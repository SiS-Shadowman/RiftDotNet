#include "stdafx.h"
#include "TypedDeviceHandle.h"

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
				return IDeviceManager::typeid;

			case RiftDotNet::DeviceType::HMD:
				return IHMDDevice::typeid;

			case RiftDotNet::DeviceType::Sensor:
				return ISensorDevice::typeid;

			default:
				throw gcnew ArgumentException();
			}
		}
	}
}