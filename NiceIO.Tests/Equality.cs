using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace NiceIO.Tests
{
	[TestFixture]
	public class Equality
	{
		[Test]
		public void SingleFile()
		{
			var path1 = new Path("myfile");
			var path2 = new Path("myfile");
			Assert.IsTrue(path1.Equals(path2));
			Assert.IsTrue(path1 == path2);
		}
	}
}
