using NUnit.Framework;
using System.Linq;

namespace NiceIO.Tests
{
	[TestFixture]
	public class MoveDirectory : TestWithTempDir
	{
		[Test]
		public void InsideSameDirectory()
		{
			PopulateTempDir(new[] { "somedir/", "somedir/myfile" });

			var destination = _tempPath.Combine("somedir2");
			var returnValue = _tempPath.Combine("somedir").Move(destination);

			Assert.AreEqual(destination, returnValue);

			AssertTempDir(new[]
			{
				"somedir2/",
				"somedir2/myfile"
			});
		}

		[Test]
		public void InsideSameDirectoryLongPath()
		{
			var longPath = PopulateSpecificDir(kLongPath, new[] { "myfile.txt", "somedir/", "somedir/myfile" });

			CollectionAssert.AreEquivalent(new[] { "somedir" }, longPath.Directories(true).Select(p => p.FileName));

			var destination = longPath.Combine("somedir2");
			var returnValue = longPath.Combine("somedir").Move(destination);

			Assert.AreEqual(destination, returnValue);

			AssertSpecificDir(longPath, new[]
			{
				"myfile.txt",
				"somedir2/",
				"somedir2/myfile"
			});
		}

		[Test]
		public void IntoExistingDirectory()
		{
			PopulateTempDir(new[]
			{
				"somedir/",
				"somedir/myfile",
				"targetdir/",
				"targetdir/pre_existing_file",
			});

			var result = _tempPath.Combine("somedir").Move(_tempPath.Combine("targetdir"));
			Assert.AreEqual(_tempPath.Combine("targetdir/somedir"), result);

			AssertTempDir(new[]
			{
				"targetdir/",
				"targetdir/somedir/",
				"targetdir/somedir/myfile",
				"targetdir/pre_existing_file",
			});
		}

		[Test]
		public void IntoExistingDirectoryLongPath()
		{
			var longPath = PopulateSpecificDir(kLongPath, new[]
			{
				"somedir/",
				"somedir/myfile",
				"targetdir/",
				"targetdir/pre_existing_file",
			});

			var result = longPath.Combine("somedir").Move(longPath.Combine("targetdir"));
			Assert.AreEqual(longPath.Combine("targetdir/somedir"), result);

			AssertSpecificDir(longPath, new[]
			{
				"targetdir/",
				"targetdir/somedir/",
				"targetdir/somedir/myfile",
				"targetdir/pre_existing_file",
			});
		}
	}
}