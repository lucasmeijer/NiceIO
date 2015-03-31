using NUnit.Framework;

namespace NiceIO.Tests
{
	[TestFixture]
	public class ExtensionWithDot
	{
		[Test]
		public void Simple()
		{
			Assert.AreEqual(".txt", new Path("file.txt").ExtensionWithDot);
		}

		[Test]
		public void FileWithoutExtension()
		{
			Assert.AreEqual("", new Path("myfile").ExtensionWithDot);
		}

		[Test]
		public void FileWithMultipleDots()
		{
			Assert.AreEqual(".exe", new Path("myfile.something.something.exe").ExtensionWithDot);
		}
	}
}
