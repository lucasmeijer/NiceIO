using System.IO;

namespace NiceIO.Tests
{
	class FakeFileSystemForFakeDirectoryTests : NPath.RelayingFileSystem
	{
		NPath CurrentDirectory { get; }
		public FakeFileSystemForFakeDirectoryTests(string fakeDir) : base(Active) => CurrentDirectory = fakeDir;
		public override NPath Directory_GetCurrentDirectory() => CurrentDirectory;
		public override NPath[] Directory_GetFiles(NPath path, string filter, SearchOption searchOptions) => BaseFileSystem.Directory_GetFiles(path, filter, searchOptions);
	}
}