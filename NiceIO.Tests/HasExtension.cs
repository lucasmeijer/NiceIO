using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace NiceIO.Tests
{
	[TestFixture]
	public class HasExtension
	{
		[Test]
		public void WithDot()
		{
			Assert.IsTrue(new Path("myfile.txt").HasExtension(".txt"));
		}
		
		[Test]
		public void WithoutDot()
		{
			Assert.IsTrue(new Path("myfile.txt").HasExtension("txt"));
		}

		[Test]
		public void WithDotWrongExtension()
		{
			Assert.IsFalse(new Path("myfile.txt").HasExtension(".exe"));
		}

		[Test]
		public void WithoutDotWrongExtension()
		{
			Assert.IsFalse(new Path("myfile.txt").HasExtension("exe"));
		}
	}
}
