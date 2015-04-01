using System;
using System.IO;
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

			var destination = _tempPath.Combine("somedir2");
			var returnValue = _tempPath.Combine("somedir").Move(destination);

			Assert.AreEqual(destination,returnValue);

			AssertTempDir(new[]
			{
				"somedir2/",
				"somedir2/myfile"
			});
		}

		[Test]
		public void IntoExistingDirectory()
		{
			var entries = new[]
			{
				"somedir/", 
				"somedir/myfile", 
				"targetdir/",
				"targetdir/pre_existing_file",
			};
			PopulateTempDir(entries);

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
	}
}