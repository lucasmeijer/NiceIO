using System;

namespace NiceIO.Tests
{
	public class TempDir : IDisposable
	{
		private readonly Path _path;

		public TempDir(string prefix)
		{
			_path = Path.CreateTempDirectory(prefix);
		}

		public void Dispose()
		{
			_path.Delete();
		}
	}
}