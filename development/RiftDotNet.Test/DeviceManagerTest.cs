using FluentAssertions;
using NUnit.Framework;

namespace RiftDotNet.Test
{
	[TestFixture]
	public sealed class DeviceManagerTest
	{
		[Test]
		public void TestCtor()
		{
			using (var mgr = Factory.CreateDeviceManager())
			{
				mgr.Should().NotBeNull();
				mgr.Type.Should().Be(DeviceType.Manager);
				mgr.Parent.Should().BeNull();
			}
		}

		[Test]
		public void TestHMDDevices()
		{
			// ReSharper disable PossibleMultipleEnumeration

			using (var mgr = Factory.CreateDeviceManager())
			{
				var devices = mgr.HMDDevices;
				devices.Should().NotBeNull();
				foreach (var desc in devices)
				{
					desc.Should().NotBeNull();
					desc.IsAvailable.Should().BeTrue();
					desc.IsCreated.Should().BeFalse();

					var hmd = desc.CreateDevice();
					hmd.Should().NotBeNull();
				}
			}

			// ReSharper restore PossibleMultipleEnumeration
		}

		[Test]
		public void TestSensorDevices()
		{
// ReSharper disable PossibleMultipleEnumeration

			using (var mgr = Factory.CreateDeviceManager())
			{
				var devices = mgr.SensorDevices;
				devices.Should().NotBeNull();
				foreach (var desc in devices)
				{
					desc.Should().NotBeNull();
					desc.IsAvailable.Should().BeTrue();
					desc.IsCreated.Should().BeFalse();

					var sensor = desc.CreateDevice();
					sensor.Should().NotBeNull();
				}
			}

// ReSharper restore PossibleMultipleEnumeration
		}
	}
}