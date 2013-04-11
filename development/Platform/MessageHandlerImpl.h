#pragma once

#include "RiftDotNet.h"

using namespace System;




namespace RiftDotNet
{
	namespace Platform
	{
		class MessageHandler;

		/// <summary>
		/// Intermediary IMessageHandler implementation.
		/// Provides method calls **INTO** the native OVR::MessageHandler base class.
		/// These methods are called from the managed MessageHandler base class, which is
		/// what a user inherits in order to install a message handler.
		/// </summary>
		public ref class MessageHandlerImpl sealed
			: public IMessageHandler
			, IDisposable
		{
		public:

			MessageHandlerImpl(RiftDotNet::MessageHandler^ managed);
			~MessageHandlerImpl();

			property bool IsInstalled { virtual bool get(); }

			virtual void RemoveHandlerFromDevices();

			virtual void OnMessage(RiftDotNet::IMessage^ message)
			{
				_managed->OnMessage(message);
			}

			virtual bool SupportsMessageType(RiftDotNet::MessageType type)
			{
				return _managed->SupportsMessageType(type);
			}

			property RiftDotNet::MessageHandler^ Managed
			{
				RiftDotNet::MessageHandler^ get() { return _managed; }
			}

			Platform::MessageHandler* GetNative()
			{
				return _native;
			}

		private:

			Platform::MessageHandler* _native ;
			RiftDotNet::MessageHandler^ _managed;
		};
	}
}