using NUnit.Framework;

namespace NiceIO.Tests
{
	[TestFixture]
	public class Equality
	{
		[Test]
		public void SingleFile()
		{
			var path1 = new NPath("myfile");
			var path2 = new NPath("myfile");
			Assert.IsTrue(path1.Equals(path2));
			Assert.IsTrue(path1 == path2);
		}

		[Test]
		public void InFolderButDifferentSlashes()
		{
			var path1 = new NPath(@"mydir/myfile");
			var path2 = new NPath(@"mydir\myfile");
			Assert.IsTrue(path1.Equals(path2));
			Assert.IsTrue(path1 == path2);
		}

		[Test]
		public void WithDifferentDriveLetters()
		{
			var path1 = new NPath(@"c:\mydir\myfile");
			var path2 = new NPath(@"e:\mydir\myfile");
			Assert.IsFalse(path1.Equals(path2));
			Assert.IsFalse(path1 == path2);
		}

		[Test]
		public void OneRootedOneNonRooted()
		{
			var path1 = new NPath(@"\mydir\myfile");
			var path2 = new NPath(@"mydir\myfile");
			Assert.IsFalse(path1.Equals(path2));
			Assert.IsFalse(path1 == path2);
		}

		[Test]
		public void OneWithTrailingSlash()
		{
			var path1 = new NPath(@"mydir/mydir2");
			var path2 = new NPath(@"mydir/mydir2/");
			Assert.IsTrue(path1.Equals(path2));
			Assert.IsTrue(path1 == path2);
		}
	}
}