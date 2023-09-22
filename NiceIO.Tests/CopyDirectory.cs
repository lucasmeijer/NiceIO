using NUnit.Framework;

namespace NiceIO.Tests
{
	[TestFixture]
	public class CopyDirectory : TestWithTempDir
	{
		[Test]
		public void InsideSameDirectory()
		{
			PopulateTempDir(new[] {"somedir/", "somedir/myfile"});

			var dest = _tempPath.Combine("somedir2");
			var result = _tempPath.Combine("somedir").Copy(dest);
			Assert.AreEqual(dest, result);

			AssertTempDir(new[]
			{
				"somedir/",
				"somedir/myfile",
				"somedir2/",
				"somedir2/myfile"
			});
		}

		[Test]
		public void InsideSameDirectoryLongPath()
		{
			PopulateTempDir(new[] { "somedir/", "somedir/myfile" });

			var dest = _tempPath.Combine(kLongPath);
			var result = _tempPath.Combine("somedir").Copy(dest);
			Assert.AreEqual(dest, result);

			Assert.IsTrue(_tempPath.Combine("somedir").Exists());
			Assert.IsTrue(_tempPath.Combine("somedir/myfile").Exists());
			Assert.IsTrue(dest.Exists());
			Assert.IsTrue(dest.Combine("myfile").Exists());

		}

		[Test]
		public void WithFilter()
		{
			PopulateTempDir(new[]
			{
				"somedir/", 
				"somedir/myfile", 
				"somedir/subdir/",
				"somedir/subdir/myfile2",
				"somedir/subdir/myfile3"
			});

			_tempPath.Combine("somedir").Copy(_tempPath.Combine("somedir2"), p =>
			{
				//only let myfil2 through
				return p.FileName == "myfile2";
			});

			AssertTempDir(new[]
			{
				"somedir/", 
				"somedir/myfile", 
				"somedir/subdir/",
				"somedir/subdir/myfile2",
				"somedir/subdir/myfile3",

				"somedir2/",
				"somedir2/subdir/",
				"somedir2/subdir/myfile2"
			});
		}

		[Test]
		public void WithFilterInLongDirectory()
		{
			PopulateTempDir(new[]
			{
				"somedir/",
				"somedir/myfile",
				"somedir/subdir/",
				"somedir/subdir/myfile2",
				"somedir/subdir/myfile3"
			});

			var dest = _tempPath.Combine(kLongPath);
			_tempPath.Combine("somedir").Copy(dest, p =>
			{
				//only let myfil2 through
				return p.FileName == "myfile2";
			});

			Assert.IsFalse(dest.Combine("myfile").Exists());
			Assert.IsTrue(dest.Combine("subdir/myfile2").Exists());
			Assert.IsFalse(dest.Combine("subdir/myfile3").Exists());
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
				"targetdir/somedir/",
			});

			var returnValue = _tempPath.Combine("somedir").Copy(_tempPath.Combine("targetdir"));
			Assert.AreEqual (_tempPath.Combine ("targetdir/somedir"), returnValue);

			AssertTempDir(new[]
			{
				"somedir/", 
				"somedir/myfile", 
				"targetdir/",
				"targetdir/pre_existing_file",

				"targetdir/somedir/",
				"targetdir/somedir/myfile",	
			});
		}

		[Test]
		public void IntoExistingLongDirectory()
		{
			var longPath = PopulateSpecificDir(kLongPath, new[]
			{
				"somedir/",
				"somedir/myfile",
				"targetdir/",
				"targetdir/pre_existing_file",
				"targetdir/somedir/"
			});

			var returnValue = longPath.Combine("somedir").Copy(longPath.Combine("targetdir"));
			Assert.AreEqual(longPath.Combine("targetdir/somedir"), returnValue);

			AssertSpecificDir(longPath, new[] {
				"somedir/",
				"somedir/myfile",
				"targetdir/",
				"targetdir/pre_existing_file",

				"targetdir/somedir/",
				"targetdir/somedir/myfile",
			});
		}
	}
}
