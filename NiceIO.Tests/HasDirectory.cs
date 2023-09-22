using System;
using NUnit.Framework;

namespace NiceIO.Tests
{
    [TestFixture]
    public class HasDirectory
    {
        [Test]
        public void MiddleDirectory() => Assert.IsTrue(new NPath("/a/b/c").HasDirectory("b"));

        [Test]
        public void File() => Assert.IsTrue(new NPath("/a/b/c").HasDirectory("c"));

        [Test]
        public void FileShouldNotHave() => Assert.IsFalse(new NPath("/a/b/c.ext").HasDirectory("c"));

        [Test]
        public void DoesNotHave() => Assert.IsFalse(new NPath("/a/b/c").HasDirectory("d"));

        [Test]
        public void Relative() => Assert.IsTrue(new NPath("artifacts/b/c").HasDirectory("artifacts"));

        [Test]
        public void Root() => Assert.Throws<ArgumentException>(() => new NPath("a/b/c").HasDirectory("/"));

        [Test]
        public void LinuxWorkingDirectoryIsNotMatchedAgainst()
        {
            using (NPath.WithFileSystem(new FakeFileSystemForFakeDirectoryTests("/mydir")))
                Assert.IsFalse(new NPath("myrelative/path").HasDirectory("mydir"));
        }

        [Test]
        public void WindowsWorkingDirectoryIsNotMatchedAgainst()
        {
            using (NPath.WithFileSystem(new FakeFileSystemForFakeDirectoryTests("C:/mydir")))
                Assert.IsFalse(new NPath("myrelative/path").HasDirectory("mydir"));
        }

        [Test]
        public void WindowsUNCWorkingDirectoryIsNotMatchedAgainst()
        {
            using (NPath.WithFileSystem(new FakeFileSystemForFakeDirectoryTests("\\\\MyWindowsPC/mydir")))
                Assert.IsFalse(new NPath("myrelative/path").HasDirectory("mydir"));
        }

        [Test]
        public void SingleDotHasSingleDot() => Assert.Throws<ArgumentException>(() => new NPath(".").HasDirectory("."));
    }
}
