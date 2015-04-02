using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace NiceIO.Tests
{
	[TestFixture]
	public class SpecialPaths
	{
		[Test]
		public void CurrentDirectory()
		{
			Assert.IsTrue(NPath.CurrentDirectory.DirectoryExists());
		}

		[Test]
		public void HomeDirectory()
		{
			Console.WriteLine(NPath.HomeDirectory);
			Assert.IsTrue(NPath.HomeDirectory.DirectoryExists());
		}
	}
}
