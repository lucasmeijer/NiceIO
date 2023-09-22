using NUnit.Framework;

namespace NiceIO.Tests
{
	[TestFixture]
	public class EnsureDirectoryExists : TestWithTempDir
	{
		[Test]
		public void CreatesDirectory()
		{
			var subdir = _tempPath.Combine("subdir");
			Assert.AreEqual(subdir, subdir.EnsureDirectoryExists());

			AssertTempDir(new[] { "subdir/" });
		}

		[Test]
		public void CreatesDirectoryLongPath()
		{
			var subdir = _tempPath.Combine(kLongPath);
			Assert.AreEqual(subdir, subdir.EnsureDirectoryExists());
			Assert.IsTrue(subdir.DirectoryExists());
		}

		[Test]
		public void ExistingDirectory()
		{
			PopulateTempDir(new[ ]{ "subdir/"});
			var subdir = _tempPath.Combine("subdir");
			Assert.AreEqual(subdir,subdir.EnsureDirectoryExists());
			AssertTempDir(new[] { "subdir/" });
		}

		[Test]
		public void CreatesDirectoryWithArgument()
		{
			_tempPath.EnsureDirectoryExists("subdir");
			AssertTempDir(new[] { "subdir/" });
		}

		[Test]
		public void EnsureParentDirectoryExists()
		{
			Assert.AreEqual(_tempPath.Combine("subdir1/subdir2/myfile"),_tempPath.Combine("subdir1/subdir2/myfile").EnsureParentDirectoryExists());

			AssertTempDir(new[] { "subdir1/","subdir1/subdir2/" });
		}

		[Test]
		public void EnsureParentDirectoryExistsLongPath()
		{
			var subdir = _tempPath.Combine(kLongPath);
			Assert.AreEqual(subdir, subdir.EnsureDirectoryExists());

			do
			{
				subdir.EnsureParentDirectoryExists();
				subdir = subdir.Parent;
			}
			while (subdir != _tempPath);
		}
	}
}
