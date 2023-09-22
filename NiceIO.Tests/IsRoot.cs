using NUnit.Framework;

namespace NiceIO.Tests
{
    [TestFixture]
    public class IsRoot
    {
        [Test]
        public void WithDriveLetter()
        {
            Assert.IsTrue(new NPath("C:\\").IsRoot);
        }

        [Test]
        public void WithUNCServerName()
        {
            Assert.IsTrue(new NPath("\\\\MyWindowsPC\\").IsRoot);
        }

        [Test]
        public void Dot()
        {
            Assert.IsFalse(new NPath(".").IsRoot);
        }

        [Test]
        public void Slash()
        {
            Assert.IsTrue(new NPath("/").IsRoot);
        }

        [Test]
        public void CombineDoubleDot()
        {
            Assert.IsTrue(new NPath("/mydir").Combine("..").IsRoot);
        }

        [Test]
        public void RootedButNotRoot()
        {
            Assert.IsFalse(new NPath("/myfile").IsRoot);
        }

        [Test]
        public void SlashInMiddle()
        {
            Assert.IsFalse(new NPath("sure/not/root").IsRoot);
        }

        [Test]
        public void DoubleDot()
        {
            Assert.IsFalse(new NPath("..").IsRoot);
        }

        [Test]
        public void WindowsRootDirectoryIsRoot()
        {
            Assert.IsTrue(new NPath("C:\\").IsRoot);
        }

        [Test]
        public void WindowsDirectoryIsNotRoot()
        {
            Assert.IsFalse(new NPath("C:\\somedir").IsRoot);
        }

        [Test]
        public void WindowsRootViaParentIsRoot()
        {
            Assert.IsTrue(new NPath("C:\\somedir").Parent.IsRoot);
        }

        [Test]
        public void WindowsUNCRootDirectoryIsRoot()
        {
            Assert.IsTrue(new NPath("\\\\MyWindowsMachine\\").IsRoot);
        }

        [Test]
        public void WindowsUNCDirectoryIsNotRoot()
        {
            Assert.IsFalse(new NPath("\\\\MyWindowsMachine\\somdir").IsRoot);
        }

        [Test]
        public void WindowsUNCRootViaParentIsRoot()
        {
            Assert.IsTrue(new NPath("\\\\MyWindowsMachine\\somedir").Parent.IsRoot);
        }

        [Test]
        public void LinuxRootDirectoryIsRoot()
        {
            Assert.IsTrue(new NPath("/").IsRoot);
        }

        [Test]
        public void LinuxDirectoryIsNotRoot()
        {
            Assert.IsFalse(new NPath("/somedir").IsRoot);
        }

        [Test]
        public void LinuxRootViaParentIsRoot()
        {
            Assert.IsTrue(new NPath("/somedir").Parent.IsRoot);
        }

        [Test]
        public void RelativePathWithSingleParentParentIsNotRoot()
        {
            var nPath = new NPath("somedir").Parent;
            Assert.IsFalse(nPath.IsRoot);
        }

        [Test]
        public void RelativePathParentIsNotRoot()
        {
            Assert.IsFalse(new NPath("somedir1/somedir2/somedir3").Parent.IsRoot);
        }
    }
}
