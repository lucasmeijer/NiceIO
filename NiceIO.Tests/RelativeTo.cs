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
		public void RelativeAndRelativeNoCommonParent()
		{
			Assert.Throws<ArgumentException>(() => new NPath("mydir1/mydir2/myfile").RelativeTo(new NPath("somethingelse")).ToString(SlashMode.Forward));
		}

		[Test]
		public void ToAnUnrelatedDirectory()
		{
			var relative = new NPath("/mydir1/mydir2/myfile").RelativeTo(new NPath("/unrelated"));
			Assert.AreEqual("../mydir1/mydir2/myfile", relative.ToString(SlashMode.Forward));
			Assert.IsTrue(relative.IsRelative);
		}

		[Test]
		public void RootOfDrive()
		{
			Assert.IsFalse(new NPath("D:\\").IsRelative);
		}

		[Test]
		public void PathRelativeWindowsRoot()
		{
			var relative = new NPath("C:\\mydir1\\mydir2\\myfile").RelativeTo(new NPath("C:\\"));
			Assert.AreEqual("mydir1\\mydir2\\myfile", relative.ToString(SlashMode.Backward));
			Assert.IsTrue(relative.IsRelative);
		}

		[Test]
		public void PathRelativeLinuxRoot()
		{
			var relative = new NPath("/mydir1/mydir2/myfile").RelativeTo(new NPath("/"));
			Assert.AreEqual("mydir1/mydir2/myfile", relative.ToString(SlashMode.Forward));
			Assert.IsTrue(relative.IsRelative);
		}

		[Test]
		public void WindowsPathRelativeToLinuxRootThrows()
		{
			Assert.Throws<ArgumentException>(() => new NPath("C:\\mydir1\\mydir2\\myfile").RelativeTo(new NPath("/")));
		}

		[Test]
		public void LinuxPathRelativeToWindowsRootThrows()
		{
			Assert.Throws<ArgumentException>(() => new NPath("/mydir1/mydir2/myfile").RelativeTo(new NPath("C:\\")));
		}

		[Test]
		public void WhenNotAChildSameLevel()
		{
			var relative = new NPath("/mydir1/mydir2/myfile").RelativeTo(new NPath("/mydir1/mydir2/mydir3"));
			Assert.AreEqual("../myfile", relative.ToString(SlashMode.Forward));
			Assert.IsTrue(relative.IsRelative);
		}

		[Test]
		public void WhenNotAChildSameLevelAndRelative()
		{
			var relative = new NPath("mydir1/mydir2/myfile").RelativeTo(new NPath("mydir1/mydir2/mydir3"));
			Assert.AreEqual("../myfile", relative.ToString(SlashMode.Forward));
			Assert.IsTrue(relative.IsRelative);
		}

		[Test]
		public void WhenNotAChildParentLevel()
		{
			var relative = new NPath("/mydir1/mydir2/myfile").RelativeTo(new NPath("/mydir1/mydir3"));
			Assert.AreEqual("../mydir2/myfile", relative.ToString(SlashMode.Forward));
			Assert.IsTrue(relative.IsRelative);
		}

		[Test]
		public void WhenNotAChildAndManyParentLevels()
		{
			var relative = new NPath("/mydir1/mydir2/myfile").RelativeTo(new NPath("/mydir1/mydir3/mydir3.1/mydir3.2/mydir3.3/mydir3.4"));
			Assert.AreEqual("../../../../../mydir2/myfile", relative.ToString(SlashMode.Forward));
			Assert.IsTrue(relative.IsRelative);
		}

		[Test]
		public void WhenNotAChildAndManySubDirLevels()
		{
			var relative = new NPath("/mydir1/mydir2/mydir2.1/mydir2.2/mydir2.3/mydir2.4/myfile").RelativeTo(new NPath("/mydir1/mydir3"));
			Assert.AreEqual("../mydir2/mydir2.1/mydir2.2/mydir2.3/mydir2.4/myfile", relative.ToString(SlashMode.Forward));
			Assert.IsTrue(relative.IsRelative);
		}
	}
}