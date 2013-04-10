using FluentAssertions;
using NUnit.Framework;

namespace RiftDotNet.Test
{
	[TestFixture]
	public sealed class SensorInfoTest
	{
		[Test]
		public void TestCreate()
		{
			var factory = ((ITestingFactory)Factory.PlatformFactory);
			var info = factory.CreateSensorInfo();
			info.Should().NotBeNull();

			info.InfoClassType.Should().Be(DeviceType.None);
			info.Manufacturer.Should().BeNull();
			info.ProductId.Should().Be(0);
			info.ProductName.Should().BeNull();
			info.SerialNumber.Should().BeNull();
			info.Type.Should().Be(DeviceType.Sensor);
			info.VendorId.Should().Be(0);
			info.Version.Should().Be(0);
		}
	}
}