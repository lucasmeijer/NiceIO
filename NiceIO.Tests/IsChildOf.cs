using NUnit.Framework;

namespace NiceIO.Tests
{
    [TestFixture]
    public class IsChildOf : TestWithTempDir
    {
        [Test]
        public void FileInDir()
        {
            Assert.IsTrue(_tempPath.Combine("file").IsChildOf(_tempPath));
        }

        [Test]
        public void FileInSubDir()
        {
            Assert.IsTrue(_tempPath.Combine("subdir/file").IsChildOf(_tempPath));
        }

        [Test]
        public void FileWithSameNameButLonger()
        {
            Assert.IsFalse(new NPath("my/directory").IsChildOf("my/dir"));
        }

        [Test]
        public void UnrelatedPaths()
        {
            Assert.IsFalse(new NPath("/a/b/c").IsChildOf("/unrelated"));
        }

        [Test]
        public void OfRoot()
        {
            Assert.IsTrue(new NPath("/a/b/c").IsChildOf("/"));
        }

        [Test]
        public void AbsoluteAndRelative()
        {
            Assert.IsFalse(new NPath("/a/b/c").IsChildOf("the/relative"));
        }

        [Test]
        public void RelativeAndAbsolute()
        {
            Assert.IsFalse(new NPath("relative").IsChildOf("/a/b/c"));
        }

        [Test]
        public void RelativeFileIsChildOfCurrentDirectory()
        {
            Assert.IsTrue(new NPath("relative").IsChildOf(NPath.CurrentDirectory));
        }

        [Test]
        public void TwoRelative()
        {
            Assert.IsTrue(new NPath("hello/there").IsChildOf(new NPath("hello")));
        }

        [Test]
        public void TwoRelativeUnrelated()
        {
            Assert.IsFalse(new NPath("hello/there").IsChildOf(new NPath("boink")));
        }

        [Test]
        public void WindowsPathIsNotChildOfLinuxRoot()
        {
            Assert.IsFalse(new NPath("C:\\hello\\there").IsChildOf(new NPath("/")));
        }

        [Test]
        public void WindowsPathIsNotChildOfWindowsUNCRoot()
        {
            Assert.IsFalse(new NPath("C:\\hello\\there").IsChildOf(new NPath("\\\\MyWindowsPC\\")));
        }

        [Test]
        public void WindowsUNCPathIsNotChildOfLinuxRoot()
        {
            Assert.IsFalse(new NPath("\\\\MyWindowssPC\\hello\\there").IsChildOf(new NPath("/")));
        }

        [Test]
        public void WindowsUNCPathIsNotChildOfWindowsRoot()
        {
            Assert.IsFalse(new NPath("\\\\MyWindowssPC\\hello\\there").IsChildOf(new NPath("C:\\")));
        }

        [Test]
        public void LinuxPathIsNotChildOfWindowsRoot()
        {
            Assert.IsFalse(new NPath("/hello/there").IsChildOf(new NPath("C:\\")));
        }

        [Test]
        public void LinuxPathIsNotChildOfWindowsUNCRoot()
        {
            Assert.IsFalse(new NPath("/hello/there").IsChildOf(new NPath("\\\\MyWindowsPC\\")));
        }

        [Test]
        public void SameAsOrChildOf_ReturnsTrueWhenSameAs()
        {
            Assert.That(_tempPath.IsSameAsOrChildOf(_tempPath));
        }

        [Test]
        public void SameAsOrChildOf_ReturnsTrueWhenSameAs_WithAbsolutePaths()
        {
            Assert.That(_tempPath.MakeAbsolute().IsSameAsOrChildOf(_tempPath));
            Assert.That(_tempPath.IsSameAsOrChildOf(_tempPath.MakeAbsolute()));
        }

        [Test]
        public void SameAsOrChildOf_ReturnsTrueWhenChildOf()
        {
            Assert.That(_tempPath.Combine("testdir").IsSameAsOrChildOf(_tempPath));
        }

        [Test]
        public void ChildOfCurrentDir()
        {
            Assert.IsTrue(new NPath("my/file").IsChildOf(new NPath(".")));
        }

        [Test]
        public void LooksLikeChildOfCurrentDirButISNot()
        {
            Assert.IsFalse(new NPath("../my/file").IsChildOf(new NPath(".")));
        }
    }
}
