#pragma once

#include <vcclr.h>
#include <OVR_Device.h>

#include "RiftDotNet.h"
#include "Log4Net.h"
#include "Helper.h"
#include "MessageHandlerImpl.h"

using namespace System;




namespace RiftDotNet
{
	namespace Platform
	{
		ref class MessageHandlerImpl;

		/// <summary>
		/// Implementation of the native OVR::MessageHandler interface.
		/// Provides the callbacks to a managed IMessageHandler implementation object.
		/// </summary>
		public class MessageHandler
			: public OVR::MessageHandler
		{
		public:

			MessageHandler(MessageHandlerImpl^ impl);

			~MessageHandler()
			{
				RemoveHandlerFromDevices();
			}

			virtual void OnMessage(const OVR::Message& nativeMessage)
			{
				try
				{
					auto message = CreateMessage(nativeMessage);
					_impl->OnMessage(message);
				}
				catch(Exception^ e)
				{
					// We shouldn't really let exceptions propagate to
					// the native code: We'll just catch everything and log it, for now...
					Rift::Log->ErrorFormat("Caught exception in IMessageHandler::OnMessage, ignoring it: {0}", e);
				}
			}

			virtual bool SupportsMessageType(OVR::MessageType type) const
			{
				try
				{
					return _impl->SupportsMessageType(Helper::FromNative(type));
				}
				catch(Exception^ e)
				{
					Rift::Log->ErrorFormat("Caught exception in IMessageHandler::SupportsMessageType, ignoring it: {0}", e);
				}

				return false;
			}

			/// Obtains a reference to the actual .NET IMessageHandler implementation
			MessageHandlerImpl^ ManagedHandler()
			{
				return _impl;
			}

		private:

			IMessage^ CreateMessage(const OVR::Message& message);

		private:

			gcroot<MessageHandlerImpl^> _impl;
		};
	}
}