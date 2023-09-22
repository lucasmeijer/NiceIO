using System;
using System.Linq;
using NUnit.Framework;

namespace NiceIO.Tests
{
	[TestFixture]
	public class CopyFiles : TestWithTempDir
	{
		[Test]
		public void Recurse()
		{
			PopulateTempDir(new[] { "mydir/", "mydir/file", "mydir/otherfile", "mydir/mysubdir/", "mydir/mysubdir/file2" });

			var result = _tempPath.Combine("mydir").CopyFiles(_tempPath.Combine("newdir"), recurse: true);

			CollectionAssert.AreEquivalent(new[]
			{
				_tempPath.Combine("newdir/file"),
				_tempPath.Combine("newdir/otherfile"),
				_tempPath.Combine("newdir/mysubdir/file2")
			}, result);

			AssertTempDir(new[] {
				"mydir/", "mydir/file", "mydir/otherfile", "mydir/mysubdir/", "mydir/mysubdir/file2",
				"newdir/","newdir/file","newdir/otherfile", "newdir/mysubdir/", "newdir/mysubdir/file2"});
		}

		[Test]
		public void RecurseInDirectoryLongPath()
		{
			PopulateTempDir(new[] { "mydir/", "mydir/file", "mydir/otherfile", "mydir/mysubdir/", "mydir/mysubdir/file2" });

			var longPath = _tempPath.Combine(kLongPath).CreateDirectory();
			var result = _tempPath.Combine("mydir").CopyFiles(longPath.Combine("newdir"), recurse: true);

			CollectionAssert.AreEquivalent(new[]
			{
				longPath.Combine("newdir/file"),
				longPath.Combine("newdir/otherfile"),
				longPath.Combine("newdir/mysubdir/file2")
			}, result);

			AssertSpecificDir(longPath, new[] { "newdir/","newdir/file","newdir/otherfile", "newdir/mysubdir/", "newdir/mysubdir/file2"});
		}

		[Test]
		public void FileFilter()
		{
			var original = new[] { "mydir/", "mydir/file", "mydir/otherfile", "mydir/mysubdir/", "mydir/mysubdir/file2" };
			PopulateTempDir(original);

			var result = _tempPath.Combine("mydir").CopyFiles(_tempPath.Combine("newdir"), recurse: true, fileFilter: p => p.FileName.StartsWith("file", StringComparison.Ordinal));

			CollectionAssert.AreEquivalent(new[]
			{
				_tempPath.Combine("newdir/file"),
				_tempPath.Combine("newdir/mysubdir/file2")
			}, result);

			AssertTempDir(original.Concat(new[] { "newdir/", "newdir/file", "newdir/mysubdir/", "newdir/mysubdir/file2" }));
		}

		[Test]
		public void FileFilterInDirectoryLongPath()
		{
			var original = new[] { "mydir/", "mydir/file", "mydir/otherfile", "mydir/mysubdir/","mydir/mysubdir/file2" };
			PopulateTempDir(original);

			var longPath = _tempPath.Combine(kLongPath).CreateDirectory();
			var result = _tempPath.Combine("mydir").CopyFiles(longPath.Combine("newdir"), recurse: true, fileFilter:p=>p.FileName.StartsWith("file", StringComparison.Ordinal));

			CollectionAssert.AreEquivalent(new[]
			{
				longPath.Combine("newdir/file"),
				longPath.Combine("newdir/mysubdir/file2")
			}, result);

			AssertSpecificDir(longPath, new[] { "newdir/", "newdir/file", "newdir/mysubdir/", "newdir/mysubdir/file2" });
		}
	}
}
