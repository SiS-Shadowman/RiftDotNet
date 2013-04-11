namespace RiftDotNet
{
	/// <summary>
	/// Interface for all messages.
	/// </summary>
	public interface IMessage
	{
		/// <summary>
		/// What kind of message this is.
		/// </summary>
		MessageType Type { get; }

		/// <summary>
		/// Device that emitted the message.
		/// </summary>
		/// <remarks>
		/// Upon retrieving this property, the caller becomes responsible
		/// for disposing of the object.
		/// </remarks>
		IDevice Device { get; }
	}
}