using NUnit.Framework;

namespace NiceIO.Tests
{
	[TestFixture]
	public class ParentContaining : TestWithTempDir
	{
		[Test]
		public void TwoLevelsDown()
		{
			PopulateTempDir(new [] { "somedir/","somedir/dir2/","somedir/dir2/myfile", "somedir/needle"});

			Assert.AreEqual(_tempPath.Combine("somedir"), _tempPath.Combine("somedir/dir2/myfile").ParentContaining("needle"));
		}

		[Test]
		public void NonExisting()
		{
			PopulateTempDir(new[] { "somedir/", "somedir/dir2/", "somedir/dir2/myfile" });

			Assert.IsNull(_tempPath.Combine("somedir/dir2/myfile").ParentContaining("nonexisting"));
		}

		[Test]
		public void WithComplexNeedle()
		{
			PopulateTempDir(new[] { "somedir/", "somedir/dir2/", "somedir/dir2/myfile" ,"needledir/","needledir/needlefile"});

			Assert.AreEqual(_tempPath, _tempPath.Combine("somedir/dir2/myfile").ParentContaining(new NPath("needledir/needlefile")));
		}

		[Test]
		public void InRelativePath()
		{
			PopulateTempDir(new[] { "somedir/", "somedir/dir2/", "somedir/dir2/myfile" ,"needledir/","needledir/needlefile"});
			using (NPath.SetCurrentDirectory(_tempPath))
				Assert.AreEqual(new NPath(""), new NPath("somedir/dir2/myfile").ParentContaining("needledir/needlefile"));
		}
	}
}
