using NUnit.Framework;

namespace NiceIO.Tests
{
	[TestFixture]
	public class Equality
	{
		[Test]
		public void SingleFile()
		{
			var path1 = new Path("myfile");
			var path2 = new Path("myfile");
			Assert.IsTrue(path1.Equals(path2));
			Assert.IsTrue(path1 == path2);
		}

		[Test]
		public void InFolderButDifferentSlashes()
		{
			var path1 = new Path(@"mydir/myfile");
			var path2 = new Path(@"mydir\myfile");
			Assert.IsTrue(path1.Equals(path2));
			Assert.IsTrue(path1 == path2);
		}

		[Test]
		public void WithDifferentDriveLetters()
		{
			var path1 = new Path(@"c:\mydir\myfile");
			var path2 = new Path(@"e:\mydir\myfile");
			Assert.IsFalse(path1.Equals(path2));
			Assert.IsFalse(path1 == path2);
		}

		[Test]
		public void OneRootedOneNonRooted()
		{
			var path1 = new Path(@"\mydir\myfile");
			var path2 = new Path(@"mydir\myfile");
			Assert.IsFalse(path1.Equals(path2));
			Assert.IsFalse(path1 == path2);
		}

		[Test]
		public void OneWithTrailingSlash()
		{
			var path1 = new Path(@"mydir/mydir2");
			var path2 = new Path(@"mydir/mydir2/");
			Assert.IsTrue(path1.Equals(path2));
			Assert.IsTrue(path1 == path2);
		}
	}
}