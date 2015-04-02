using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace NiceIO.Tests
{
	[TestFixture]
	public class IsChildOf : TestWithTempDir
	{
		[Test]
		public void FileInDir()
		{
			Assert.IsTrue(_tempPath.Combine("file").IsChildOf(_tempPath));
		}

		[Test]
		public void FileInSubDir()
		{
			Assert.IsTrue(_tempPath.Combine("subdir/file").IsChildOf(_tempPath));	
		}

		[Test]
		public void UnrelatedPaths()
		{
			Assert.IsFalse(new Path("/a/b/c").IsChildOf("/unrelated"));
		}

		[Test]
		public void AbsoluteAndRelative()
		{
			Assert.Throws<ArgumentException>(() => new Path("/a/b/c").IsChildOf("the/relative"));
		}

		[Test]
		public void RelativeAndAbsolute()
		{
			Assert.Throws<ArgumentException>(() => new Path("relative").IsChildOf("/a/b/c"));
		}

		[Test]
		public void TwoRelative()
		{
			Assert.IsTrue(new Path("hello/there").IsChildOf(new Path("hello")));
		}

		[Test]
		public void TwoRelativeUnrelated()
		{
			Assert.IsFalse(new Path("hello/there").IsChildOf(new Path("boink")));
		}

	}
}
