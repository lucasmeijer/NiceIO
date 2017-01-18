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
			Assert.IsFalse(new NPath("/a/b/c").IsChildOf("/unrelated"));
		}

		[Test]
		public void AbsoluteAndRelative()
		{
			Assert.Throws<ArgumentException>(() => new NPath("/a/b/c").IsChildOf("the/relative"));
		}

		[Test]
		public void RelativeAndAbsolute()
		{
			Assert.Throws<ArgumentException>(() => new NPath("relative").IsChildOf("/a/b/c"));
		}

		[Test]
		public void TwoRelative()
		{
			Assert.IsTrue(new NPath("hello/there").IsChildOf(new NPath("hello")));
		}

		[Test]
		public void TwoRelativeUnrelated()
		{
			Assert.IsFalse(new NPath("hello/there").IsChildOf(new NPath("boink")));
		}

		[Test]
		public void WindowsPathIsChildOfLinuxRoot()
		{
			Assert.IsFalse(new NPath("C:\\hello\\there").IsChildOf(new NPath("/")));
		}

		[Test]
		public void LinuxPathIsChildOfWindowsRoot()
		{
			Assert.IsFalse(new NPath("/hello/there").IsChildOf(new NPath("C:\\")));
		}
	}
}
