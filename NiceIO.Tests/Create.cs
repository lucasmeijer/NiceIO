using System;
using System.Collections;
using System.IO;
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
		public void FileInLongPath_SimpleCase()
		{
			var longPath = _tempPath.Combine(kLongPath).CreateDirectory();
			Assert.IsTrue(longPath.Combine("myfile").CreateFile().FileExists());
		}
		
		[Test]
		[TestCase(1, false)]
		[TestCase(10, false)]
		[TestCase(200, false)]
		[TestCase(255, false)]
		[TestCase(256, true)] // Limit per path segment is still 255 characters
		public void File_WithLongStringArgument(int length, bool willThrowException)
		{
			var longPath = new String('u', length);

			if (willThrowException)
				Assert.Throws<PathTooLongException>(() => _tempPath.CreateFile(longPath + ".txt"));
			else
			{
				Assert.IsTrue(_tempPath.CreateFile(longPath).FileExists());
				AssertTempDir(new[] { $"{longPath}" });
			}
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
		public void File_InNonExistingLongPathDirectory()
		{
			Assert.IsFalse(_tempPath.Combine(kLongPath).Exists());
			_tempPath.Combine(kLongPath + "not_yet_existing_dir/myotherdir/myfile").CreateFile().FileExists();

			AssertSpecificDir(_tempPath.Combine(kLongPath), new[]
			{
				"not_yet_existing_dir/",
				"not_yet_existing_dir/myotherdir/",
				"not_yet_existing_dir/myotherdir/myfile"
			});
		}

		[Test]
		public void File_WithArgument()
		{
			Assert.IsTrue(_tempPath.CreateFile("myfile").FileExists());
			AssertTempDir(new[] { "myfile" });
		}

		[Test]
		public void File_WithArgumentInLongPath()
		{
			var longPath = _tempPath.Combine(kLongPath).CreateDirectory();
			Assert.IsTrue(longPath.CreateFile("myfile").FileExists());
		}

		[Test]
		public void File_WithArgumentInDirectory()
		{
			Assert.IsTrue(_tempPath.CreateFile("mydir/myfile").FileExists());
			AssertTempDir(new[] { "mydir/", "mydir/myfile" });
		}

		[Test]
		public void File_WithArgumentInLongPathDirectory()
		{
			Assert.IsTrue(_tempPath.CreateFile(kLongPath + "mydir/myfile").FileExists());
			AssertSpecificDir(_tempPath.Combine(kLongPath), new [] { "mydir/", "mydir/myfile" });
		}

		[Test]
		public void Directory_SimpleCase()
		{
			Assert.IsTrue(_tempPath.Combine("mydir").CreateDirectory().DirectoryExists());
			AssertTempDir(new[] { "mydir/" });
		}

		[Test]
		public void DirectoryInLongPath_SimpleCase()
		{
			var longPath = _tempPath.Combine(kLongPath).CreateDirectory();
			Assert.IsTrue(longPath.Combine("mydir").CreateDirectory().DirectoryExists());
		}

		[Test]
		public void Directory_InNonExistingDirectory()
		{
			_tempPath.Combine("not_yet_existing_dir/myotherdir/mydir").CreateDirectory();

			AssertTempDir(new[]
			{
				"not_yet_existing_dir/",
				"not_yet_existing_dir/myotherdir/",
				"not_yet_existing_dir/myotherdir/mydir/"
			});
		}

		[Test]
		public void Directory_InNonExistingLongPathDirectory()
		{
			_tempPath.Combine(kLongPath + "not_yet_existing_dir/myotherdir/mydir").CreateDirectory();
			AssertSpecificDir(_tempPath.Combine(kLongPath), new[]
			{
				"not_yet_existing_dir/",
				"not_yet_existing_dir/myotherdir/",
				"not_yet_existing_dir/myotherdir/mydir/"
			});
		}

		[Test]
		public void Directory_WithStringArgument()
		{
			Assert.IsTrue(_tempPath.CreateDirectory("mysubdir").DirectoryExists());
			AssertTempDir(new[] { "mysubdir/" });
		}

		[Test]
		public void Directory_WithStringArgumentLongPath()
		{
			_tempPath.Combine(kLongPath).CreateDirectory();
			Assert.IsTrue(_tempPath.CreateDirectory(kLongPath + "mysubdir").DirectoryExists());
		}

		[Test]
		public void Directory_WithStringArgumentInDirectory()
		{
			_tempPath.Combine(kLongPath).CreateDirectory();
			Assert.IsTrue(_tempPath.CreateDirectory(kLongPath + "mysubdir/mysubdir2").DirectoryExists());
			Assert.IsTrue(_tempPath.Combine(kLongPath + "mysubdir").DirectoryExists());
		}

		[Test]
		public void Directory_WithPathArgument()
		{
			Assert.IsTrue(_tempPath.CreateDirectory(new NPath("mysubdir")).DirectoryExists());
			AssertTempDir(new[] { "mysubdir/" });
		}

		[Test]
		public void Directory_WithPathArgumentLongPath()
		{
			_tempPath.Combine(kLongPath).CreateDirectory();
			Assert.IsTrue(_tempPath.CreateDirectory(new NPath(kLongPath + "mysubdir")).DirectoryExists());
		}

		[Test]
		public void Directory_WithPathArgumentInDirectory()
		{
			Assert.IsTrue(_tempPath.CreateDirectory(new NPath("mysubdir/mysubdir2")).DirectoryExists());
			AssertTempDir(new[] { "mysubdir/", "mysubdir/mysubdir2/" });
		}

		[Test]
		public void Directory_WithPathArgumentInDirectoryLongPath()
		{
			_tempPath.Combine(kLongPath).CreateDirectory();
			Assert.IsTrue(_tempPath.CreateDirectory(new NPath(kLongPath + "mysubdir/mysubdir2")).DirectoryExists());
			Assert.IsTrue(_tempPath.Combine(kLongPath + "mysubdir").DirectoryExists());
		}

		[Test]
		[TestCase(1, false)]
		[TestCase(10, false)]
		[TestCase(200, false)]
		[TestCase(255, false)]
		[TestCase(256, true)] // Limit per path segment is still 255 characters
		public void Directory_WithLongStringArgument(int length, bool willThrowException)
		{
			var longPath = new String('u', length);

			if (willThrowException)
				Assert.Throws<PathTooLongException>(() => _tempPath.CreateDirectory(longPath));
			else
			{
				Assert.IsTrue(_tempPath.CreateDirectory(longPath).DirectoryExists());
				AssertTempDir(new[] { $"{longPath}/" });
			}
		}

		protected static IEnumerable Directory_CreateWithDifferentPathLength_TestCases()
		{
			var path = "somesubdirectorywithalongname/somesubdirectorywithalongname/somesubdirectorywithalongname";
			while (path.Length < 500)
			{
				yield return new TestCaseData(path) { TestName = $"Directory_Create (with {path.Length} characters path)" };
				path += "/somesubdirectory";
			}
		}

		[Test]
		[TestCaseSource(nameof(Directory_CreateWithDifferentPathLength_TestCases))]
		public void Directory_CreateWithDifferentPathLength(string path)
		{
			Assert.IsTrue(_tempPath.Combine(path).CreateDirectory().DirectoryExists());
		}

		protected static IEnumerable DirectoryAndFiles_CreateWithDifferentPathLength_TestCases()
		{
			var path = "somesubdirectorywithalongname/somesubdirectorywithalongname/somesubdirectorywithalongname";
			while (path.Length < 500)
			{
				yield return new TestCaseData(path, path.Length) { TestName = $"DirectoryAndFiles (with {path.Length} characters path)" };
				path += "/somesubdirectory";
			}
		}

		[Test]
		[TestCaseSource(nameof(DirectoryAndFiles_CreateWithDifferentPathLength_TestCases))]
		public void DirectoryAndFiles_CreateWithDifferentPathLength(string path, int length)
		{
			var p = _tempPath.Combine(path);
			Assert.IsTrue(p.CreateDirectory().DirectoryExists());

			var name = "file";
			do
			{
				Assert.IsTrue(p.Combine($"{name}.txt").WriteAllText($"File {name} in path length {length}").FileExists());
				name += "_withalongname";
			}
			while (name.Length < 251);
		}

		protected static IEnumerable File_CreateWithDifferentPathLength_TestCases()
		{
			var path = "somesubdirectorywithalongname/somesubdirectorywithalongname/somesubdirectorywithalongname/";
			while (path.Length < 500)
			{
				var p = path + "file.txt";
				yield return new TestCaseData(p, p.Length) { TestName = $"File_Create (with {p.Length} characters path)" }; ;
				path += "somesubdirectory/";
			}
		}

		[Test]
		[TestCaseSource(nameof(File_CreateWithDifferentPathLength_TestCases))]
		public void File_CreateWithDifferentPathLength(string path, int length)
		{
			Assert.IsTrue(_tempPath.Combine(path).WriteAllText($"Path length {length}").FileExists());
		}
	}
}