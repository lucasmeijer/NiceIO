using System.IO;
using System.Linq;
using NUnit.Framework;

namespace NiceIO.Tests
{
	public class TestWithTempDir
	{
		protected Path _tempPath;

		[SetUp]
		public void Setup()
		{
			_tempPath = Path.CreateTempDirectory("NiceIOTest");
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

		protected void AssertTempDir(string[] entries)
		{
			entries = entries.OrderBy(s => s).ToArray();
			var expectedPaths = entries.Select(s => _tempPath.Combine(s).RelativeTo(_tempPath));

			var actualPaths = _tempPath.Contents(recurse: true).OrderBy(s => s.ToString()).ToArray();

			CollectionAssert.AreEquivalent(expectedPaths, actualPaths.Select(p => p.RelativeTo(_tempPath)));

			for (var i = 0; i != entries.Length; i++)
			{
				if (!entries[i].EndsWith("/"))
					continue;

				Assert.IsTrue(actualPaths[i].DirectoryExists(), actualPaths[i] + " was expected to be a directory");
			}
		}
	}
}