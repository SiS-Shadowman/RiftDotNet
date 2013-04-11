using FluentAssertions;
using NUnit.Framework;

namespace RiftDotNet.Test
{
	[TestFixture]
	public sealed class FactoryTest
	{
		[Test]
		public void TestCtor()
		{
			IFactory f = Factory.PlatformFactory;
			f.Should().NotBeNull();
		}
	}
}