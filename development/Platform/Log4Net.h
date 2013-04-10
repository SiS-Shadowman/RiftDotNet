#pragma once

#include <Kernel/OVR_Log.h>




namespace RiftDotNet
{
	/// <summary>
	/// Helper class to expose the Log4Net system.
	/// </summary>
	private ref class Rift
	{
	public:

		static log4net::ILog^ Log = log4net::LogManager::GetLogger(System::Reflection::MethodBase::GetCurrentMethod()->DeclaringType);
	};



	/// <summary>
	/// OVR::Log implementation which dispatches all logging evets to Log4Net.
	/// </summary>
	public class Log4Net
		: public OVR::Log
	{
	public:

		// This virtual function receives all the messages,
		// developers should override this function in order to do custom logging
		virtual void    LogMessageVarg(OVR::LogMessageType messageType, const char* fmt, va_list argList)
		{
			//
			// Early rejection of message based on type
			// Makes sense because formatting of unlogged messages can incur a noticeable overhead
			//

			switch(messageType)
			{
			case OVR::Log_Error:
				if (!Rift::Log->IsErrorEnabled)
					return;

				break;

			case OVR::Log_Debug:
			case OVR::Log_DebugText:
				if (!Rift::Log->IsDebugEnabled)
					return;

				break;

			case OVR::Log_Assert:
				if (!Rift::Log->IsFatalEnabled)
					return;

				break;

			case OVR::Log_Text:
			default:
				if (!Rift::Log->IsInfoEnabled)
					return;

				break;
			}


			const int BufferLength = 1024;
			char buffer[BufferLength];

			OVR::Log::FormatLog(buffer, BufferLength, messageType, fmt, argList);

			//
			// We want to remove any newline characters from the end of the string.
			// If they are present, they mess with the formatting performed by log4net.
			//

			int length = (int)strlen(buffer);
			int actualLength = length;
			for(int i = length - 1; i >= 0; i--)
			{
				char c = buffer[i];
				if (c != '\r' && c != '\n')
				{
					actualLength = i;
					break;
				}
			}

			auto message = gcnew String(buffer, 0, actualLength);

			switch(messageType)
			{
			case OVR::Log_Error:
				Rift::Log->Error(message);
				break;

			case OVR::Log_Debug:
			case OVR::Log_DebugText:
				Rift::Log->Debug(message);
				break;

			case OVR::Log_Assert:
				Rift::Log->Fatal(message);
				break;

			case OVR::Log_Text:
			default:
				Rift::Log->Info(message);
				break;
			}
		}
	};
}
