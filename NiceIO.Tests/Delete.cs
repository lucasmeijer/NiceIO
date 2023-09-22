using System;
using System.IO;
using System.Linq;
using System.Threading;
using NUnit.Framework;

namespace NiceIO.Tests
{
    [TestFixture]
    public class Delete : TestWithTempDir
    {
        [Test]
        public void DeleteFile()
        {
            PopulateTempDir(new[] {"somefile"});

            var path = _tempPath.Combine("somefile");
            Assert.IsTrue(path.FileExists());
            path.Delete();

            AssertTempDir(new string[0]);
        }

        [Test]
        public void DeleteFileLongPath()
        {
            var longPath = PopulateSpecificDir(kLongPath, new[] { "somefile" });

            var path = longPath.Combine("somefile");
            Assert.IsTrue(path.FileExists());
            path.Delete();
            Assert.IsFalse(path.FileExists());
        }

        [Test]
        public void DeleteDirectory()
        {
            PopulateTempDir(new[]
            {
                "somedir/",
                "somedir/somefile"
            });

            var path = _tempPath.Combine("somedir");
            Assert.IsTrue(path.DirectoryExists());

            path.Delete();

            AssertTempDir(new string[0]);
        }

        [Test]
        public void DeleteDirectoryLongPath()
        {
            var longPath = PopulateSpecificDir(kLongPath, new[] { "somedir/", "somedir/somefile" });

            var path = longPath.Combine("somedir");
            Assert.IsTrue(path.DirectoryExists());
            path.Delete();
            Assert.IsFalse(path.DirectoryExists());
        }

        [Test]
        public void DeleteDirectoryWhileItIsLocked()
        {
            PopulateTempDir(new[] {"somedir/", "somedir/myfile"});

            var directory = _tempPath.Combine("somedir");

            //create a file in the directory and keep an open filehandle to it
            using (new FileStream(directory.Combine("somefile").ToString(), FileMode.Create))
            {
                directory.Delete(DeleteMode.Soft);
            }
            Assert.IsFalse(directory.Combine("myfile").FileExists());
        }
        
        [Test]
        public void DeleteLargeDirectoryWhileFilesAreRemovedInParallel()
        {
	        const int numFiles = 1000;
	        PopulateTempDir(Enumerable.Range(0, numFiles).Select(i => $"somedir/{i}").Prepend("somedir/"));

	        var path = _tempPath.Combine("somedir");

	        var threadStartBarrier = new Barrier(2);
	        var deleteEverySecondFileThread = new Thread(() =>
	        {
		        threadStartBarrier.SignalAndWait();
		        foreach (var i in Enumerable.Range(0, numFiles / 2))
		        {
			        try
			        {
				        path.Combine((i * 2).ToString()).Delete();
			        }
			        catch (Exception e) when (e is InvalidOperationException or FileNotFoundException or UnauthorizedAccessException)
			        {
			        }
		        }
	        });
	        deleteEverySecondFileThread.Start();
	        threadStartBarrier.SignalAndWait();

	        try
	        {
		        Assert.That(() => path.Delete(), Throws.Nothing);
	        }
	        finally
	        {
				deleteEverySecondFileThread.Join();
	        }
	        AssertTempDir(Array.Empty<string>());
        }

        [Test]
        public void DeleteOnMultiplePaths()
        {
            PopulateTempDir(new[] { "somefile", "somedir/", "somedir/myfile", "somefile2" });

            var twoPaths = new[] { _tempPath.Combine("somefile"), _tempPath.Combine("somedir") };

            var result = twoPaths.Delete();

            CollectionAssert.AreEqual(twoPaths, result);

            AssertTempDir(new[] { "somefile2" });
        }

        [Test]
        public void DeleteOnMultipleLongPaths()
        {
            var longPath = PopulateSpecificDir(kLongPath, new[] { "somefile", "somedir/", "somedir/myfile", "somefile2" });

            var twoPaths = new[] { longPath.Combine("somefile"), longPath.Combine("somedir")};

            var result = twoPaths.Delete();

            CollectionAssert.AreEqual(twoPaths, result);

            Assert.IsTrue(longPath.Combine("somefile2").FileExists());
            Assert.IsFalse(longPath.Combine("somefile").FileExists());
            Assert.IsFalse(longPath.Combine("somedir").DirectoryExists());
        }

        [Test]
        public void DeleteContentsOfDirectory()
        {
            PopulateTempDir(new[]
            {
                "somedir/",
                "somedir/somefile"
            });

            var path = _tempPath.Combine("somedir");
            Assert.IsTrue(path.DirectoryExists());

            path.DeleteContents();

            AssertTempDir(new[] { "somedir/" });
        }

        [Test]
        [Platform(Include = "Win")]
        public void DeleteContentsOfDirectoryThrowsWhenFileCannotBeDeleted()
        {
            PopulateTempDir(new[]
            {
                "somedir/",
                "somedir/somefile"
            });

            var path = _tempPath.Combine("somedir");
            Assert.IsTrue(path.DirectoryExists());

            using (var writer = File.Open(path.Combine("somefile").ToString(), FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
            {
                Assert.Throws<IOException>(() => path.DeleteContents());
            }
        }

        public void DeleteIfExistsOnFileThatExists()
        {
            PopulateTempDir(new[] { "somefile" });

            var path = _tempPath.Combine("somefile");
            path.DeleteIfExists();

            AssertTempDir(new string[0]);
        }

        [Test]
        public void DeleteIfExistsOnFileThatDoesNotExist()
        {
            var path = _tempPath.Combine("somefile");
            path.DeleteIfExists();

            AssertTempDir(new string[0]);
        }

        [Test]
        public void DeleteIfExists_RelativePath()
        {
            var path = _tempPath.Combine("my/relativefile").WriteAllText("Content");
            Assert.IsTrue(path.FileExists(), $"File {_tempPath.ToString(SlashMode.Forward)}/my/relativeFile doesn't exist");
            Assert.IsTrue(_tempPath.Combine("my").DirectoryExists(), $"Directory {_tempPath.ToString(SlashMode.Forward)}/my doesn't exist");
            using (NPath.SetCurrentDirectory(_tempPath))
            {
                Assert.IsTrue(new NPath("my/relativefile").FileExists(), "File ./my/relativeFile doesn't exist");
                new NPath("my/relativefile").DeleteIfExists();
                Assert.IsFalse(new NPath("my/relativefile").FileExists(), "File ./my/relativeFile should not exist");
                Assert.IsTrue(new NPath("my").DirectoryExists(), "Directory ./my doesn't exist");
            }
        }

        [Test]
        [Category("CrashesNUnitOnSystemNetFramework")]
        public void DeleteIfExists_RelativePath_InsideLongPath()
        {
            var longPath = PopulateSpecificDir(kLongPath, new[] { "my/relativefile" });
            Assert.IsTrue(longPath.Combine("my/relativefile").FileExists(), $"File {_tempPath.ToString(SlashMode.Forward)}/{kLongPath}/my/relativeFile doesn't exist");
            Assert.IsTrue(longPath.Combine("my").DirectoryExists(), $"Directory {_tempPath.ToString(SlashMode.Forward)}/{kLongPath}/my doesn't exist");
            using (NPath.SetCurrentDirectory(longPath))
            {
                Assert.IsTrue(new NPath("my/relativefile").FileExists(), "File ./my/relativeFile doesn't exist");
                new NPath("my/relativefile").DeleteIfExists();
                Assert.IsFalse(new NPath("my/relativefile").FileExists(), "File ./my/relativeFile should not exist");
                Assert.IsTrue(new NPath("my").DirectoryExists(), "Directory ./my doesn't exist");
            }
        }

        [Test]
        [Category("CrashesNUnitOnSystemNetFramework")]
        public void DeleteIfExists_RelativePath_InsideLongPathSubDir()
        {
            var longPath = PopulateSpecificDir(kLongPath, new[] { "my/relativefile" });
            Assert.IsTrue(longPath.Combine("my/relativefile").FileExists(), $"File {_tempPath.ToString(SlashMode.Forward)}/{kLongPath}/my/relativeFile doesn't exist");
            Assert.IsTrue(longPath.Combine("my").DirectoryExists(), $"Directory {_tempPath.ToString(SlashMode.Forward)}/{kLongPath}/my doesn't exist");
            using (NPath.SetCurrentDirectory(longPath.Combine("my")))
            {
                Assert.IsTrue(new NPath("./relativefile").FileExists(), "File ./relativeFile doesn't exist");
                new NPath("./relativefile").DeleteIfExists();
                Assert.IsFalse(new NPath("./relativefile").FileExists(), "File ./relativeFile should not exist");
            }
        }

        [Test]
        [Category("CrashesNUnitOnSystemNetFramework")]
        public void DeleteIfExists_RelativePath_LongPath()
        {
            var longPath = PopulateSpecificDir(kLongPath, new[] { "my/relativefile" });
            Assert.IsTrue(longPath.Combine("my/relativefile").FileExists(), $"File {_tempPath.ToString(SlashMode.Forward)}/{kLongPath}/my/relativeFile doesn't exist");
            Assert.IsTrue(longPath.Combine("my").DirectoryExists(), $"Directory {_tempPath.ToString(SlashMode.Forward)}/{kLongPath}/my doesn't exist");
            using (NPath.SetCurrentDirectory(_tempPath))
            {
                Assert.IsTrue(new NPath(kLongPath).Combine("my/relativefile").FileExists(), $"File ./{kLongPath}/my/relativeFile doesn't exist");
                new NPath(kLongPath).Combine("my/relativefile").DeleteIfExists();
                Assert.IsFalse(new NPath(kLongPath).Combine("my/relativefile").FileExists(), $"File ./{kLongPath}/my/relativeFile should not exist");
                Assert.IsTrue(new NPath(kLongPath).Combine("my").DirectoryExists(), $"Directory ./{kLongPath}/my doesn't exist");
            }
        }
    }
}
