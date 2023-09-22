using System.IO;
using System.Linq;
using NUnit.Framework;

namespace NiceIO.Tests
{
    [TestFixture]
    public class Contents : TestWithTempDir
    {
        [Test]
        public void Files()
        {
            var files = new[] {"myfile.txt", "myfile2.exe"};
            PopulateTempDir(files);

            CollectionAssert.AreEquivalent(files, _tempPath.Files().Select(p => p.FileName));
        }

        [Test]
        [TestCase(256)]
        [TestCase(257)]
        [TestCase(258)]
        [TestCase(259)]
        [TestCase(260)]
        [TestCase(261)]
        [TestCase(262)]
        public void Files_LongPaths(int length)
        {
            var sub = _tempPath.Combine("somedirectorywithalongname/somedirectorywithalongname/somedirectorywithalongname").CreateDirectory();
            var name = new string('a', length - sub.ToString(SlashMode.Native).Length - 1);
            var longPath = sub.Combine(name).CreateDirectory();
            Assert.AreEqual(length, longPath.ToString().Length);

            var files = new[] { "myfile.txt", "myfile2.exe" };
            longPath.Combine("myfile.txt").WriteAllText("hello");
            longPath.Combine("myfile2.exe").WriteAllText("hello");

            CollectionAssert.AreEquivalent(files, longPath.Files().Select(p => p.FileName));
        }

        [Test]
        public void FilesWithWildcard()
        {
            var files = new[] { "myfile.txt", "myfile2.exe" };
            PopulateTempDir(files);

            var enumerable = _tempPath.Files("*.txt");
            Assert.AreEqual(_tempPath.Combine("myfile.txt"), enumerable.Single());
        }

        [Test]
        [TestCase(256, "*.txt")]
        [TestCase(257, "*.txt")]
        [TestCase(258, "*.txt")]
        [TestCase(259, "*.txt")]
        [TestCase(260, "*.txt")]
        [TestCase(261, "*.txt")]
        [TestCase(262, "*.txt")]
        [TestCase(256, "*.extension")]
        [TestCase(257, "*.extension")]
        [TestCase(258, "*.extension")]
        [TestCase(259, "*.extension")]
        [TestCase(260, "*.extension")]
        [TestCase(261, "*.extension")]
        [TestCase(262, "*.extension")]
        public void FilesWithWildcard_LongPaths(int length, string filter)
        {
            var sub = _tempPath.Combine("somedirectorywithalongname/somedirectorywithalongname/somedirectorywithalongname").CreateDirectory();
            var name = new string('a', length - sub.ToString(SlashMode.Native).Length - 2 - filter.Length);
            var longPath = sub.Combine(name).CreateDirectory();
            Assert.AreEqual(length, longPath.ToString().Length + 1 + filter.Length);

            var files = new[] { "myfile.txt", "myfile2.exe" };
            longPath.Combine("myfile.txt").WriteAllText("hello");
            longPath.Combine("myfile.extension").WriteAllText("hello");

            var enumerable = longPath.Files(filter);
            Assert.AreEqual(longPath.Combine("myfile" + filter.Substring(1)), enumerable.Single());
        }

        [Test]
        public void FilesRecursive()
        {
            PopulateTempDir(new[] {"myfile.txt", "mydir/", "mydir/myfile2.txt"});

            CollectionAssert.AreEquivalent(new[] {"myfile.txt", "myfile2.txt"}, _tempPath.Files(recurse: true).Select(p => p.FileName));
        }

        [Test]
        public void ContentsWithWildcard()
        {
            PopulateTempDir(new[] { "myfile.txt", "mydir/", "mydir2/", "otherdir/", "mydir/mysubdir/", "otherfile" });

            CollectionAssert.AreEquivalent(new[] { "myfile.txt", "mydir", "mydir2" }, _tempPath.Contents("my*").Select(p => p.FileName));
        }

        [Test]
        public void ContentsRecursive()
        {
            PopulateTempDir(new[] {"myfile.txt", "mydir/", "mydir/myfile2.txt", "mydir/mysubdir/", "mydir/mysubdir/myfile3"});

            CollectionAssert.AreEquivalent(new[] { "myfile.txt", "myfile2.txt", "mydir", "mysubdir", "myfile3" }, _tempPath.Contents(recurse: true).Select(p => p.FileName));
        }

        [Test]
        public void DirectoriesRecursive()
        {
            PopulateTempDir(new[] { "myfile.txt", "mydir/", "mydir/myfile2.txt", "mydir/mysubdir/", "mydir/mysubdir/myfile3" });

            CollectionAssert.AreEquivalent(new[] { "mydir", "mysubdir" }, _tempPath.Directories(recurse: true).Select(p => p.FileName));
        }

        [Test]
        public void FilesRecursive_InLongPath()
        {
            var longPath = PopulateSpecificDir(kLongPath, new[] { "myfile.txt", "myfile.exe", "mydir/", "mydir/myfile2.txt" });
            CollectionAssert.AreEquivalent(new[] { "myfile.txt", "myfile.exe", "myfile2.txt" }, longPath.Files(true).Select(p => p.FileName));
        }

        [Test]
        public void ContentsRecursive_InLongPath()
        {
            var longPath = PopulateSpecificDir(kLongPath, new[] { "myfile.txt", "myfile.exe", "mydir/", "mydir/myfile2.txt" });
            CollectionAssert.AreEquivalent(new[] { "mydir", "myfile.txt", "myfile.exe", "myfile2.txt" }, longPath.Contents(true).Select(p => p.FileName));
        }

        [Test]
        public void DirectoriesRecursive_InLongPath()
        {
            var longPath = PopulateSpecificDir(kLongPath, new[] { "myfile.txt", "myfile.exe", "mydir/", "mydir/myfile2.txt", "mydir/mysubdir/" });
            CollectionAssert.AreEquivalent(new[] { "mydir", "mysubdir" }, longPath.Directories(true).Select(p => p.FileName));
        }

        [Test]
        public void ContentsOfRelativeCurrentDirectory()
        {
            PopulateTempDir(new[] { "myfile.txt", "sourceDir\\", "sourceDir\\source.cpp" });

            var originalCurrentDirectory = Directory.GetCurrentDirectory();
            Directory.SetCurrentDirectory(_tempPath.ToString());

            var currentDirRelative = new NPath("myfile.txt").Parent;
            CollectionAssert.AreEquivalent(new[] { "myfile.txt", "source.cpp" }, currentDirRelative.Files(recurse: true).Select(p => p.FileName));

            Directory.SetCurrentDirectory(originalCurrentDirectory);
        }

        [Test]
        public void DepthWindowsPath()
        {
            Assert.That(new NPath("C:\\my\\path\\is\\kind\\of\\long").Depth, Is.EqualTo(6));
        }

        [Test]
        public void DepthWindowsUNCPath()
        {
            Assert.That(new NPath("\\\\MyWindowsPC\\my\\path\\is\\kind\\of\\long").Depth, Is.EqualTo(6));
        }

        [Test]
        public void DepthLinuxPath()
        {
            Assert.That(new NPath("/my/path/is/kind/of/long").Depth, Is.EqualTo(6));
        }

        [Test]
        public void DepthRelativePath()
        {
            Assert.That(new NPath("my/path/is/kind/of/long").Depth, Is.EqualTo(6));
        }
    }
}
