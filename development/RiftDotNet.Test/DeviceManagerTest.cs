using System;
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
			IDeviceManager mgr;
			using (mgr = Factory.CreateDeviceManager())
			{
				mgr.Should().NotBeNull();
				mgr.RefCount.Should().Be(1);
				mgr.IsDisposed.Should().BeFalse();
				mgr.Type.Should().Be(DeviceType.Manager);
				mgr.Parent.Should().BeNull();
				mgr.MessageHandler.Should().BeNull();
			}
			mgr.RefCount.Should().Be(0);
			mgr.IsDisposed.Should().BeTrue();
		}

		[Test]
		public void TestHMDDevices()
		{
			// ReSharper disable PossibleMultipleEnumeration

			IDeviceManager mgr;
			using (mgr = Factory.CreateDeviceManager())
			{
				mgr.RefCount.Should().Be(1);
				mgr.IsDisposed.Should().BeFalse();
				var devices = mgr.HMDDevices;
				devices.Should().NotBeNull();
				foreach (var desc in devices)
				{
					desc.Should().NotBeNull();
					desc.IsAvailable.Should().BeTrue();
					desc.IsCreated.Should().BeFalse();

					IHMDDevice hmd = desc.CreateDevice();
					hmd.Should().NotBeNull();
				}
			}
			mgr.RefCount.Should().Be(0);
			mgr.IsDisposed.Should().BeTrue();
			// ReSharper restore PossibleMultipleEnumeration
		}

		[Test]
		public void TestMessageHandler()
		{
			IDeviceManager mgr;
			using (mgr = Factory.CreateDeviceManager())
			{
				mgr.RefCount.Should().Be(1);
				mgr.IsDisposed.Should().BeFalse();
				mgr.MessageHandler.Should().BeNull();
				using (var handler = new DummyHandler())
				{
					//
					// Attaching...
					//

					mgr.MessageHandler = handler;
					mgr.MessageHandler.Should().BeSameAs(handler);

					handler.IsInstalled.Should().BeTrue();
					handler.Impl.Should().NotBeNull();


					//
					// Attaching again
					//

					var impl = handler.Impl;
					mgr.MessageHandler = handler;
					mgr.MessageHandler.Should().BeSameAs(handler);
					handler.Impl.Should().BeSameAs(impl); //< one to one relationship must be preserved

					//
					// Removing via method
					//
					handler.RemoveHandlerFromDevices();
					handler.IsInstalled.Should().BeFalse();
					handler.Impl.Should().BeNull();
					mgr.MessageHandler.Should().BeNull();


					//
					// Removing via property
					//

					mgr.MessageHandler = handler;
					handler.IsInstalled.Should().BeTrue();
					handler.Impl.Should().NotBeNull();
					impl = handler.Impl;

					mgr.MessageHandler = null;
					mgr.MessageHandler.Should().BeNull();
					handler.IsInstalled.Should().BeFalse();
					handler.Impl.Should().BeSameAs(impl); //< The thing is that the filter might be installed elsewhere: the impl can only be disposed of when the handler is removed from *all* devices or disposed of itself
				}

				// TOOD: Test throwing exceptions, filtering
			}
			mgr.RefCount.Should().Be(0);
			mgr.IsDisposed.Should().BeTrue();
		}

		[Test]
		public void TestSensorDevices()
		{
// ReSharper disable PossibleMultipleEnumeration

			IDeviceManager mgr;
			using (mgr = Factory.CreateDeviceManager())
			{
				mgr.RefCount.Should().Be(1);
				mgr.IsDisposed.Should().BeFalse();
				var devices = mgr.SensorDevices;
				devices.Should().NotBeNull();
				foreach (var desc in devices)
				{
					desc.Should().NotBeNull();
					desc.IsAvailable.Should().BeTrue();
					desc.IsCreated.Should().BeFalse();

					ISensorDevice sensor = desc.CreateDevice();
					sensor.Should().NotBeNull();
				}
			}
			mgr.RefCount.Should().Be(0);
			mgr.IsDisposed.Should().BeTrue();

// ReSharper restore PossibleMultipleEnumeration
		}

		[Test]
		public void TestEquals()
		{
			IDeviceManager mgr;
			int hashCode;
			using (mgr = Factory.CreateDeviceManager())
			{
				mgr.Equals(mgr).Should().BeTrue();
				mgr.GetHashCode().Should().Be(mgr.GetHashCode());
				mgr.Equals(null).Should().BeFalse();
				mgr.Equals(1).Should().BeFalse();
				hashCode = mgr.GetHashCode();
			}

			mgr.GetHashCode().Should().Be(hashCode); //< The hashcode may not have changed after disposal
			// TODO: Is it okay to base Equality on the internal pointer?
		}

		[Test]
		public void TestContracts()
		{
			var mgr = Factory.CreateDeviceManager();
			mgr.Dispose();
			mgr.IsDisposed.Should().BeTrue();
			mgr.Dispose(); //< Successive dispose is okay
			new Action(() => { var unused = mgr.HMDDevices; }).ShouldThrow<ObjectDisposedException>();
			new Action(() => { var unused = mgr.MessageHandler; }).ShouldThrow<ObjectDisposedException>();
			new Action(() => { var unused = mgr.Parent; }).ShouldThrow<ObjectDisposedException>();
			new Action(() => { var unused = mgr.SensorDevices; }).ShouldThrow<ObjectDisposedException>();
			new Action(() => { var unused = mgr.Type; }).ShouldThrow<ObjectDisposedException>();
		}
	}

	internal class DummyHandler
		: MessageHandler
	{
		public override void OnMessage(IMessage message)
		{
			
		}

		public override bool SupportsMessageType(MessageType type)
		{
			return true;
		}
	}
}