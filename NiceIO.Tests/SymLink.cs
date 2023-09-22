using System;
using System.Linq;
using NUnit.Framework;

namespace NiceIO.Tests
{
#if NET5_0_OR_GREATER
    [TestFixture]
    [Ignore("Post net5 migration, see if we want to do the work to re-enable these tests or if we want to remove them")]
#endif
    public class SymLink : TestWithTempDir
    {
        [Test]
        public void CanCreateSymbolicLinkWithAbsolutePath()
        {
            var realFile = _tempPath.Combine("RealFile").WriteAllText("Content");
            var symLink = _tempPath.Combine("SymbolicLink").CreateSymbolicLink(realFile.MakeAbsolute());
            Assert.That(symLink.ReadAllText(), Is.EqualTo("Content"));
        }

        [Test]
        public void CanCreateSymbolicLinkWithRelativePath()
        {
            var realFile = _tempPath.Combine("RealFile").WriteAllText("Content");
            using (NPath.SetCurrentDirectory(_tempPath))
            {
                var symLink = _tempPath.Combine("SymbolicLink").CreateSymbolicLink(realFile.FileName);
                Assert.AreEqual($"{_tempPath.ToString(SlashMode.Forward)}/SymbolicLink", symLink.ToString(SlashMode.Forward));
                Assert.That(symLink.IsSymbolicLink, Is.True);
                Assert.IsTrue(symLink.FileExists(), "File SymbolicLink should exist");
                Assert.That(symLink.ReadAllText(), Is.EqualTo("Content"));
            }
        }

        [Test]
        public void CanCreateSymbolicLinkToDirectory()
        {
            _tempPath.CreateDirectory("subdir").Combine("file").WriteAllText("Content");
            using (NPath.SetCurrentDirectory(_tempPath))
            {
                var symLink = _tempPath.Combine("SymbolicLink").CreateSymbolicLink("subdir", false);
                Assert.That(symLink.Combine("file").ReadAllText(), Is.EqualTo("Content"));
            }
        }

        [Test]
        public void IsSymbolicLink_IsTrueForSymlink()
        {
            var realFile = _tempPath.Combine("RealFile").WriteAllText("Content");
            var symLink = _tempPath.Combine("SymbolicLink").CreateSymbolicLink(realFile.FileName);
            Assert.That(symLink.IsSymbolicLink, Is.True);
        }

        [Test]
        public void IsSymbolicLink_IsTrueForSymlink_RelativePath_SameCurrentDirectory()
        {
            var realFile = _tempPath.Combine("RealFile").WriteAllText("Content");
            var symLink = _tempPath.Combine("SymbolicLink").CreateSymbolicLink(realFile.FileName);

            using (NPath.SetCurrentDirectory(_tempPath))
            {
                Assert.IsTrue(new NPath("./RealFile").FileExists());
                Assert.IsTrue(new NPath("./SymbolicLink").FileExists());
                Assert.That(new NPath("./SymbolicLink").IsSymbolicLink, Is.True);
            }
        }

        [Test]
        public void IsSymbolicLink_IsTrueForSymlink_RelativePath_ChangeCurrentDirectory()
        {
            var realFile = _tempPath.Combine("RealFile").WriteAllText("Content");
            var symLink = _tempPath.Combine("SymbolicLink").CreateSymbolicLink(realFile.FileName);

            var subdir = _tempPath.CreateDirectory("subdir");
            using (NPath.SetCurrentDirectory(subdir))
            {
                Assert.IsTrue(new NPath("../RealFile").FileExists());
                Assert.IsTrue(new NPath("../SymbolicLink").FileExists());
                Assert.That(new NPath("../SymbolicLink").IsSymbolicLink, Is.True);
            }
        }

        [Test]
        public void IsSymbolicLink_IsTrueForSymlink_LongPath()
        {
            var longPath = _tempPath.Combine(kLongPath).CreateDirectory();
            var realFile = longPath.Combine("RealFile").WriteAllText("Content");
            var symLink = longPath.Combine("SymbolicLink").CreateSymbolicLink(realFile.FileName);
            Assert.That(symLink.IsSymbolicLink, Is.True);
        }

        [Test]
        [Category("CrashesNUnitOnSystemNetFramework")]
        public void IsSymbolicLink_IsTrueForSymlink_RelativePath_SameCurrentDirectory_LongPath()
        {
            var longPath = _tempPath.Combine(kLongPath).CreateDirectory();
            var realFile = longPath.Combine("RealFile").WriteAllText("Content");
            var symLink = longPath.Combine("SymbolicLink").CreateSymbolicLink(realFile.FileName);

            using (NPath.SetCurrentDirectory(longPath))
            {
                Assert.IsTrue(new NPath("./RealFile").FileExists());
                Assert.IsTrue(new NPath("./SymbolicLink").FileExists());
                Assert.That(new NPath("./SymbolicLink").IsSymbolicLink, Is.True);
            }
        }

        [Test]
        [Category("CrashesNUnitOnSystemNetFramework")]
        public void IsSymbolicLink_IsTrueForSymlink_RelativePath_ChangeCurrentDirectory_LongPath()
        {
            var longPath = _tempPath.Combine(kLongPath).CreateDirectory();
            var realFile = longPath.Combine("RealFile").WriteAllText("Content");
            var symLink = longPath.Combine("SymbolicLink").CreateSymbolicLink(realFile.FileName);

            var subdir = longPath.CreateDirectory("subdir");
            using (NPath.SetCurrentDirectory(subdir))
            {
                Assert.IsTrue(new NPath("../RealFile").FileExists());
                Assert.IsTrue(new NPath("../SymbolicLink").FileExists());
                Assert.That(new NPath("../SymbolicLink").IsSymbolicLink, Is.True);
            }
        }

        [Test]
        public void IsSymbolicLink_IsFalseForRegularFile()
        {
            var realFile = _tempPath.Combine("RealFile").WriteAllText("Content");
            Assert.That(realFile.IsSymbolicLink, Is.False);
        }

        [Test]
        public void IsSymbolicLink_IsFalseForRegularDirectory()
        {
            var subdir = _tempPath.Combine("subdir").CreateDirectory();
            Assert.That(subdir.IsSymbolicLink, Is.False);
        }

        [Test]
        public void CanDeleteSymbolicLinkToFile()
        {
            var realFile = _tempPath.Combine("RealFile").WriteAllText("Content");
            var symLink = _tempPath.Combine("SymbolicLink").CreateSymbolicLink(realFile.FileName);
            symLink.Delete();
            Assert.That(realFile.FileExists());
            Assert.That(!symLink.FileExists());
        }

        [Test]
        public void CanDeleteSymbolicLinkToDirectory()
        {
            var subdir = _tempPath.Combine("subdir").CreateDirectory();
            var symLink = _tempPath.Combine("SymbolicLink").CreateSymbolicLink(subdir, false);
            symLink.Delete();
            Assert.That(subdir.DirectoryExists());
            Assert.That(!symLink.Exists());
        }

        // When running under Mono on Windows, it seems that Mono has trouble recursively deleting a directory if it
        // contains a symlink to another directory. So, manually clean up such symlinks here.
        [TearDown]
        public void DeleteTempDirContents()
        {
            foreach (var symlink in _tempPath.Directories().Where(d => d.IsSymbolicLink))
            {
                Console.WriteLine($"Cleaning up {symlink}");
                symlink.Delete();
            }
        }
    }
}
