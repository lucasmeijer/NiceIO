using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace NiceIO.Tests
{
	[TestFixture]
	public class Exists
	{
		private DeleteOnDisposePath _tempPath;

		[SetUp]
		public void Setup()
		{
			_tempPath = Path.CreateTempDirectory("Exists");
		}

		[TearDown]
		public void TearDown()
		{
			_tempPath.Dispose();
		}

		[Test]
		public void FileExists()
		{
			PopulateTempDir(new[] {"somefile"});	
			Assert.IsTrue(_tempPath.Combine("somefile").Exists());
			Assert.IsTrue(_tempPath.Combine("somefile").FileExists());
			Assert.IsFalse(_tempPath.Combine("somefile").DirectoryExists());
		}

		[Test]
		public void DirectoryExists()
		{
			PopulateTempDir(new[] { "somefile/" });
			Assert.IsTrue(_tempPath.Combine("somefile").Exists());
			Assert.IsFalse(_tempPath.Combine("somefile").FileExists());
			Assert.IsTrue(_tempPath.Combine("somefile").DirectoryExists());
		}

		private void PopulateTempDir(string[] entries)
		{
			foreach (var entry in entries)
			{
				if (entry[entry.Length - 1] == '/')
					Directory.CreateDirectory(_tempPath + "/" + entry);
				else
					File.WriteAllText(_tempPath+"/"+entry,"hello");
			}
		}
	}
}
