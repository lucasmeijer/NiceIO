using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using NUnit.Framework;

namespace NiceIO.Tests
{
	public class TestWithTempDir
	{
		protected static readonly string kLongPathPart = "subdirectorywithalongname/";
		protected static readonly string kLongPath = string.Concat(Enumerable.Repeat(kLongPathPart, 16));
		protected NPath _tempPath;

		[SetUp]
		public virtual void Setup()
		{
			_tempPath = NPath.CreateTempDirectory("NiceIOTest");
		}

		[TearDown]
		public void TearDown()
		{
			try
			{
				_tempPath.Delete();
			}
			catch (IOException)
			{
				if (Environment.OSVersion.Platform != PlatformID.Win32Windows && Environment.OSVersion.Platform != PlatformID.Win32NT)
					throw;

				// Maybe this will reduce test teardown flakiness on windows?
				// Should we just ignore this and leave tempdirs behind?
				Thread.Sleep(2000);
				_tempPath.Delete();
			}
		}

		protected void PopulateTempDir(IEnumerable<string> entries)
		{
			foreach (var entry in entries)
			{
				if (entry[entry.Length - 1] == '/' || entry[entry.Length - 1] == '\\')
					Directory.CreateDirectory(_tempPath + "/" + entry);
				else
					File.WriteAllText(_tempPath + "/" + entry, "hello");
			}
		}

		protected NPath PopulateSpecificDir(string path, string[] entries)
		{
			return PopulateSpecificDir(_tempPath.Combine(path).CreateDirectory(), entries);
		}

		protected NPath PopulateSpecificDir(NPath path, string[] entries)
		{
			foreach (var entry in entries)
			{
				if (entry[entry.Length - 1] == '/' || entry[entry.Length - 1] == '\\')
					path.Combine(entry.Substring(0, entry.Length - 1)).CreateDirectory();
				else
					path.Combine(entry).WriteAllText("hello");
			}

			return path;
		}

		protected void AssertTempDir(IEnumerable<string> entries)
		{
			AssertSpecificDir(_tempPath, entries);
		}

		protected void AssertSpecificDir(NPath path, IEnumerable<string> entries, bool recurse = true, bool onlyFiles = false)
		{
			var entriesArray = entries.ToArray();
			Array.Sort(entriesArray, string.CompareOrdinal);
			var expectedPaths = entriesArray.Select(s => path.Combine(s).RelativeTo(path));

			var actualPaths = onlyFiles ? path.Files(recurse).ToArray() : path.Contents(recurse).ToArray();
			Array.Sort(actualPaths, (a, b) => string.CompareOrdinal(a.ToString(), b.ToString()));

			CollectionAssert.AreEquivalent(expectedPaths, actualPaths.Select(p => p.RelativeTo(path)));

			for (var i = 0; i != entriesArray.Length; i++)
			{
				if (!entriesArray[i].EndsWith("/", StringComparison.Ordinal))
					continue;

				Assert.IsTrue(actualPaths[i].DirectoryExists(), actualPaths[i] + " was expected to be a directory");
			}
		}
	}
}
