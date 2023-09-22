using NUnit.Framework;
using System.Collections;

namespace NiceIO.Tests
{
    [TestFixture]
    public class CurrentDirectory : TestWithTempDir
    {
        [Test]
        public void SetCurrentDirectory()
        {
            var backup = NPath.CurrentDirectory;

            using (NPath.SetCurrentDirectory(_tempPath))
            {
                //only asserting on the filename here, as _tempPath on osx is a symlink, so the folder you get back is not the same as the one you set
                Assert.AreEqual(_tempPath.FileName, NPath.CurrentDirectory.FileName);
            }

            Assert.AreEqual(backup, NPath.CurrentDirectory);
        }

        protected static IEnumerable SetCurrentDirectoryLongPath_TestCases()
        {
            var path = "loremipsumdolorsitametconsecteturadipiscingelit/praesentpharetrapuruseuquamdictumdapibus/crasacorcisapien/necpurusimperdiettempusdiaminhendreritsapien/aliquameratvolutpat/suspendissepotenti/praesenteroslaoreet";
            while (path.Length < 400)
            {
                yield return new TestCaseData(path) { TestName = $"Set current dir with path length {path.Length}" };
                path += "/somesubdirectory";
            }
        }

        [Test]
        [TestCaseSource(nameof(SetCurrentDirectoryLongPath_TestCases))]
        [Category("CrashesNUnitOnSystemNetFramework")]
        public void SetCurrentDirectoryLongPath(string path)
        {
            var backup = NPath.CurrentDirectory;

            var longPath = _tempPath.Combine(path).CreateDirectory();
            longPath.DirectoryMustExist();

            using (NPath.SetCurrentDirectory(longPath))
            {
                //only asserting on the filename here, as _tempPath on osx is a symlink, so the folder you get back is not the same as the one you set
                Assert.AreEqual(longPath.FileName, NPath.CurrentDirectory.FileName);
            }

            Assert.AreEqual(backup, NPath.CurrentDirectory);
        }
    }
}
