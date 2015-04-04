using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace NiceIO.Tests
{
	public class TestWithTempDir
	{
		protected NPath _tempPath;

		[SetUp]
		public void Setup()
		{
			_tempPath = NPath.CreateTempDirectory("NiceIOTest");
		}

		[TearDown]
		public void TearDown()
		{
			_tempPath.Delete();
		}

		protected void PopulateTempDir(string[] entries)
		{
			foreach (var entry in entries)
			{
				if (entry[entry.Length - 1] == '/')
					Directory.CreateDirectory(_tempPath + "/" + entry);
				else
					File.WriteAllText(_tempPath + "/" + entry, "hello");
			}
		}

		protected void AssertTempDir(IEnumerable<string> entries)
		{
			var entriesArray = entries.ToArray();
			entriesArray = entriesArray.OrderBy(s => s).ToArray();
			var expectedPaths = entriesArray.Select(s => _tempPath.Combine(s).RelativeTo(_tempPath));

			var actualPaths = _tempPath.Contents(recurse: true).OrderBy(s => s.ToString()).ToArray();

			CollectionAssert.AreEquivalent(expectedPaths, actualPaths.Select(p => p.RelativeTo(_tempPath)));

			for (var i = 0; i != entriesArray.Length; i++)
			{
				if (!entriesArray[i].EndsWith("/"))
					continue;

				Assert.IsTrue(actualPaths[i].DirectoryExists(), actualPaths[i] + " was expected to be a directory");
			}
		}
	}
}