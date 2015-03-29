using System.IO;
using NUnit.Framework;

namespace NiceIO.Tests
{
	public class TestWithTempDir
	{
		protected DeleteOnDisposePath _tempPath;

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

		protected void PopulateTempDir(string[] entries)
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