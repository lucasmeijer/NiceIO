using System;
using System.IO;
using NUnit.Framework;

namespace NiceIO.Tests
{
	[TestFixture]
	public class CopyFile : TestWithTempDir
	{
		[Test]
		public void FileToSameDirectory()
		{
			PopulateTempDir(new[] {"myfile.txt"});

			var path = _tempPath.Combine("myfile.txt");
			var dest = _tempPath.Combine("mycopy.txt");
			
			var result = path.Copy(dest);
			Assert.AreEqual(dest, result);

			AssertTempDir(new[] {"myfile.txt", "mycopy.txt"});
		}

		[Test]
		public void IntoNonExistingDirectory()
		{
			PopulateTempDir(new[] {"myfile.txt"});

			var path = _tempPath.Combine("myfile.txt");
			var dest = _tempPath.Combine("mydir/mycopy.txt");
			path.Copy(dest);

			AssertTempDir(new[] {"myfile.txt", "mydir/", "mydir/mycopy.txt"});
		}

		[Test]
		public void OnExistingDirectory()
		{
			PopulateTempDir(new[] {"somedir/", "myfile.txt"});

			Assert.Throws<IOException>(() => _tempPath.Combine("myfile.txt").Copy(_tempPath.Combine("somedir")));
		}

		[Test]
		public void NonExistingFile()
		{
			Assert.Throws<ArgumentException>(() => _tempPath.Combine("nonexisting.txt").Copy(_tempPath.Combine("target")));
		}

		[Test]
		public void WithRelativeSource()
		{
			Assert.Throws<ArgumentException>(() => new Path("somedir/somefile").Copy(new Path("irrelevant")));
		}

		[Test]
		public void WithRelativeDestination()
		{
			PopulateTempDir(new[] {"myfile.txt"});
			Assert.Throws<ArgumentException>(() => _tempPath.Combine("file.txt").Copy(new Path("irrelevant")));
		}
	}
}