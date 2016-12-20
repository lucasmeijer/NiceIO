using System.IO;
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

			Assert.IsTrue(_tempPath.Exists("somefile"));
			Assert.IsTrue(_tempPath.FileExists("somefile"));
			Assert.IsFalse(_tempPath.DirectoryExists("somefile"));
		}

		[Test]
		public void DirectoryExists()
		{
			PopulateTempDir(new[] {"somefile/"});
			Assert.IsTrue(_tempPath.Combine("somefile").Exists());
			Assert.IsFalse(_tempPath.Combine("somefile").FileExists());
			Assert.IsTrue(_tempPath.Combine("somefile").DirectoryExists());
		}

		[Test]
		public void FileWithSpace()
		{
			PopulateTempDir(new[] {"some file"});
			Assert.IsTrue(_tempPath.Combine("some file").FileExists());
			AssertTempDir(new [] {"some file"});
		}

		[Test]
		public void AssertFileExistsOkWhenExists()
		{
			PopulateTempDir(new[] { "somefile" });
			Assert.That(_tempPath.Combine("somefile"), Is.EqualTo(_tempPath.Combine("somefile").AssertFileExists()));
		}

		[Test]
		public void AssertFileExistsThrowsWhenDoesNotExists()
		{
			Assert.Throws<FileNotFoundException>(() => _tempPath.Combine("somefile").AssertFileExists());
		}

		[Test]
		public void AssertDirectoryExistsOkWhenExists()
		{
			PopulateTempDir(new[] { "somefile/" });
			Assert.That(_tempPath.Combine("somefile"), Is.EqualTo(_tempPath.Combine("somefile").AssertDirectoryExists()));
		}

		[Test]
		public void AssertDirectExistsThrowsWhenDoesNotExists()
		{
			Assert.Throws<DirectoryNotFoundException>(() => _tempPath.Combine("somefile").AssertDirectoryExists());
		}
	}
}