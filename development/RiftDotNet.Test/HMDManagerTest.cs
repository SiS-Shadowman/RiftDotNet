using System;
using System.Diagnostics;
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
					return;
					//throw new Exception("Please attach your oculus rift to the computer");

				mgr.AttachedDevice.Should().NotBeNull();
				mgr.AttachedDevice.Should().BeSameAs(mgr.Devices[0]);
				var hmd = mgr.WaitForAttachedDevice(null);
				hmd.Should().BeSameAs(mgr.AttachedDevice);
			}
		}

		[Test]
		public void TestDevicesPresent()
		{
			//
			// This test assumes that the device has already been attached when
			// the HMDManager is created and verifies that it correctly queries
			// them.
			//

			var debug = new Dummy.DeviceManager();
			var hmds = new[] {debug.CreateHMD(), debug.CreateHMD()};
			debug.SetHmds(hmds);
			using (var mgr = new HMDManager(new Dummy.Factory(), debug))
			{
				mgr.Devices.Count.Should().Be(2);
				mgr.Devices[0].Info.Should().BeSameAs(hmds[0].Info);
				mgr.Devices[1].Info.Should().BeSameAs(hmds[1].Info);

				var attached1 = mgr.AttachedDevice;
				var attached2 = mgr.WaitForAttachedDevice(null);
				attached1.Should().BeSameAs(attached2);
			}
		}

		[Test]
		public void TestDeviceAdded()
		{
			//
			// This test uses the dummy implementation and sends a message from another
			// thread that a device has been added.
			//
			using (var mgr = new HMDManager(new Dummy.Factory()))
			{
				var debug = (Dummy.DeviceManager)mgr.Manager;
				mgr.Devices.Count.Should().Be(0);

				IHMD hmdAdded = null;
				Action<IHMD> fn = tmp =>
					{
						hmdAdded = tmp;
					};
				mgr.DeviceAttached += fn;

				var sw = new Stopwatch();
				sw.Start();
				debug.SimulateAddOneDevice(TimeSpan.FromMilliseconds(100), false);
				var hmd = mgr.WaitForAttachedDevice(null);
				sw.Elapsed.Should().BeGreaterOrEqualTo(TimeSpan.FromMilliseconds(100)); //< Because of the way, we can be certain

				// The manager must have waited for the device to be attached.
				hmd.Should().NotBeNull();
				mgr.Devices.Count.Should().Be(1);
				mgr.Devices[0].Should().BeSameAs(hmd);

				// The manager must have emitted the signal *and* passed the correct
				// hmd
				hmdAdded.Should().NotBeNull();
				hmdAdded.Should().BeSameAs(hmd);

				// The HMD must recognize that it is currently attached
				hmd.IsAttached.Should().BeTrue();

				// The manager must return this device, as it is currently attached
				mgr.AttachedDevice.Should().BeSameAs(hmd);
			}
		}
	}
}