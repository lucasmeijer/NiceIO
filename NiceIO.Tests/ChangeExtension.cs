using System;
using NUnit.Framework;

namespace NiceIO.Tests
{
    [TestFixture]
    public class ChangeExtension
    {
        [Test]
        public void WithoutDot()
        {
            var p1 = new NPath("/my/path/file.txt");
            var p2 = new NPath("/my/path/file.mp4").ChangeExtension("txt");
            Assert.AreEqual(p1, p2);
        }

        [Test]
        public void WithDot()
        {
            var p1 = new NPath("/my/path/file.txt");
            var p2 = new NPath("/my/path/file.mp4").ChangeExtension(".txt");
            Assert.AreEqual(p1, p2);
        }

        [Test]
        public void OnPathWithMultipleDots()
        {
            var p1 = new NPath("/my/path/file.something.txt");
            var p2 = new NPath("/my/path/file.something.mp4").ChangeExtension(".txt");
            Assert.AreEqual(p1, p2);
        }

        [Test]
        public void ToEmptyString()
        {
            var expected = new NPath("/my/path/file.something");
            var actual = new NPath("/my/path/file.something.exe").ChangeExtension(string.Empty);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void EndsWithSingleDot()
        {
            Assert.AreEqual("/my/path/file", new NPath("/my/path/file.").ChangeExtension(string.Empty).ToString());
        }

        [Test]
        public void NoExtension()
        {
            Assert.AreEqual("/my/path/file.cs", new NPath("/my/path/file").ChangeExtension("cs").ToString());
        }

        [Test]
        public void FolderWithExtension()
        {
            Assert.AreEqual("/my/mydir.txt", new NPath("/my/mydir.something/").ChangeExtension("txt").ToString());
        }

        [Test]
        public void CannotChangeTheExtensionOfAWindowsRootDirectory()
        {
            Assert.Throws<ArgumentException>(() => new NPath("C:\\").ChangeExtension(".txt"));
        }

        [Test]
        public void CannotChangeTheExtensionOfAWindowsUNCRootDirectory()
        {
            Assert.Throws<ArgumentException>(() => new NPath("\\\\MyWindowsPC\\").ChangeExtension(".txt"));
        }

        [Test]
        public void CannotChangeTheExtensionOfALinuxRootDirectory()
        {
            Assert.Throws<ArgumentException>(() => new NPath("/").ChangeExtension(".txt"));
        }
    }
}
