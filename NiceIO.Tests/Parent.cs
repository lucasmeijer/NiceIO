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
			var path = new NPath("mydir/myotherdir/myfile.exe").Parent;
			Assert.AreEqual("mydir/myotherdir", path.ToString(SlashMode.Forward));
		}

		[Test]
		public void ParentFromFile()
		{
			var path = new NPath("myfile.exe").Parent;
			Assert.AreEqual(".", path.ToString());
		}

		[Test]
		public void ParentFromFileInRoot()
		{
			var path = new NPath("/myfile.exe").Parent;
			Assert.AreEqual("/", path.ToString(SlashMode.Forward));
		}

		[Test]
		public void ParentFromEmpty()
		{
			Assert.Throws<InvalidOperationException>(() => { var p = new NPath ("/").Parent; });
		}
	}
}