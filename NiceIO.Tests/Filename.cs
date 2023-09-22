using NUnit.Framework;

namespace NiceIO.Tests
{
    [TestFixture]
    public class FileName
    {
        [Test]
        public void FileWithExtension() => Assert.AreEqual("myfile.txt", new NPath("/hello/there/myfile.txt").FileName);

        [Test]
        public void FileWithoutExtension() => Assert.AreEqual ("myfile", new NPath ("/hello/there/myfile").FileName);

        [Test]
        public void FileWithoutMultipleDots() => Assert.AreEqual ("myfile.weird.txt", new NPath ("/hello/there/myfile.weird.txt").FileName);

        [Test]
        public void Empty() => Assert.AreEqual ("", new NPath ("").FileName);

        [Test]
        public void TrailingSlash() => Assert.AreEqual ("mydir", new NPath ("mydir/").FileName);
        
        [Test]
        public void WithoutSlashes() => Assert.AreEqual ("no_slashes_here", new NPath ("no_slashes_here").FileName);
    }
}
