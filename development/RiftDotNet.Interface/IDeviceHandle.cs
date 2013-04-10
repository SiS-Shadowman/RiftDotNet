using System;

namespace RiftDotNet
{
	public interface IDeviceHandle
		: IDisposable
	{
		bool IsCreated { get; }
		bool IsAvailable { get; }
		DeviceType DeviceType { get; }

		IDevice CreateDevice();
	}

	public interface IDeviceHandle<out T>
		: IDeviceHandle
		where T : IDevice
	{
		new T CreateDevice();
	}
}