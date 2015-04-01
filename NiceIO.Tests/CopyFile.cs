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

			//turns out .net throws an IOException here, and mono an ArgumentException. let's deal with that another day.
			var result = _tempPath.Combine("myfile.txt").Copy(_tempPath.Combine("somedir"));
			Assert.AreEqual(_tempPath.Combine("somedir/myfile.txt"), result);

			AssertTempDir(new[] { "myfile.txt", "somedir/", "somedir/myfile.txt" });
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
			var result = _tempPath.Combine("myfile.txt").Copy(new Path("myfile2.txt"));
			Assert.AreEqual(_tempPath.Combine("myfile2.txt"),result);
			AssertTempDir(new []
				{
					"myfile.txt",
					"myfile2.txt"
				});
		}
	}
}