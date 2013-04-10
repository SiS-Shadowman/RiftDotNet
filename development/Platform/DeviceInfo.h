#pragma once

using namespace System;

#include "RiftDotNet.h"




namespace RiftDotNet
{
	namespace Platform
	{
		public ref class DeviceInfo abstract
			: public IDeviceInfo
		{
		public:

			property DeviceType InfoClassType { virtual DeviceType get() { return _infoClassType; } }
			property DeviceType Type { virtual DeviceType get() { return _type; } }
			property String^ ProductName { virtual String^ get() { return _productName; } }
			property String^ Manufacturer { virtual String^ get() { return _manufacturer; } }
			property UINT Version { virtual UINT get() { return _version; } }

			virtual ~DeviceInfo()
			{}

		private:

			const DeviceType _infoClassType;
			const DeviceType _type;
			String^ _productName;
			String^ _manufacturer;
			const UINT _version;

		protected:

			DeviceInfo(const OVR::DeviceInfo& native)
				: _infoClassType((DeviceType)native.InfoClassType)
				, _type((DeviceType)native.Type)
				, _productName(gcnew String(native.ProductName))
				, _manufacturer(gcnew String(native.Manufacturer))
				, _version(native.Version)
			{}
		};
	}
}