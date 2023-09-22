using System;
using NUnit.Framework;

namespace NiceIO.Tests
{
	[TestFixture]
	public class CopyAndMoveFiles : TestWithTempDir
	{
		[Test]
		public void CopyTwoFiles()
		{
			PopulateTempDir(new[] { "file1", "file2" });
			var result = _tempPath.Files().Copy(_tempPath.Combine("somedir"));

			CollectionAssert.AreEquivalent(new[] { _tempPath.Combine("somedir/file1"), _tempPath.Combine("somedir/file2") }, result);

			AssertTempDir(new[] { "file1", "file2", "somedir/", "somedir/file1", "somedir/file2" });
		}

		[Test]
		public void CopyTwoFilesIntoLongDirectory()
		{
			PopulateTempDir(new[] { "file1", "file2" });
			var longPath = _tempPath.Combine(kLongPath).CreateDirectory();
			var result = _tempPath.Files().Copy(longPath);

			CollectionAssert.AreEquivalent(new[] { longPath.Combine("file1"), longPath.Combine("file2") }, result);
			
			AssertSpecificDir(longPath, new [] { "file1", "file2" }, false);
			AssertSpecificDir(_tempPath, new [] { "file1", "file2", kLongPathPart }, false);
		}

		[Test]
		public void CopyTwoFilesFromLongDirectory()
		{
			var longPath = PopulateSpecificDir(kLongPath, new[] { "file1", "file2" });
			var result = longPath.Files().Copy(_tempPath);

			CollectionAssert.AreEquivalent(new[] { _tempPath.Combine("file1"), _tempPath.Combine("file2") }, result);
			AssertSpecificDir(longPath, new [] { "file1", "file2" }, false);
			AssertSpecificDir(_tempPath, new [] { "file1", "file2", kLongPathPart }, false);
		}

		[Test]
		public void CopyFilesInFolders()
		{
			PopulateTempDir(new[] { "file1", "file2" ,"dir/", "dir/file3"});
			var result = _tempPath.Files(recurse:true).Copy(_tempPath.Combine("somedir"));

			CollectionAssert.AreEquivalent(new[] { _tempPath.Combine("somedir/file1"), _tempPath.Combine("somedir/file2") , _tempPath.Combine("somedir/file3")}, result);

			AssertTempDir(new[] { "file1", "file2", "dir/", "dir/file3", "somedir/", "somedir/file1", "somedir/file2", "somedir/file3" });
		}

		[Test]
		public void CopyIntoRelativePath()
		{
			Assert.Throws<ArgumentException>(() => _tempPath.Files().Copy("relative"));
		}

		[Test]
		public void MoveTwoFiles()
		{
			PopulateTempDir(new[] { "file1", "file2" });
		
			var result = _tempPath.Files().Move(_tempPath.Combine("somedir"));
			CollectionAssert.AreEquivalent(new[] { _tempPath.Combine("somedir/file1"), _tempPath.Combine("somedir/file2") }, result);

			AssertTempDir(new[] {"somedir/", "somedir/file1", "somedir/file2" });
		}

		[Test]
		public void MoveTwoFilesIntoLongDirectory()
		{
			PopulateTempDir(new[] { "file1", "file2" });
			var longPath = _tempPath.Combine(kLongPath).CreateDirectory();
			var result = _tempPath.Files().Move(longPath);

			CollectionAssert.AreEquivalent(new[] { longPath.Combine("file1"), longPath.Combine("file2") }, result);
			AssertSpecificDir(longPath, new [] { "file1", "file2" }, false, true);
			AssertSpecificDir(_tempPath, new string[0], false, true);
		}

		[Test]
		public void MoveTwoFilesFromLongDirectory()
		{
			var longPath = PopulateSpecificDir(kLongPath, new[] { "file1", "file2" });
			var result = longPath.Files().Move(_tempPath);

			CollectionAssert.AreEquivalent(new[] { _tempPath.Combine("file1"), _tempPath.Combine("file2") }, result);
			AssertSpecificDir(longPath, new string[0], false, true);
			AssertSpecificDir(_tempPath, new [] { "file1", "file2" }, false, true);
		}

		[Test]
		public void MoveFilesInFolders()
		{
			PopulateTempDir(new[] { "file1", "file2", "dir/", "dir/file3" });
			var result = _tempPath.Files(recurse: true).Move(_tempPath.Combine("somedir"));

			CollectionAssert.AreEquivalent(new[] { _tempPath.Combine("somedir/file1"), _tempPath.Combine("somedir/file2"), _tempPath.Combine("somedir/file3") }, result);

			AssertTempDir(new[] { "dir/", "somedir/", "somedir/file1", "somedir/file2", "somedir/file3" });
		}

		[Test]
		public void MoveIntoRelativePath()
		{
			Assert.Throws<ArgumentException>(() => _tempPath.Files().Move("relative"));
		}

	}
}
