using System;
using NUnit.Framework;

namespace NiceIO.Tests
{
	[TestFixture]
	public class RelativeTo
	{
		[Test]
		public void ToBaseDirectory()
		{
			var relative = new NPath("/mydir1/mydir2/myfile").RelativeTo(new NPath("/mydir1"));
			Assert.AreEqual("mydir2/myfile", relative.ToString(SlashMode.Forward));
			Assert.IsTrue(relative.IsRelative);
		}

		[Test]
		public void RootedAndRelative()
		{
			Assert.Throws<ArgumentException>(() => new NPath("/mydir1/mydir2/myfile").RelativeTo(new NPath("myfile")));
		}

		[Test]
		public void RelativeAndRooted()
		{
			Assert.Throws<ArgumentException>(() => new NPath("mydir1/mydir2/myfile").RelativeTo(new NPath("/mydir1")));
		}

		[Test]
		public void WithDifferentDriveLetters()
		{
			Assert.Throws<ArgumentException>(() => new NPath("c:/mydir1/mydir2/myfile").RelativeTo(new NPath("d:/mydir1")));
		}

		[Test]
		public void RelativeAndRelative()
		{
			Assert.AreEqual("mydir2/myfile", new NPath("mydir1/mydir2/myfile").RelativeTo(new NPath("mydir1")).ToString(SlashMode.Forward));
		}

		[Test]
		public void ToAnUnrelatedDirectory()
		{
			Assert.Throws<ArgumentException>(() => new NPath("/mydir1/mydir2/myfile").RelativeTo(new NPath("/unrelated")));
		}

		[Test]
		public void RootOfDrive()
		{
			Assert.IsFalse(new NPath("D:\\").IsRelative);
		}
	}
}