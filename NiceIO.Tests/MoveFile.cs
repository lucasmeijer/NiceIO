using System;
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

			var result = _tempPath.Combine("myfile.txt").Move(_tempPath.Combine("somedir"));
			Assert.AreEqual(_tempPath.Combine("somedir/myfile.txt"), result);

			AssertTempDir(new[] { "somedir/", "somedir/myfile.txt" });
		}

		[Test]
		public void WithRelativeSourceAndDestination()
		{
			PopulateTempDir(new[] { "somedir/", "somedir/myfile.txt" });
			using (NPath.SetCurrentDirectory(_tempPath))
				new NPath("somedir/myfile.txt").Move(new NPath("irrelevant"));
			AssertTempDir(new[] { "somedir/", "irrelevant" });
		}
	}
}