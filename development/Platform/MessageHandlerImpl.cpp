#include "stdafx.h"
#include "MessageHandlerImpl.h"
#include "MessageHandler.h"




namespace RiftDotNet
{
	namespace Platform
	{
		MessageHandlerImpl::MessageHandlerImpl(RiftDotNet::MessageHandler^ managed)
		{
			if (managed == nullptr)
				throw gcnew ArgumentNullException("managed");

			if (managed->Impl != nullptr)
				throw gcnew ArgumentException("Expected a MessageHandler which has not yet been given an MessageHandlerImpl instance", "managed");

			_native = new MessageHandler(this);
			_managed = managed;
			_managed->Impl = this;
		}

		MessageHandlerImpl::~MessageHandlerImpl()
		{
			if (_native != nullptr)
			{
				RemoveHandlerFromDevices();
			}

			if (_managed != nullptr)
			{
				_managed->Impl = nullptr;
			}

			delete _native;
			_native = nullptr;
			_managed = nullptr;
		}

		bool MessageHandlerImpl::IsInstalled::get()
		{
			if (IsDisposed)
				throw gcnew ObjectDisposedException("IMessageHandler");

			return _native->IsHandlerInstalled();
		}

		void MessageHandlerImpl::RemoveHandlerFromDevices()
		{
			_native->RemoveHandlerFromDevices();
		}
	}
}