using System;
using NUnit.Framework;

namespace NiceIO.Tests
{
	[TestFixture]
	public class Combine
	{
		[Test]
		public void WithTrailingSlash()
		{
			Assert.AreEqual("mysubdir", new NPath("mydir").Combine("mysubdir/").FileName);
		}

		[Test]
		public void WithRootedArgument()
		{
			Assert.Throws<ArgumentException>(() => new NPath("/somedir").Combine(new NPath("/other")));
		}

		[Test]
		public void Simple()
		{
			Assert.AreEqual(new NPath("/somedir/other/myfile"), new NPath("/somedir").Combine(new NPath("other/myfile")));
		}

		[Test]
		public void WithRelativePathStartingWithDotDot()
		{
			Assert.AreEqual(new NPath("/other/myfile"), new NPath("/somedir/somedir2").Combine(new NPath("../../other/myfile")));	
		}

		[Test]
		public void CombiningDotDotOntoRelativePath()
		{
			Assert.AreEqual(new NPath("../other/myfile"), new NPath("somedir/somedir2").Combine(new NPath("../../../other/myfile")));	
		}

		[Test]
		public void WithMultipleArguments()
		{
			Assert.AreEqual(new NPath("/a/b/c/d/e"), new NPath("/a").Combine("b", "c/d", "e"));
		}

		[Test]
		public void RelativeThatGoesAboveRoot()
		{
			Assert.Throws<ArgumentException>(() => new NPath("/a").Combine(new NPath("../../b")));
		}

		[Test]
		public void CombineWithLinuxRoot()
		{
			Assert.AreEqual(new NPath("/somedir"), new NPath("/").Combine("somedir"));
		}

		[Test]
		public void CombineWithWindowsRoot()
		{
			Assert.AreEqual(new NPath("C:\\somedir"), new NPath("C:\\").Combine("somedir"));
		}
	}
}