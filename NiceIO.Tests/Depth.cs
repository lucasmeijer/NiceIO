using NUnit.Framework;

namespace NiceIO.Tests
{
    [TestFixture]
    public class Depth
    {
        [Test]
        public void Root() => Assert.AreEqual(0, new NPath("/").Depth);

        [Test]
        public void CurrentDir() => Assert.AreEqual(0, new NPath(".").Depth);

        [Test]
        public void File() => Assert.AreEqual(1, new NPath("myfile").Depth);

        [Test]
        public void FileInDir() => Assert.AreEqual(2, new NPath("mydir/myfile").Depth);

        [Test]
        public void WithDriveLetter() => Assert.AreEqual(2, new NPath("c:/mydir/myfile").Depth);

        [Test]
        public void WithUNCServerName() => Assert.AreEqual(2, new NPath("\\\\MyWindowsPC/mydir/myfile").Depth);
    }
}
