using System;
using FluentAssertions;
using NUnit.Framework;

namespace RiftDotNet.Test
{
	[TestFixture]
	public sealed class FusionSensorTest
	{
		[Test]
		public void TestCtor()
		{
			

			//
			// Contracts
			//

			new Action(() => Factory.CreateSensorFusion(null)).ShouldThrow<ArgumentNullException>();
		}
	}
}