using System;

namespace RiftDotNet
{
	public abstract class MessageHandler
		: IMessageHandler
	{
		#region IMessageHandler Members

		public abstract void Dispose();

		public bool IsInstalled
		{
			get
			{
				var tmp = IsInstalledCallback;
				if (tmp == null)
					return false;

				return tmp();
			}
		}

		public void RemoveHandlerFromDevices()
		{
			var tmp = RemoveHandlerFromDevicesCallback;
			if (tmp == null)
				return;

			tmp();
		}

		public abstract void OnMessage(IMessage message);
		public abstract bool SupportsMessageType(MessageType type);

		#endregion

		public Func<bool> IsInstalledCallback;
		public Action RemoveHandlerFromDevicesCallback;
	}
}