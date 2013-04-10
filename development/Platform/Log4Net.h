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
			const int BufferLength = 1024;
			char buffer[BufferLength];

			OVR::Log::FormatLog(buffer, BufferLength, messageType, fmt, argList);
			auto message = gcnew String(buffer);

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
