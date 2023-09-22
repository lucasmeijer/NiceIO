using System;
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
		public void FileToSameDirectoryLongPath()
		{
			var longPath = PopulateSpecificDir(kLongPath, new[] { "myfile.txt" });

			var path = longPath.Combine("myfile.txt");
			var dest = longPath.Combine("mycopy.txt");

			var result = path.Copy(dest);
			Assert.AreEqual(dest, result);

			AssertSpecificDir(longPath, new[] {"myfile.txt", "mycopy.txt"});
		}

		[Test]
		public void IntoNonExistingDirectory()
		{
			PopulateTempDir(new[] { "myfile.txt" });

			var path = _tempPath.Combine("myfile.txt");
			var dest = _tempPath.Combine("mydir/mycopy.txt");
			path.Copy(dest);

			AssertTempDir(new[] { "myfile.txt", "mydir/", "mydir/mycopy.txt" });
		}

		[Test]
		public void IntoNonExistingDirectoryLongPath()
		{
			var longPath = PopulateSpecificDir(kLongPath, new[] { "myfile.txt" });

			var path = longPath.Combine("myfile.txt");
			var dest = longPath.Combine("mydir/mycopy.txt");
			path.Copy(dest);

			AssertSpecificDir(longPath, new[] {"myfile.txt", "mydir/", "mydir/mycopy.txt"});
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
		public void OnExistingDirectory_LongPath()
		{
			var rootPath = _tempPath.ToString(SlashMode.Native);
			var longPath = new string('_', 247 - rootPath.Length - 2);
			PopulateTempDir(new[] { longPath + "/", "myfile_with_a_super_long_name.txt" });

			var result = _tempPath.Combine("myfile_with_a_super_long_name.txt").Copy(_tempPath.Combine(longPath));
			Assert.AreEqual(_tempPath.Combine(longPath  + "/myfile_with_a_super_long_name.txt"), result);

			AssertTempDir(new[] { "myfile_with_a_super_long_name.txt", longPath + "/", longPath + "/myfile_with_a_super_long_name.txt" });
		}

		[Test]
		public void NonExistingFile()
		{
			Assert.Throws<ArgumentException>(() => _tempPath.Combine("nonexisting.txt").Copy(_tempPath.Combine("target")));
		}

		[Test]
		public void WithRelativeSourceAndDestination()
		{
			PopulateTempDir(new[] { "somedir/", "somedir/myfile.txt" });
			using (NPath.SetCurrentDirectory(_tempPath))
				new NPath("somedir/myfile.txt").Copy(new NPath("irrelevant"));
			AssertTempDir(new[] { "somedir/", "somedir/myfile.txt", "irrelevant" });
		}
	}
}