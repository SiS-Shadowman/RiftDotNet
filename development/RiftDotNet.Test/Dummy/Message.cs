namespace RiftDotNet.Test.Dummy
{
	public abstract class Message
		: IMessage
	{
		#region IMessage Members

		public MessageType Type { get; set; }
		public IDevice Device { get; set; }

		#endregion
	}
}