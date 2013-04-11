#pragma once

#include <OVR_Device.h>

#include "RiftDotNet.h"
#include "DeviceInfo.h"


namespace RiftDotNet
{
	namespace Platform
	{
		public ref class HMDInfo sealed
			: public DeviceInfo
			, public IHMDInfo
		{
		public:

			property unsigned int HResolution { virtual unsigned int get() { return _hResolution; } }
			property unsigned int VResolution  { virtual unsigned int get() { return _vResolution; } }
			property float     HScreenSize { virtual float get() { return _hScreenSize; } }
			property float VScreenSize { virtual float get() { return _vScreenSize; } }
			property float     VScreenCenter { virtual float get() { return _vScreenCenter; } }
			property float     EyeToScreenDistance { virtual float get() { return _vScreenCenter; } }
			property float     LensSeparationDistance { virtual float get() { return _vScreenCenter; } }
			property float     InterpupillaryDistance { virtual float get() { return _interpupillaryDistance; } }
			property array<float>^ DistortionK { virtual  array<float>^ get() { return _distortionK; } }
			property int       DesktopX { virtual int get() { return _desktopX; } }
			property int DesktopY { virtual int get() { return _desktopY; } }
			property Object^ DisplayDevice { virtual Object^ get() { return _displayDevice; } }

			HMDInfo()
				: DeviceInfo(DeviceType::HMD)
				, _hResolution(0)
				, _vResolution(0)
				, _hScreenSize(0)
				, _vScreenSize(0)
				, _vScreenCenter(0)
				, _eyeToScreenDistance(0)
				, _lensSeparationDistance(0)
				, _interpupillaryDistance(0)
				, _desktopX(0)
				, _desktopY(0)
				, _displayDevice(nullptr)
			{
				_distortionK = gcnew array<float>(4);
			}

			HMDInfo(const OVR::HMDInfo& native)
				: DeviceInfo(native)
				, _hResolution(native.HResolution)
				, _vResolution(native.VResolution)
				, _hScreenSize(native.HScreenSize)
				, _vScreenSize(native.VScreenSize)
				, _vScreenCenter(native.VScreenCenter)
				, _eyeToScreenDistance(native.EyeToScreenDistance)
				, _lensSeparationDistance(native.LensSeparationDistance)
				,_interpupillaryDistance(native.InterpupillaryDistance)
				, _distortionK(gcnew array<float>(4))
				, _desktopX(native.DesktopX)
				, _desktopY(native.DesktopY)
				, _displayDevice(gcnew String(native.DisplayDeviceName)) //< TODO: MacOS...
			{
				DistortionK[0] = native.DistortionK[0];
				DistortionK[1] = native.DistortionK[1];
				DistortionK[2] = native.DistortionK[2];
				DistortionK[3] = native.DistortionK[3];
			}

		private:

			const unsigned int _hResolution, _vResolution;
			const float     _hScreenSize, _vScreenSize;
			const float     _vScreenCenter;
			const float     _eyeToScreenDistance;
			const float     _lensSeparationDistance;
			const float     _interpupillaryDistance;
			array<float>^ _distortionK;
			const int       _desktopX, _desktopY;
			Object^ _displayDevice;
		};
	}
}
