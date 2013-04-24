using System;
using System.Diagnostics.Contracts;

namespace RiftDotNet
{
	public interface IMessageHandler
		: IDisposable
	{
		/// <summary>
		/// Returns 'true' if handler is currently installed on any devices.
		/// </summary>
		bool IsInstalled { get; }

		bool IsDisposed { get; }

		void RemoveHandlerFromDevices();

		void OnMessage(IMessage message);

		/// <summary>
		/// Determines if handler supports a specific message type. Can
		/// be used to filter out entire message groups. The result
		/// returned by this function shouldn't change after handler creation.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		[Pure]
		bool SupportsMessageType(MessageType type);
	}
}