using System;
using NUnit.Framework;

namespace NiceIO.Tests
{
    public static class NiceIOTestExtension
    {
        public static void AssertIs(this NPath _this, string value)
        {
            Assert.AreEqual(value, _this.ToString());
        }
    }


    [TestFixture]
    public class RelativeTo
    {
        [Test]
        public void ToBaseDirectory()
        {
            var relative = new NPath("/mydir1/mydir2/myfile").RelativeTo(new NPath("/mydir1"));

            Assert.AreEqual("mydir2/myfile", relative.ToString());
            Assert.IsTrue(relative.IsRelative);
        }

        [Test]
        public void RootedAndRelative()
        {
	        using (NPath.WithFileSystem(new FakeFileSystemForFakeDirectoryTests("/")))
                new NPath("/mydir1/mydir2/myfile").RelativeTo("myfile").AssertIs("../mydir1/mydir2/myfile");
        }

        [Test]
        public void RelativeAndRooted()
        {
            using (NPath.WithFileSystem(new FakeFileSystemForFakeDirectoryTests("/")))
                new NPath("mydir1/mydir2/myfile").RelativeTo(new NPath("/mydir1")).AssertIs("mydir2/myfile");
        }

        [Test]
        public void RelativeAndRooted2()
        {
            using (NPath.WithFileSystem(new FakeFileSystemForFakeDirectoryTests("/some_other_dir")))
                new NPath("mydir1/mydir2/myfile").RelativeTo(new NPath("/mydir1")).AssertIs("../some_other_dir/mydir1/mydir2/myfile");
        }

        [Test]
        public void WithDifferentDriveLettersReturnsAbsolutePath()
        {
            new NPath("c:/mydir1/mydir2/myfile").RelativeTo(new NPath("d:/mydir1")).AssertIs("c:/mydir1/mydir2/myfile");
        }

        [Test]
        public void WithDifferentUNCServerNamesReturnsAbsolutePath()
        {
            new NPath("\\\\MyWindowsPC1/mydir1/mydir2/myfile").RelativeTo(new NPath("\\\\MyWindowsPC2/mydir1")).AssertIs("\\\\MyWindowsPC1/mydir1/mydir2/myfile");
        }

        [Test]
        public void RelativeAndRelative()
        {
            new NPath("mydir1/mydir2/myfile").RelativeTo(new NPath("mydir1")).AssertIs("mydir2/myfile");
        }

        [Test]
        public void RelativeAndRelativeNoCommonParent()
        {
            new NPath("mydir1/mydir2/myfile").RelativeTo(new NPath("somethingelse")).AssertIs("../mydir1/mydir2/myfile");
        }

        [Test]
        public void ToAnUnrelatedDirectory()
        {
            var relative = new NPath("/mydir1/mydir2/myfile").RelativeTo(new NPath("/unrelated"));
            Assert.AreEqual("../mydir1/mydir2/myfile", relative.ToString());
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
        public void PathRelativeWindowsUNCRoot()
        {
            var relative = new NPath("\\\\MyWindowssPC\\mydir1\\mydir2\\myfile").RelativeTo(new NPath("\\\\MyWindowssPC\\"));
            Assert.AreEqual("mydir1\\mydir2\\myfile", relative.ToString(SlashMode.Backward));
            Assert.IsTrue(relative.IsRelative);
        }

        [Test]
        public void PathRelativeLinuxRoot()
        {
            var relative = new NPath("/mydir1/mydir2/myfile").RelativeTo(new NPath("/"));
            Assert.AreEqual("mydir1/mydir2/myfile", relative.ToString());
            Assert.IsTrue(relative.IsRelative);
        }

        [Test]
        public void WindowsPathRelativeToLinuxRootIsFalseIfOnDifferentDrive()
        {
            new NPath("C:\\mydir1\\mydir2\\myfile").RelativeTo(new NPath("/")).AssertIs("C:/mydir1/mydir2/myfile");
        }

        [Test]
        public void WindowsUNCPathRelativeToLinuxRootIsFalseIfOnDifferentDrive()
        {
            new NPath("\\\\MyWindowsPC\\mydir1\\mydir2\\myfile").RelativeTo(new NPath("/")).AssertIs("\\\\MyWindowsPC/mydir1/mydir2/myfile");
        }

        [Test]
        public void WindowsPathRelativeToWindowsUNCReturnsAbsolutePath()
        {
            new NPath("C:\\mydir1\\mydir2\\myfile").RelativeTo(new NPath("\\\\MyWindowsPC\\")).AssertIs("C:/mydir1/mydir2/myfile");
        }

        [Test]
        public void LinuxPathRelativeToWindowsReturnsAbsolutePath()
        {
            new NPath("/mydir1/mydir2/myfile").RelativeTo(new NPath("C:\\")).AssertIs("/mydir1/mydir2/myfile");
        }

        [Test]
        public void LinuxPathRelativeToWindowsUNCReturnsAbsolutePath()
        {
            new NPath("/mydir1/mydir2/myfile").RelativeTo(new NPath("\\\\MyWindowsPC\\")).AssertIs("/mydir1/mydir2/myfile");
        }

        [Test]
        public void WindowsUNCPathRelativeToWindowsReturnAbsolutePath()
        {
            new NPath("\\\\MyWindowsPC\\mydir1\\mydir2\\myfile").RelativeTo(new NPath("C:\\")).AssertIs("\\\\MyWindowsPC/mydir1/mydir2/myfile");
        }

        [Test]
        public void WhenNotAChildSameLevel()
        {
            var relative = new NPath("/mydir1/mydir2/myfile").RelativeTo(new NPath("/mydir1/mydir2/mydir3"));
            Assert.AreEqual("../myfile", relative.ToString());
            Assert.IsTrue(relative.IsRelative);
        }

        [Test]
        public void WhenNotAChildSameLevelAndRelative()
        {
            var relative = new NPath("mydir1/mydir2/myfile").RelativeTo(new NPath("mydir1/mydir2/mydir3"));
            Assert.AreEqual("../myfile", relative.ToString());
            Assert.IsTrue(relative.IsRelative);
        }

        [Test]
        public void WhenNotAChildParentLevel()
        {
            var relative = new NPath("/mydir1/mydir2/myfile").RelativeTo(new NPath("/mydir1/mydir3"));
            Assert.AreEqual("../mydir2/myfile", relative.ToString());
            Assert.IsTrue(relative.IsRelative);
        }

        [Test]
        public void WhenNotAChildAndManyParentLevels()
        {
            var relative = new NPath("/mydir1/mydir2/myfile").RelativeTo(new NPath("/mydir1/mydir3/mydir3.1/mydir3.2/mydir3.3/mydir3.4"));
            Assert.AreEqual("../../../../../mydir2/myfile", relative.ToString());
            Assert.IsTrue(relative.IsRelative);
        }

        [Test]
        public void WhenNotAChildAndManySubDirLevels()
        {
            var relative = new NPath("/mydir1/mydir2/mydir2.1/mydir2.2/mydir2.3/mydir2.4/myfile").RelativeTo(new NPath("/mydir1/mydir3"));
            Assert.AreEqual("../mydir2/mydir2.1/mydir2.2/mydir2.3/mydir2.4/myfile", relative.ToString());
            Assert.IsTrue(relative.IsRelative);
        }

        [Test]
        public void IdenticalPaths()
        {
            new NPath("/one/file").RelativeTo("/one/file").AssertIs(".");
        }

        [Test]
        public void PathsThatLookRelativeToADumbStringCompare()
        {
            new NPath("/averylongname/myfile").RelativeTo("/avery").AssertIs("../averylongname/myfile");
        }

        [Test]
        public void PathsConsideredNonOrdinallyEqual()
        {
            new NPath("/sß/file").RelativeTo("/ßs").AssertIs("../sß/file");
        }
    }
}
