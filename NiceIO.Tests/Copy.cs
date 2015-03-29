using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace NiceIO.Tests
{
	[TestFixture]
	public class Copy : TestWithTempDir
	{
		[Test]
		public void FileToSameDirectory()
		{
			PopulateTempDir(new [] { "myfile.txt"});

			var path = _tempPath.Combine("myfile.txt");
			var dest = _tempPath.Combine("mycopy.txt");
			path.Copy(dest);

			Assert.IsTrue(dest.FileExists());
		}

		[Test]
		public void IntoNonExistingDirectory()
		{
			PopulateTempDir(new[] { "myfile.txt" });

			var path = _tempPath.Combine("myfile.txt");
			var dest = _tempPath.Combine("mydir/mycopy.txt");
			path.Copy(dest);

			Assert.IsTrue(dest.FileExists());
		}

		[Test]
		public void OnExistingDirectory()
		{
			PopulateTempDir(new[] { "somedir/", "myfile.txt" });

			Assert.Throws<System.IO.IOException>(() => _tempPath.Combine("myfile.txt").Copy(_tempPath.Combine("somedir")));
		}

		[Test]
		public void WithRelativeSource()
		{
			Assert.Throws<InvalidOperationException>(()=>new Path("somedir/somefile").Copy(new Path("irrelevant")));
		}

		[Test]
		public void WithRelativeDestination()
		{
			PopulateTempDir(new[] { "myfile.txt" });
			Assert.Throws<InvalidOperationException>(() => _tempPath.Combine("file.txt").Copy(new Path("irrelevant")));
		}

	}
}
