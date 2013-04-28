#include "stdafx.h"
#include "DeviceHandle.h"
#include "SensorInfo.h"
#include "HMDInfo.h"




namespace RiftDotNet
{
	namespace Platform
	{
		IDeviceInfo^ DeviceHandle::DeviceInfo::get()
		{
			if (IsDisposed)
				throw gcnew ObjectDisposedException("IDeviceHandle");

			switch(_native->Handle().GetType())
			{
			case OVR::Device_HMD:
				{
					OVR::HMDInfo info;
					GetDeviceInfo(info);
					return gcnew HMDInfo(info);
				}
				break;

			case OVR::Device_Sensor:
				{
					OVR::SensorInfo info;
					GetDeviceInfo(info);
					return gcnew SensorInfo(info);
				}
				break;

			default:
				throw gcnew Exception("Unknown device type");
			}
		}

		void DeviceHandle::GetDeviceInfo(OVR::DeviceInfo& info)
		{
			if (IsDisposed)
				throw gcnew ObjectDisposedException("IDeviceHandle");

			if (!_native->Handle().GetDeviceInfo(&info))
			{
				throw gcnew Exception("Unable to retrieve information about this device - Check the log for further information");
			}
		}
	}
}