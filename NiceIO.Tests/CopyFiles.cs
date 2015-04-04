using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using NUnit.Framework;

namespace NiceIO.Tests
{
	[TestFixture]
	public class CopyFiles : TestWithTempDir
	{
		public void Recurse()
		{
			PopulateTempDir(new [] { "mydir/","mydir/file","mydir/otherfile","mydir/mysubdir/file2"});

			var result = _tempPath.Combine("mydir").CopyFiles(_tempPath.Combine("newdir"), recurse:true);

			CollectionAssert.AreEquivalent(new[]
			{
				_tempPath.Combine("newdir/file"),
				_tempPath.Combine("newdir/otherfile"),
				_tempPath.Combine("newdir/mysubdir/file2")
			},result);
		
			AssertTempDir(new[] { "mydir/", "mydir/file", "mydir/otherfile", "mydir/mysubdir/file2", "newdir/","newdir/file","newdir/otherfile","newdir/mysubdir/file2"});
		}

		[Test]
		public void FileFilter()
		{
			var original = new[] { "mydir/", "mydir/file", "mydir/otherfile", "mydir/mysubdir/","mydir/mysubdir/file2" };
			PopulateTempDir(original);

			var result = _tempPath.Combine("mydir").CopyFiles(_tempPath.Combine("newdir"), recurse: true, fileFilter:p=>p.FileName.StartsWith("file"));

			CollectionAssert.AreEquivalent(new[]
			{
				_tempPath.Combine("newdir/file"),
				_tempPath.Combine("newdir/mysubdir/file2")
			}, result);

			AssertTempDir(original.Concat(new[]{"newdir/", "newdir/file", "newdir/mysubdir/file2", "newdir/mysubdir/" }));
		}
	}
}
