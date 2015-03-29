using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace NiceIO.Tests
{
	[TestFixture]
	public class Delete : TestWithTempDir
	{
		[Test]
		public void DeleteFile()
		{
			PopulateTempDir(new [] { "somefile"});
		
			var path = _tempPath.Combine("somefile");
			Assert.IsTrue(path.FileExists());
			path.Delete();
			Assert.IsFalse(path.FileExists());
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

			Assert.IsFalse(path.DirectoryExists());
		}
	}
}
