using System.IO;
using FluentAssertions;
using NUnit.Framework;

namespace RiftDotNet.Test
{
	[SetUpFixture]
	public sealed class Setup
	{
		[SetUp]
		public void ConfigureLog4Net()
		{
			var fileInfo = new FileInfo(@"Log.config");
			fileInfo.Exists.Should().BeTrue();
			log4net.Config.XmlConfigurator.Configure(fileInfo);
		}
	}
}