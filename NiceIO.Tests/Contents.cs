using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace NiceIO.Tests
{
	[TestFixture]
	public class Contents : TestWithTempDir
	{
		[Test]
		public void Files()
		{
			var files = new[] {"myfile.txt", "myfile2.exe"};
			PopulateTempDir(files);

			CollectionAssert.AreEquivalent(files, _tempPath.Files().Select(p => p.FileName));
		}

		[Test]
		public void FilesWithWildcard()
		{
			var files = new[] { "myfile.txt", "myfile2.exe" };
			PopulateTempDir(files);

			var enumerable = _tempPath.Files("*.txt");
			Assert.AreEqual(_tempPath.Combine("myfile.txt"), enumerable.Single());
		}

		[Test]
		public void FilesRecursive()
		{
			PopulateTempDir(new[] {"myfile.txt", "mydir/", "mydir/myfile2.txt"});

			CollectionAssert.AreEquivalent(new[] {"myfile.txt", "myfile2.txt"}, _tempPath.Files(recurse:true).Select(p => p.FileName));
		}

		[Test]
		public void ContentsWithWildcard()
		{
			PopulateTempDir(new[] { "myfile.txt", "mydir/", "mydir2/", "otherdir/", "mydir/mysubdir/","otherfile" });

			CollectionAssert.AreEquivalent(new[] { "myfile.txt", "mydir", "mydir2" }, _tempPath.Contents("my*").Select(p => p.FileName));

		}

		[Test]
		public void ContentsRecursive()
		{
			PopulateTempDir(new[] {"myfile.txt", "mydir/", "mydir/myfile2.txt", "mydir/mysubdir/", "mydir/mysubdir/myfile3"});

			CollectionAssert.AreEquivalent(new[] { "myfile.txt", "myfile2.txt", "mydir", "mysubdir", "myfile3" }, _tempPath.Contents(recurse: true).Select(p => p.FileName));
		}

		[Test]
		public void DirectoriesRecursive()
		{
			PopulateTempDir(new[] { "myfile.txt", "mydir/", "mydir/myfile2.txt", "mydir/mysubdir/", "mydir/mysubdir/myfile3" });

			CollectionAssert.AreEquivalent(new[] { "mydir", "mysubdir" }, _tempPath.Directories(recurse: true).Select(p => p.FileName));
		}

		[Test]
		public void ContentsOfRelativeCurrentDirectory()
		{
			PopulateTempDir(new[] { "myfile.txt", "sourceDir\\", "sourceDir\\source.cpp" });

			var originalCurrentDirectory = Directory.GetCurrentDirectory();
			Directory.SetCurrentDirectory(_tempPath.ToString());

			var currentDirRelative = new NPath("myfile.txt").Parent;
			CollectionAssert.AreEquivalent(new[] { "myfile.txt", "source.cpp" }, currentDirRelative.Files(recurse: true).Select(p => p.FileName));

			Directory.SetCurrentDirectory(originalCurrentDirectory);
		}

		[Test]
		public void DepthWindowsPath()
		{
			Assert.That(new NPath("C:\\my\\path\\is\\kind\\of\\long").Depth, Is.EqualTo(6));
		}

		[Test]
		public void DepthLinuxPath()
		{
			Assert.That(new NPath("/my/path/is/kind/of/long").Depth, Is.EqualTo(6));
		}

		[Test]
		public void DepthRelativePath()
		{
			Assert.That(new NPath("my/path/is/kind/of/long").Depth, Is.EqualTo(6));
		}
	}
}