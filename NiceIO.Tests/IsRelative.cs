using NUnit.Framework;

namespace NiceIO.Tests
{
    [TestFixture]
    public class IsRelative : TestWithTempDir
    {
        [Test]
        public void Absolute() => Assert.IsFalse(new NPath("/file").IsRelative);

        [Test]
        public void Absolute2() => Assert.IsFalse(new NPath("/file/two").IsRelative);

        [Test]
        public void Empty() => Assert.IsTrue(new NPath("").IsRelative);

        [Test]
        public void Relative() => Assert.IsTrue(new NPath("file").IsRelative);

        [Test]
        public void Relative2() => Assert.IsTrue(new NPath("file/two").IsRelative);

        [Test]
        public void AbsoluteWithDriveLetter() => Assert.IsFalse(new NPath("c:/file").IsRelative);

        [Test]
        public void AbsoluteWithDriveLetter2() => Assert.IsFalse(new NPath("c:/file/two").IsRelative);

        [Test]
        public void AbsoluteWithUNCServerName() => Assert.IsFalse(new NPath("\\\\MyWindowsPC/file").IsRelative);

        [Test]
        public void AbsoluteWithUNCServerName2() => Assert.IsFalse(new NPath("\\\\MyWindowsPC/file/two").IsRelative);
    }
}
