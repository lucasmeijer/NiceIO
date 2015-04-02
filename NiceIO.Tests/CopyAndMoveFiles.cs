using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace NiceIO.Tests
{
	[TestFixture]
	public class CopyAndMoveFiles : TestWithTempDir
	{
		[Test]
		public void CopyTwoFiles()
		{
			PopulateTempDir(new [] { "file1","file2"});
			var result = _tempPath.Files().Copy(_tempPath.Combine("somedir"));

			CollectionAssert.AreEqual(new[] { _tempPath.Combine("somedir/file1"), _tempPath.Combine("somedir/file2")}, result);

			AssertTempDir(new [] { "file1","file2","somedir/","somedir/file1","somedir/file2"});
		}

		[Test]
		public void CopyFilesInFolders()
		{
			PopulateTempDir(new[] { "file1", "file2" ,"dir/", "dir/file3"});
			var result = _tempPath.Files(recurse:true).Copy(_tempPath.Combine("somedir"));

			CollectionAssert.AreEqual(new[] { _tempPath.Combine("somedir/file1"), _tempPath.Combine("somedir/file2") , _tempPath.Combine("somedir/file3")}, result);

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
			CollectionAssert.AreEqual(new[] { _tempPath.Combine("somedir/file1"), _tempPath.Combine("somedir/file2") }, result);

			AssertTempDir(new[] {"somedir/", "somedir/file1", "somedir/file2" });
		}

		[Test]
		public void MoveFilesInFolders()
		{
			PopulateTempDir(new[] { "file1", "file2", "dir/", "dir/file3" });
			var result = _tempPath.Files(recurse: true).Move(_tempPath.Combine("somedir"));

			CollectionAssert.AreEqual(new[] { _tempPath.Combine("somedir/file1"), _tempPath.Combine("somedir/file2"), _tempPath.Combine("somedir/file3") }, result);

			AssertTempDir(new[] { "dir/", "somedir/", "somedir/file1", "somedir/file2", "somedir/file3" });
		}

		[Test]
		public void MoveIntoRelativePath()
		{
			Assert.Throws<ArgumentException>(() => _tempPath.Files().Move("relative"));
		}

	}
}
