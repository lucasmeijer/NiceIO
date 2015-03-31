using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace NiceIO.Tests
{
	[TestFixture]
	public class ToString
	{
		[Test]
		public void Forward()
		{
			Assert.AreEqual("a/b", new Path("a/b").ToString(SlashMode.Forward));
		}
		
		[Test]
		public void Backward()
		{
			Assert.AreEqual("a/b", new Path(@"a\b").ToString(SlashMode.Backward));
		}
	}
}
