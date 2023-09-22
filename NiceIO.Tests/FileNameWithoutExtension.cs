using NUnit.Framework;

namespace NiceIO.Tests
{
	[TestFixture]
	public class FileNameWithoutExtension
	{
		[Test]
		public void FileWithExtension()
		{
			Assert.AreEqual("myfile", new NPath("/hello/there/myfile.txt").FileNameWithoutExtension);
		}

		[Test]
		public void FileWithoutExtension()
		{
			Assert.AreEqual ("myfile", new NPath ("/hello/there/myfile").FileNameWithoutExtension);
		}

		[Test]
		public void FileWithoutMultipleDots()
		{
			Assert.AreEqual ("myfile.weird", new NPath ("/hello/there/myfile.weird.txt").FileNameWithoutExtension);
		}
	}
}
