using System;
using FluentAssertions;
using NUnit.Framework;

namespace RiftDotNet.Test
{
	[TestFixture]
	public sealed class HMDManagerTest
	{
		[Test]
		public void TestCtor()
		{
			using (var mgr = new HMDManager())
			{
				mgr.AttachedDevice.Should().BeNull();
				var hmd = mgr.WaitForAttachedDevice(TimeSpan.FromMilliseconds(100));
				hmd.Should().BeNull();

				hmd = mgr.WaitForAttachedDevice(TimeSpan.FromMilliseconds(10));
				hmd.Should().BeNull();

				mgr.Devices.Should().BeEmpty();
			}
		}

		[Test]
		public void TestWithOneDeviceAttached()
		{
			using (var mgr = new HMDManager())
			{
				if (mgr.Devices.Count == 0)
					throw new Exception("Please attach your oculus rift to the computer");

				mgr.AttachedDevice.Should().NotBeNull();
				mgr.AttachedDevice.Should().BeSameAs(mgr.Devices[0]);
				var hmd = mgr.WaitForAttachedDevice(null);
				hmd.Should().BeSameAs(mgr.AttachedDevice);
			}
		}
	}
}