using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RiftDotNet.Test.Dummy
{
	public sealed class Factory
		: IFactory
	{
		public IDeviceManager CreateDeviceManager()
		{
			return new DeviceManager();
		}

		public ISensorFusion CreateSensorFusion(ISensorDevice sensorDevice)
		{
			return new SensorFusion(sensorDevice);
		}
	}
}
