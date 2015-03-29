using NUnit.Framework;

namespace NiceIO.Tests
{
	[TestFixture]
	public class MoveDirectory : TestWithTempDir
	{
		[Test]
		public void InsideSameDirectory()
		{
			PopulateTempDir(new[] { "somedir/", "somedir/myfile" });

			_tempPath.Combine("somedir").Move(_tempPath.Combine("somedir2"));

			AssertTempDir(new[]
			{
				"somedir2/",
				"somedir2/myfile"
			});
		}
		/*
		[Test]
		public void WithFilter()
		{
			PopulateTempDir(new[] { "somedir/", "somedir/myfile", "somedir/myfile2" });

			_tempPath.Combine("somedir").Copy(_tempPath.Combine("somedir2"), p => p.FileName != "myfile");

			AssertTempDir(new[]
			{
				"somedir/",
				"somedir/myfile",
				"somedir/myfile2",

				"somedir2/",
				"somedir2/myfile2"
			});
		}

		public void IntoExistingDirectory()
		{
			PopulateTempDir(new[]
			{
				"somedir/", 
				"somedir/myfile", 
				"targetdir/",
				"targetdir/pre_existing_file",
			});

			_tempPath.Combine("somedir").Copy(_tempPath.Combine("targetdir"));

			AssertTempDir(new[]
			{
				"somedir/",
				"somedir/myfile",
				"targetdir/",
				"targetdir/pre_existing_file",
				"targetdir/myfile",
			});
		}*/
	}
}