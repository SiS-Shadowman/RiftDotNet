using FluentAssertions;
using NUnit.Framework;

namespace RiftDotNet.Test
{
	[TestFixture]
	public sealed class HMDInfoTest
	{
		[Test]
		public void TestCreate()
		{
			var factory = ((ITestingFactory) Factory.PlatformFactory);
			var info = factory.CreateHMDInfo();
			info.Should().NotBeNull();

			info.InfoClassType.Should().Be(DeviceType.None);
			info.Type.Should().Be(DeviceType.HMD);
			info.DisplayDevice.Should().BeNull();
			info.EyeToScreenDistance.Should().Be(0);
			info.HResolution.Should().Be(0);
			info.HScreenSize.Should().Be(0);
			info.InterpupillaryDistance.Should().Be(0);
			info.LensSeparationDistance.Should().Be(0);
			info.Manufacturer.Should().BeNull();
			info.ProductName.Should().BeNull();
			info.VResolution.Should().Be(0);
			info.VScreenCenter.Should().Be(0);
			info.VScreenSize.Should().Be(0);
			info.Version.Should().Be(0);
		}
	}
}