using System;

namespace RiftDotNet
{
	/// <summary>
	///     This class must be inherited from in order to install custom
	///     message handlers on IDevice objects.
	/// </summary>
	public abstract class MessageHandler
		: IMessageHandler
	{
		#region Internal
		/// <summary>
		///     This would be internal, but adding a friend-relationship
		///     to a c++/cli assembly is a nightmare...
		/// </summary>
		/// <remarks>
		///     Please don't fuck with this.
		/// </remarks>
		public IMessageHandler Impl;

		private bool _isDisposed;

		#endregion

		#region IMessageHandler Members

		public virtual void Dispose()
		{
			if (_isDisposed)
				return;

			RemoveHandlerFromDevices();
			Impl = null;
			_isDisposed = true;
		}

		public bool IsInstalled
		{
			get
			{
				if (IsDisposed)
					throw new ObjectDisposedException("IMessageHandler");

				if (Impl != null)
					return Impl.IsInstalled;

				return false;
			}
		}

		public bool IsDisposed
		{
			get { return _isDisposed; }
		}

		public void RemoveHandlerFromDevices()
		{
			if (IsDisposed)
				throw new ObjectDisposedException("IMessageHandler");

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