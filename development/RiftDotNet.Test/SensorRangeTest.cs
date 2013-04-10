using FluentAssertions;
using NUnit.Framework;

namespace RiftDotNet.Test
{
	[TestFixture]
	public sealed class SensorRangeTest
	{
		[Test]
		public void TestCtor()
		{
			var info = new SensorRange(1, 2, 3);
			info.MaxAcceleration.Should().BeApproximately(1, 0);
			info.MaxRotationRate.Should().BeApproximately(2, 0);
			info.MaxMagneticField.Should().BeApproximately(3, 0);
		}
	}
}