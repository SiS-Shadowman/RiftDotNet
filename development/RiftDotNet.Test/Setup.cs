using System;
using System.IO;
using System.Reflection;
using FluentAssertions;
using NUnit.Framework;

namespace RiftDotNet.Test
{
	[SetUpFixture]
	public sealed class Setup
	{
		static Setup()
		{
			AppDomain.CurrentDomain.AssemblyResolve += Resolve;
		}

		[SetUp]
		public void ConfigureLog4Net()
		{
			var fileInfo = new FileInfo(@"Log.config");
			fileInfo.Exists.Should().BeTrue();
			log4net.Config.XmlConfigurator.Configure(fileInfo);
		}

		static private Assembly Resolve(object sender, ResolveEventArgs e)
		{
			if (e.Name == "log4net.dll")
			{
				return Assembly.LoadFrom(@"log4net.dll");
			}

			return null;
		}
	}
}