using NUnit.Framework;

namespace NiceIO.Tests
{
	[TestFixture]
	public class HasExtension
	{
		[Test]
		public void WithDot()
		{
			Assert.IsTrue(new NPath("myfile.txt").HasExtension(".txt"));
		}
		
		[Test]
		public void WithoutDot()
		{
			Assert.IsTrue(new NPath("myfile.txt").HasExtension("txt"));
		}

		[Test]
		public void IsCaseInsensitive()
		{
			Assert.IsTrue(new NPath("myfile.txt").HasExtension("TxT"));
		}

		[Test]
		public void WithDotWrongExtension()
		{
			Assert.IsFalse(new NPath("myfile.txt").HasExtension(".exe"));
		}

		[Test]
		public void WithoutDotWrongExtension()
		{
			Assert.IsFalse(new NPath("myfile.txt").HasExtension("exe"));
		}

		[Test]
		public void WithMultipleArguments_Params()
		{
			Assert.IsTrue(new NPath("myfile.txt").HasExtension("exe", "txt"));
		}

		[Test]
		public void WithStarExtension()
		{
			Assert.IsTrue(new NPath("myfile.txt").HasExtension("*"));
		}

		[Test]
		public void WithStarExtensionAtWindowsRootPath()
		{
			Assert.IsTrue(new NPath("C:").HasExtension("*"));
		}

		[Test]
		public void WithStarExtensionAtUnixRootPath()
		{
			Assert.IsTrue(new NPath("/").HasExtension("*"));
		}

		[Test]
		public void AtWindowsRootPath()
		{
			Assert.IsFalse(new NPath("C:").HasExtension(".txt"));
		}

		[Test]
		public void AtUnixRootPath()
		{
			Assert.IsFalse(new NPath("/").HasExtension(".exe"));
		}

		[Test]
		public void WithExtensionLongerThanPath()
		{
			Assert.IsFalse(new NPath("f.txt").HasExtension("txttxt"));
		}

		[Test]
		public void WithEmptyExtension()
		{
			Assert.IsFalse(new NPath("file.txt").HasExtension(""));
		}

		[Test]
		public void WithEmptyExtensionPathEndsWithDot()
		{
			Assert.IsTrue(new NPath("file.").HasExtension(""));
		}

		[Test]
		public void WithEmptyExtensionPathWithoutDots()
		{
			Assert.IsFalse(new NPath("file").HasExtension(""));
		}
	}
}
