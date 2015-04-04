using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace NiceIO.Tests
{
	[TestFixture]
	public class GetHashCode
	{
		[Test]
		public void Test()
		{
			var p1 = new NPath("/b/c");
			var p2 = new NPath("c/d");

			Assert.AreNotEqual(p1.GetHashCode(), p2.GetHashCode());
		}
	}
}
