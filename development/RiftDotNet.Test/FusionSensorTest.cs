using System;
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
			ISensorFusion fusion;
			using (fusion = Factory.CreateSensorFusion())
			{
				fusion.Should().NotBeNull();
				fusion.IsDisposed.Should().BeFalse();

				fusion.IsAttachedToSensor.Should().BeFalse();
				fusion.AccelGain.Should().Be(0.05f);
				fusion.Acceleration.Should().Be(Vector3.Zero);
				fusion.AngularVelocity.Should().Be(Vector3.Zero);
				fusion.IsGravityEnabled.Should().BeTrue();
				fusion.Orientation.Should().Be(Quaternion.Identity);
				fusion.PredictedOrientation.Should().Be(Quaternion.Identity);
				fusion.YawMultiplier.Should().Be(1);
			}

			fusion.IsDisposed.Should().BeTrue();
		}

		[Test]
		public void TestAttach()
		{
			using (var fusion = Factory.CreateSensorFusion())
			{
				fusion.AttachedDevice = null;
				fusion.IsAttachedToSensor.Should().BeFalse();
			}
		}

		[Test]
		public void TestContracts()
		{
			ISensorFusion fusion = Factory.CreateSensorFusion();
			fusion.Dispose();
			fusion.IsDisposed.Should().BeTrue();
			fusion.Dispose(); //< Must be allowed

			//
			// Get
			//
			new Action(() => { var unused = fusion.IsAttachedToSensor; }).ShouldThrow<ObjectDisposedException>();
			new Action(() => { var unused = fusion.IsGravityEnabled; }).ShouldThrow<ObjectDisposedException>();
			new Action(() => { var unused = fusion.IsPredictionEnabled; }).ShouldThrow<ObjectDisposedException>();
			new Action(() => { var unused = fusion.Orientation; }).ShouldThrow<ObjectDisposedException>();
			new Action(() => { var unused = fusion.PredictedOrientation; }).ShouldThrow<ObjectDisposedException>();
			new Action(() => { var unused = fusion.PredictionTime; }).ShouldThrow<ObjectDisposedException>();
			new Action(() => { var unused = fusion.YawMultiplier; }).ShouldThrow<ObjectDisposedException>();
			new Action(() => { var unused = fusion.AccelGain; }).ShouldThrow<ObjectDisposedException>();
			new Action(() => { var unused = fusion.Acceleration; }).ShouldThrow<ObjectDisposedException>();
			new Action(() => { var unused = fusion.AngularVelocity; }).ShouldThrow<ObjectDisposedException>();

			//
			// Set
			//

			new Action(() => fusion.AccelGain = 1.5f).ShouldThrow<ObjectDisposedException>();
			new Action(() => fusion.YawMultiplier = 1.5f).ShouldThrow<ObjectDisposedException>();
			new Action(() => fusion.IsPredictionEnabled = true).ShouldThrow<ObjectDisposedException>();
			new Action(() => fusion.PredictionTime = TimeSpan.FromDays(1)).ShouldThrow<ObjectDisposedException>();

			new Action(fusion.Reset).ShouldThrow<ObjectDisposedException>();
		}
	}
}