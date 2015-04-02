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
			Assert.IsTrue(new NPath("myfile.txt").HasExtension(".txt"));
		}
		
		[Test]
		public void WithoutDot()
		{
			Assert.IsTrue(new NPath("myfile.txt").HasExtension("txt"));
		}

		[Test]
		public void IsCaseInsensitive()
		{
			Assert.IsTrue(new NPath("myfile.txt").HasExtension("TxT"));
		}

		[Test]
		public void WithDotWrongExtension()
		{
			Assert.IsFalse(new NPath("myfile.txt").HasExtension(".exe"));
		}

		[Test]
		public void WithoutDotWrongExtension()
		{
			Assert.IsFalse(new NPath("myfile.txt").HasExtension("exe"));
		}

		[Test]
		public void WithMultipleArguments_Params()
		{
			Assert.IsTrue(new NPath("myfile.txt").HasExtension("exe","txt"));
		}
	}
}
