using System;
using System.IO;
using NUnit.Framework;

namespace NiceIO.Tests
{
	[TestFixture]
	public class MoveFile : TestWithTempDir
	{
		[Test]
		public void FileToSameDirectory()
		{
			PopulateTempDir(new[] { "myfile.txt" });

			var path = _tempPath.Combine("myfile.txt");
			var dest = _tempPath.Combine("target.txt");
			var returnValue = path.Move(dest);
			Assert.AreEqual(dest,returnValue);

			AssertTempDir(new[] { "target.txt" });
		}
		
		[Test]
		public void IntoNonExistingDirectory()
		{
			PopulateTempDir(new[] { "myfile.txt" });

			var path = _tempPath.Combine("myfile.txt");
			var dest = _tempPath.Combine("mydir/mytarget.txt");
			path.Move(dest);

			AssertTempDir(new[] { "mydir/", "mydir/mytarget.txt" });
		}

		[Test]
		public void NonExistingFile()
		{
			Assert.Throws<ArgumentException>(()=>_tempPath.Combine("nonexisting.txt").Move(_tempPath.Combine("target")));
		}

		[Test]
		public void OnExistingDirectory()
		{
			PopulateTempDir(new[] { "somedir/", "myfile.txt" });

			Assert.Throws<IOException>(() => _tempPath.Combine("myfile.txt").Move(_tempPath.Combine("somedir")));
		}

		[Test]
		public void WithRelativeSource()
		{
			Assert.Throws<ArgumentException>(() => new Path("somedir/somefile").Move(new Path("irrelevant")));
		}
		
		[Test]
		public void WithRelativeDestination()
		{
			PopulateTempDir(new[] { "myfile.txt" });
			Assert.Throws<ArgumentException>(() => _tempPath.Combine("file.txt").Move(new Path("irrelevant")));
		}
	}
}