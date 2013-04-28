namespace RiftDotNet
{
	internal sealed class InternalMessageHandler
		: MessageHandler
	{
		private readonly HMDManager _manager;

		public InternalMessageHandler(HMDManager manager)
		{
			_manager = manager;
		}

		public override void OnMessage(IMessage message)
		{
			switch (message.Type)
			{
				case MessageType.DeviceAdded:
				case MessageType.DeviceRemoved:
					_manager.DeviceChanged((IMessageDeviceStatus)message);
					break;
			}
		}

		public override bool SupportsMessageType(MessageType type)
		{
			return true;
		}
	}
}