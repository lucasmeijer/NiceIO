using System;

namespace NiceIO.Tests
{
	public class TempDir : IDisposable
	{
		private readonly NPath _path;

		public TempDir(string prefix)
		{
			_path = NPath.CreateTempDirectory(prefix);
		}

		public void Dispose()
		{
			_path.Delete();
		}
	}
}