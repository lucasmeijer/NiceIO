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
			Assert.AreEqual("mysubdir", new Path("mydir").Combine("mysubdir/").FileName);
		}

		[Test]
		public void WithRootedArgument()
		{
			Assert.Throws<ArgumentException>(() => new Path("/somedir").Combine(new Path("/other")));
		}

		[Test]
		public void Simple()
		{
			Assert.AreEqual(new Path("/somedir/other/myfile"), new Path("/somedir").Combine(new Path("other/myfile")));
		}

		[Test]
		public void WithRelativePathStartingWithDotDot()
		{
			Assert.AreEqual(new Path("/other/myfile"), new Path("/somedir/somedir2").Combine(new Path("../../other/myfile")));	
		}

		[Test]
		public void CombiningDotDotOntoRelativePath()
		{
			Assert.AreEqual(new Path("../other/myfile"), new Path("somedir/somedir2").Combine(new Path("../../../other/myfile")));	
		}

		[Test]
		public void WithMultipleArguments()
		{
			Assert.AreEqual(new Path("/a/b/c/d/e"), new Path("/a").Combine("b", "c/d", "e"));
		}
	}
}