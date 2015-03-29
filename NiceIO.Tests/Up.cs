using System;
using NUnit.Framework;

namespace NiceIO.Tests
{
	[TestFixture]
	public class Up
	{
		[Test]
		public void UpFromFileInDirectory()
		{
			var path = new Path("mydir/myotherdir/myfile.exe").Up();
			Assert.AreEqual("mydir/myotherdir", path.ToString());
		}

		[Test]
		public void UpFromFile()
		{
			var path = new Path("myfile.exe").Up();
			Assert.AreEqual("", path.ToString());
		}

		[Test]
		public void UpFromFileInRoot()
		{
			var path = new Path("/myfile.exe").Up();
			Assert.AreEqual("/", path.ToString());
		}

		[Test]
		public void UpFromEmpty()
		{
			Assert.Throws<InvalidOperationException>(() => new Path("/").Up());
		}
	}
}