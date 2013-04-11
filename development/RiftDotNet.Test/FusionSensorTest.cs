using FluentAssertions;
using NUnit.Framework;
using SharpDX;

namespace RiftDotNet.Test
{
	[TestFixture]
	public sealed class FusionSensorTest
	{
		[Test]
		public void TestCtor()
		{
			ISensorFusion fusion = Factory.CreateSensorFusion();
			fusion.Should().NotBeNull();

			fusion.AttachedDevice.Should().BeNull();
			fusion.IsAttachedToSensor.Should().BeFalse();
			fusion.AccelGain.Should().Be(0.05f);
			fusion.Acceleration.Should().Be(Vector3.Zero);
			fusion.AngularVelocity.Should().Be(Vector3.Zero);
			fusion.IsGravityEnabled.Should().BeTrue();
			fusion.Orientation.Should().Be(Quaternion.Identity);
			fusion.PredictedOrientation.Should().Be(Quaternion.Identity);
			fusion.YawMultiplier.Should().Be(1);
		}
	}
}