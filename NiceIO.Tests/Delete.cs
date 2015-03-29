using System;
using System.IO;
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
		public void DeleteRelativePath()
		{
			var path = new Path("mydir/myfile.txt");
			Assert.Throws<ArgumentException>(() => path.Delete());
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
	}
}