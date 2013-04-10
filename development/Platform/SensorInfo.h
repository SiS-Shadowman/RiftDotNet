#pragma once

#include <OVR_Device.h>

#include "RiftDotNet.h"
#include "DeviceInfo.h"


using namespace System;


namespace RiftDotNet
{
	namespace Platform
	{
		public ref class SensorInfo sealed
			: public DeviceInfo
			, public ISensorInfo
		{
		public:

			property UINT16 VendorId { virtual UINT16 get() { return _vendorId; } }
			property UINT16 ProductId { virtual UINT16 get() { return _productId; } }
			property String^ SerialNumber { virtual String^ get() { return _serialNumber; } }

		private:

			const UINT16 _vendorId;
			const UINT16 _productId;
			String^ _serialNumber;

		public:

			SensorInfo(const OVR::SensorInfo& native)
				: DeviceInfo(native)
				, _vendorId(native.VendorId)
				, _productId(native.ProductId)
				, _serialNumber(gcnew String(native.SerialNumber))
			{}
		};
	}
}