using System;
using NUnit.Framework;

namespace NiceIO.Tests
{
	[TestFixture]
	public class Construction
	{
		[Test]
		public void OnlyFileName()
		{
			var path = new NPath("myfile.exe");
			Assert.AreEqual("myfile.exe", path.ToString());
		}

		[Test]
		public void FromStringWithForwardSlash()
		{
			var path = new NPath("mydir/myfile.exe");
			Assert.AreEqual("mydir/myfile.exe", path.ToString(SlashMode.Forward));
		}

		[Test]
		public void FromStringWithBackSlash()
		{
			var path = new NPath(@"mydir\myfile.exe");
			Assert.AreEqual("mydir/myfile.exe", path.ToString(SlashMode.Forward));
		}

		[Test]
		public void FromRootedString()
		{
			var path = new NPath(@"/mydir/myfile.exe");
			Assert.AreEqual("/mydir/myfile.exe", path.ToString(SlashMode.Forward));
		}

		[Test]
		public void FromStringWithWindowsDrive()
		{
			var path = new NPath(@"C:\mydir\myfile.exe");
			Assert.AreEqual("C:/mydir/myfile.exe", path.ToString(SlashMode.Forward));
			Assert.IsFalse(path.IsRelative);
		}

		[Test]
		public void FromStringWithTrailingSlash()
		{
			var path = new NPath("/mydir/myotherdir/");
			Assert.AreEqual("/mydir/myotherdir", path.ToString(SlashMode.Forward));
			Assert.AreEqual("myotherdir", path.FileName);
		}

		[Test]
		public void FromStringWithMultipleSlashes()
		{
			var path = new NPath("///mydir////myfile.txt");
			Assert.AreEqual("/mydir/myfile.txt", path.ToString(SlashMode.Forward));
		}

		[Test]
		public void WithNullArgument()
		{
			Assert.Throws<ArgumentNullException>(() => new NPath(null));
		}

		[Test]
		public void WithDotDotInPath()
		{
			var path = new NPath("/mydir/../myotherdir/myfile.txt");
			Assert.AreEqual("/myotherdir/myfile.txt", path.ToString(SlashMode.Forward));			
		}

		[Test]
		public void WithDotDotInStartOfRelativePath()
		{
			var path = new NPath("../../myotherdir/myfile.txt");
			Assert.AreEqual("../../myotherdir/myfile.txt", path.ToString(SlashMode.Forward));
		}

		[Test]
		public void WithDotDotInStartOfAbsolutePath()
		{
			Assert.Throws<ArgumentException>(() => new NPath("/../../myotherdir/myfile.txt"));
		}

		[Test]
		public void WithEmptyString()
		{
			Assert.AreEqual(".", new NPath("").ToString());
		}
	}
}