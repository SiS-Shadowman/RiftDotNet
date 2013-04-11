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
			using (IDeviceManager mgr = Factory.CreateDeviceManager())
			{
				mgr.Should().NotBeNull();
				mgr.Type.Should().Be(DeviceType.Manager);
				mgr.Parent.Should().BeNull();
				mgr.MessageHandler.Should().BeNull();
			}
		}

		[Test]
		public void TestHMDDevices()
		{
			// ReSharper disable PossibleMultipleEnumeration

			using (IDeviceManager mgr = Factory.CreateDeviceManager())
			{
				IDeviceHandle<IHMDDevice, IHMDInfo>[] devices = mgr.HMDDevices;
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

			// ReSharper restore PossibleMultipleEnumeration
		}

		[Test]
		public void TestMessageHandler()
		{
			using (IDeviceManager mgr = Factory.CreateDeviceManager())
			{
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
		}

		[Test]
		public void TestSensorDevices()
		{
// ReSharper disable PossibleMultipleEnumeration

			using (IDeviceManager mgr = Factory.CreateDeviceManager())
			{
				IDeviceHandle<ISensorDevice, ISensorInfo>[] devices = mgr.SensorDevices;
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

// ReSharper restore PossibleMultipleEnumeration
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