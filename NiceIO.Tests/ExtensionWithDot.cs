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
	}
}
