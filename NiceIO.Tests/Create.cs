using System;
using NUnit.Framework;

namespace NiceIO.Tests
{
	[TestFixture]
	public class Create : TestWithTempDir
	{
		[Test]
		public void File_SimpleCase()
		{
			Assert.IsTrue(_tempPath.Combine("myfile").CreateFile().FileExists());

			AssertTempDir(new [] { "myfile"});
		}

		[Test]
		public void File_InNonExistingDirectory()
		{
			_tempPath.Combine("not_yet_existing_dir/myotherdir/myfile").CreateFile().FileExists();

			AssertTempDir(new[]
			{
				"not_yet_existing_dir/",
				"not_yet_existing_dir/myotherdir/",
				"not_yet_existing_dir/myotherdir/myfile"
			});
		}

		[Test]
		public void File_OnRelativePath()
		{
			Assert.Throws<ArgumentException>(() => new NPath("mydir/myfile.txt").CreateFile());
		}

		[Test]
		public void File_WithArgument()
		{
			Assert.IsTrue(_tempPath.CreateFile("myfile").FileExists());
			AssertTempDir(new[] { "myfile" });
		}

		[Test]
		public void File_WithArgumentInDirectory()
		{
			Assert.IsTrue(_tempPath.CreateFile("mydir/myfile").FileExists());
			AssertTempDir(new[] { "mydir/", "mydir/myfile" });
		}

		[Test]
		public void Directory_SimpleCase()
		{
			Assert.IsTrue(_tempPath.Combine("mydir").CreateDirectory().DirectoryExists());
			AssertTempDir(new[] { "mydir/" });
		}

		[Test]
		public void Directory_InNonExistingDirectory()
		{
			_tempPath.Combine("not_yet_existing_dir/myotherdir/mydir").CreateDirectory().DirectoryExists();
			
			AssertTempDir(new[]
			{
				"not_yet_existing_dir/",
				"not_yet_existing_dir/myotherdir/",
				"not_yet_existing_dir/myotherdir/mydir/"
			});
		}

		[Test]
		public void Directory_OnRelativePath()
		{
			Assert.Throws<ArgumentException>(() => new NPath("mydir/mydir2").CreateDirectory());
		}

		[Test]
		public void Directory_WithStringArgument()
		{
			Assert.IsTrue(_tempPath.CreateDirectory("mysubdir").DirectoryExists());
			AssertTempDir(new[] { "mysubdir/" });
		}

		[Test]
		public void Directory_WithStringArgumentInDirectory()
		{
			Assert.IsTrue(_tempPath.CreateDirectory("mysubdir/mysubdir2").DirectoryExists());
			AssertTempDir(new[] { "mysubdir/", "mysubdir/mysubdir2/" });
		}

		[Test]
		public void Directory_WithPathArgument()
		{
			Assert.IsTrue(_tempPath.CreateDirectory(new NPath("mysubdir")).DirectoryExists());
			AssertTempDir(new[] { "mysubdir/" });
		}

		[Test]
		public void Directory_WithPathArgumentInDirectory()
		{
			Assert.IsTrue(_tempPath.CreateDirectory(new NPath("mysubdir/mysubdir2")).DirectoryExists());
			AssertTempDir(new[] { "mysubdir/", "mysubdir/mysubdir2/" });
		}
	}
}