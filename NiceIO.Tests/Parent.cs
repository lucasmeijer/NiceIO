using System;
using NUnit.Framework;

namespace NiceIO.Tests
{
	[TestFixture]
	public class Parent
	{
		[Test]
		public void ParentFromFileInDirectory()
		{
			var path = new Path("mydir/myotherdir/myfile.exe").Parent();
			Assert.AreEqual("mydir/myotherdir", path.ToString());
		}

		[Test]
		public void ParentFromFile()
		{
			var path = new Path("myfile.exe").Parent();
			Assert.AreEqual("", path.ToString());
		}

		[Test]
		public void ParentFromFileInRoot()
		{
			var path = new Path("/myfile.exe").Parent();
			Assert.AreEqual("/", path.ToString());
		}

		[Test]
		public void ParentFromEmpty()
		{
			Assert.Throws<InvalidOperationException>(() => new Path("/").Parent());
		}
	}
}