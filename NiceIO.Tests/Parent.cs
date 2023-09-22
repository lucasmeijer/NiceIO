using System;
using System.Linq;
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
            Assert.AreEqual("mydir/myotherdir", path.ToString());
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
            Assert.AreEqual("/", path.ToString());
        }

        [Test]
        public void ParentFromFileInRootWithDriveLetter()
        {
            var nPath = new NPath("C:/myfile.exe");
            var parent = nPath.Parent;
            var actual = parent.ToString();

            Assert.AreEqual("C:/", actual);
        }

        [Test]
        public void ParentFromFileInRootWithUNCServerName()
        {
            var nPath = new NPath("\\\\MyWindowsPC/myfile.exe");
            var parent = nPath.Parent;
            var actual = parent.ToString();

            Assert.AreEqual("\\\\MyWindowsPC/", actual);
        }

        [Test]
        public void ParentFromEmpty()
        {
            Assert.Throws<ArgumentException>(() => { var p = new NPath("/").Parent; });
        }

        [Test]
        public void RecursiveParentsStartFromFileToRoot()
        {
            var path = new NPath("/mydir/myotherdir/myfile");

            var result = path.RecursiveParents.ToArray();

            Assert.That(result[0], Is.EqualTo(new NPath("/mydir/myotherdir")));
            Assert.That(result[1], Is.EqualTo(new NPath("/mydir")));
            Assert.That(result[2], Is.EqualTo(new NPath("/")));
        }

        [Test]
        public void RecursiveParentsStartFromFile()
        {
            var path = NPath.CurrentDirectory.Combine("mydir/myotherdir/myfile.exe");

            var result = path.RecursiveParents.ToArray();

            Assert.That(result[0], Is.EqualTo(path.Parent));
            Assert.That(result[1], Is.EqualTo(path.Parent.Parent));

            // We can only assert up to the root directory we started at.  After that we don't really know what to expect
            Assert.That(result[2], Is.EqualTo(NPath.CurrentDirectory));
        }

        [Test]
        public void RelativeRecursiveParentsStartFromFile()
        {
            var path = new NPath("mydir/myotherdir/myfile.exe");

            var result = path.RecursiveParents.ToArray();

            Assert.That(result[0], Is.EqualTo(path.Parent));
            Assert.That(result[1], Is.EqualTo(path.Parent.Parent));
        }

        [Test]
        public void RecursiveParentsStartFromFileAllTheWayToWindowsRoot()
        {
            var path = new NPath("C:\\mydir\\myotherdir\\myfile.exe");

            Assert.That(path.RecursiveParents, Is.EqualTo(new[] { path.Parent, path.Parent.Parent, path.Parent.Parent.Parent }));
        }

        [Test]
        public void RecursiveParentsStartFromWindowsRoot()
        {
            var path = new NPath("C:\\");

            Assert.That(path.RecursiveParents, Is.EqualTo(new NPath[] {}));
        }

        [Test]
        public void RecursiveParentsStartFromFileAllTheWayToWindowsUNCRoot()
        {
            var path = new NPath("\\\\MyWindowsPC\\mydir\\myotherdir\\myfile.exe");

            Assert.That(path.RecursiveParents, Is.EqualTo(new[] { path.Parent, path.Parent.Parent, path.Parent.Parent.Parent }));
        }

        [Test]
        public void RecursiveParentsStartFromWindowsUNCRoot()
        {
            var path = new NPath("\\\\MyWindowsPC\\");

            Assert.That(path.RecursiveParents, Is.EqualTo(new NPath[] {}));
        }

        [Test]
        public void RecursiveParentsStartFromFileAllTheWayLinuxRoot()
        {
            var path = new NPath("/mydir/myotherdir/myfile.exe");

            Assert.That(path.RecursiveParents, Is.EqualTo(new[] { path.Parent, path.Parent.Parent, path.Parent.Parent.Parent }));
        }

        [Test]
        public void RecursiveParentsStartFromWLinuxRoot()
        {
            var path = new NPath("/");

            Assert.That(path.RecursiveParents, Is.EqualTo(new NPath[] {}));
        }

        [Test]
        public void RecursiveParentsStartFromDirectory()
        {
            var path = NPath.CurrentDirectory.Combine("mydir/myotherdir/mydir/");

            var result = path.RecursiveParents.ToArray();

            Assert.That(result[0], Is.EqualTo(path.Parent));
            Assert.That(result[1], Is.EqualTo(path.Parent.Parent));

            // We can only assert up to the root directory we started at.  After that we don't really know what to expect
            Assert.That(result[2], Is.EqualTo(NPath.CurrentDirectory));
        }
    }
}
