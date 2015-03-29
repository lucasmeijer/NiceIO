using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace NiceIO.Tests
{
	[TestFixture]
	public class CreateTempDirectory
	{
		[Test]
		public void Test()
		{
			var path = Path.CreateTempDirectory("myprefix");
			Assert.IsTrue(Directory.Exists(path.ToString()));
			Assert.IsFalse(path.IsRelative);
			StringAssert.Contains("myprefix",path.ToString());
		}
	}
}
