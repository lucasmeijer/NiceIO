using NUnit.Framework;

namespace NiceIO.Tests
{
	[TestFixture]
	public class Exists : TestWithTempDir
	{
		[Test]
		public void FileExists()
		{
			PopulateTempDir(new[] {"somefile"});
			Assert.IsTrue(_tempPath.Combine("somefile").Exists());
			Assert.IsTrue(_tempPath.Combine("somefile").FileExists());
			Assert.IsFalse(_tempPath.Combine("somefile").DirectoryExists());
		}

		[Test]
		public void DirectoryExists()
		{
			PopulateTempDir(new[] {"somefile/"});
			Assert.IsTrue(_tempPath.Combine("somefile").Exists());
			Assert.IsFalse(_tempPath.Combine("somefile").FileExists());
			Assert.IsTrue(_tempPath.Combine("somefile").DirectoryExists());
		}
	}
}