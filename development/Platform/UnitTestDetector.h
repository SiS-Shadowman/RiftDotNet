#pragma once

using namespace System;
using namespace System::Reflection;




namespace RiftDotNet
{
	namespace Platform
	{
		/// <summary>
		/// Detect if we are running as part of a nUnit unit test.
		/// This is DIRTY and should only be used if absolutely necessary 
		/// as its usually a sign of bad design.
		/// </summary>
		/// <remarks>
		/// Source: http://stackoverflow.com/questions/3167617/determine-if-code-is-running-as-part-of-a-unit-test
		/// </remarks>
		ref class UnitTestDetector
		{
		public:

			property static bool IsRunningFromNunit
			{
				bool get() { return _runningFromNUnit; }
			}

		private:

			static bool _runningFromNUnit = false;

			static UnitTestDetector()
			{
				for each (Assembly^ assem in AppDomain::CurrentDomain->GetAssemblies())
				{
					// Can't do something like this as it will load the nUnit assembly
					// if (assem == typeof(NUnit.Framework.Assert))

					if (assem->FullName->ToLowerInvariant()->StartsWith("nunit.framework"))
					{
						_runningFromNUnit = true;
						break;
					}
				}
			}
		};
	}
}