using NUnit.Framework;

namespace NiceIO.Tests
{
    [TestFixture]
    public class UNCServerName
    {
        [Test]
        public void CRoot() => Assert.AreEqual(null, new NPath("C:/").UNCServerName);

        [Test]
        public void NormalRoot() => Assert.AreEqual(null, new NPath("/").UNCServerName);

        [Test]
        public void RelativeFile() => Assert.AreEqual(null, new NPath("somefile").UNCServerName);

        [Test]
        public void NormalAbsoluteFile() => Assert.AreEqual(null, new NPath("/somedir/somefile").UNCServerName);

        [Test]
        public void CAbsoluteFile() => Assert.AreEqual(null, new NPath("C:/somedir/somefile").UNCServerName);

        [Test]
        public void UNCRoot() => Assert.AreEqual("MyWindowsPC", new NPath("\\\\MyWindowsPC/").UNCServerName);

        [Test]
        public void UNCPath() => Assert.AreEqual("MyWindowsPC", new NPath("\\\\MyWindowsPC/somefile").UNCServerName);
    }
}
