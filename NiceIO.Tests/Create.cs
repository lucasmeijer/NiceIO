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
		}

		[Test]
		public void File_InNonExistingDirectory()
		{
			_tempPath.Combine("not_yet_existing_dir/myotherdir/myfile").CreateFile().FileExists();
		}

		[Test]
		public void File_OnRelativePath()
		{
			Assert.Throws<InvalidOperationException>(() => new Path("mydir/myfile.txt").CreateFile());
		}

		[Test]
		public void Directory_SimpleCase()
		{
			Assert.IsTrue(_tempPath.Combine("mydir").CreateDirectory().FileExists());
		}

		[Test]
		public void Directory_InNonExistingDirectory()
		{
			_tempPath.Combine("not_yet_existing_dir/myotherdir/mydir").CreateDirectory().DirectoryExists();
		}

		[Test]
		public void Directory_OnRelativePath()
		{
			Assert.Throws<InvalidOperationException>(() => new Path("mydir/mydir2").CreateDirectory());
		}
	}
}