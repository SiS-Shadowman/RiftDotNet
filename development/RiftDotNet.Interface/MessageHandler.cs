namespace RiftDotNet
{
	/// <summary>
	///     This class must be inherited from in order to install custom
	///     message handlers on IDevice objects.
	/// </summary>
	public abstract class MessageHandler
		: IMessageHandler
	{
		/// <summary>
		///     This would be internal, but adding a friend-relationship
		///     to a c++/cli assembly is a nightmare...
		/// </summary>
		/// <remarks>
		///     Please don't fuck with this.
		/// </remarks>
		public IMessageHandler Impl;

		#region IMessageHandler Members

		public virtual void Dispose()
		{
			RemoveHandlerFromDevices();
			Impl = null;
		}

		public bool IsInstalled
		{
			get
			{
				if (Impl != null)
					return Impl.IsInstalled;

				return false;
			}
		}

		public void RemoveHandlerFromDevices()
		{
			if (Impl != null)
			{
				Impl.Dispose();
				Impl = null;
			}
		}

		public abstract void OnMessage(IMessage message);
		public abstract bool SupportsMessageType(MessageType type);

		#endregion
	}
}