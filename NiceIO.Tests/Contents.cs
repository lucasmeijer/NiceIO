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
		public void FilesRecursive()
		{
			PopulateTempDir(new[] {"myfile.txt", "mydir/", "mydir/myfile2.txt"});

			CollectionAssert.AreEquivalent(new[] {"myfile.txt", "myfile2.txt"}, _tempPath.Files(SearchOption.AllDirectories).Select(p => p.FileName));
		}

		[Test]
		public void ContentsRecursive()
		{
			PopulateTempDir(new[] {"myfile.txt", "mydir/", "mydir/myfile2.txt", "mydir/mysubdir/", "mydir/mysubdir/myfile3"});

			CollectionAssert.AreEquivalent(new[] {"myfile.txt", "myfile2.txt", "mydir", "mysubdir", "myfile3"}, _tempPath.Contents(SearchOption.AllDirectories).Select(p => p.FileName));
		}

		[Test]
		public void DirectoriesRecursive()
		{
			PopulateTempDir(new[] {"myfile.txt", "mydir/", "mydir/myfile2.txt", "mydir/mysubdir/", "mydir/mysubdir/myfile3"});

			CollectionAssert.AreEquivalent(new[] {"mydir", "mysubdir"}, _tempPath.Directories(SearchOption.AllDirectories).Select(p => p.FileName));
		}
	}
}