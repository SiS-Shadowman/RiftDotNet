#pragma once

#include <vcclr.h>
#include <OVR_Device.h>

#include "RiftDotNet.h"
#include "Log4Net.h"
#include "Helper.h"

using namespace System;




namespace RiftDotNet
{
	namespace Platform
	{
		/// <summary>
		/// This class wraps a managed IMessageHandler implementation,
		/// allowing callbacks from native code to be handled in managed one.
		/// </summary>
		public class MessageHandler
			: public OVR::MessageHandler
		{
		public:

			MessageHandler(RiftDotNet::MessageHandler^ impl)
			{
				if (impl == nullptr)
					throw gcnew ArgumentNullException("impl");

				_impl = impl;
				_callback = gcnew MessageHandlerCallback(this, impl);
			}

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
					_impl->SupportsMessageType(Helper::FromNative(type));
				}
				catch(Exception^ e)
				{
					Rift::Log->ErrorFormat("Caught exception in IMessageHandler::SupportsMessageType, ignoring it: {0}", e);
				}
			}

		private:

			IMessage^ CreateMessage(const OVR::Message& message);

			ref class MessageHandlerCallback sealed
			{
			public:

				MessageHandlerCallback(MessageHandler* native, RiftDotNet::MessageHandler^ managed)
				{
					_native = native;
					_managed = managed;

					_managed->IsInstalledCallback = gcnew Func<bool>(this, &MessageHandlerCallback::IsHandlerInstalled);
					_managed->RemoveHandlerFromDevicesCallback = gcnew Action(this, &MessageHandlerCallback::RemoveHandlerFromDevices);
				}

				~MessageHandlerCallback()
				{
				
				}

			private:

				bool IsHandlerInstalled()
				{
					return _native->IsHandlerInstalled();
				}

				void RemoveHandlerFromDevices()
				{
					_native->RemoveHandlerFromDevices();
				}

			private:

				MessageHandler* _native ;
				RiftDotNet::MessageHandler^ _managed;
			};

		private:

			gcroot<RiftDotNet::MessageHandler^> _impl;
			gcroot<MessageHandlerCallback^> _callback;
		};
	}
}