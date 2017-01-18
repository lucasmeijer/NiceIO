using System;
using NUnit.Framework;

namespace NiceIO.Tests
{
	[TestFixture]
	public class ExtensionWithDot
	{
		[Test]
		public void Simple()
		{
			Assert.AreEqual(".txt", new NPath("file.txt").ExtensionWithDot);
		}

		[Test]
		public void FileWithoutExtension()
		{
			Assert.AreEqual("", new NPath("myfile").ExtensionWithDot);
		}

		[Test]
		public void FileWithMultipleDots()
		{
			Assert.AreEqual(".exe", new NPath("myfile.something.something.exe").ExtensionWithDot);
		}

		[Test]
		public void ExtensionWithDotOnLinuxRoot()
		{
			Assert.Throws<ArgumentException>(() =>
			{
				var result = new NPath("/").ExtensionWithDot;
			});
		}

		[Test]
		public void ExtensionWithDotOnWindowsRoot()
		{
			Assert.Throws<ArgumentException>(() =>
			{
				var result = new NPath("C:\\").ExtensionWithDot;
			});
		}
	}
}
