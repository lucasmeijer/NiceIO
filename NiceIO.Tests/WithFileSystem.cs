using System;
using NUnit.Framework;
using Moq;

namespace NiceIO.Tests
{
    class WithFileSystem
    {
        [Test]
        public void WithFileSystemGetsRespected()
        {
            var mock = new Mock<NPath.FileSystem>();
            mock.Setup(fs => fs.File_Exists("myfile")).Returns(true).Verifiable();
            using (NPath.WithFileSystem(mock.Object))
                Assert.IsTrue(new NPath("myfile").FileExists());
            mock.Verify();
        }

        [Test]
        public void WithFileSystemRestoredPreviousFileSystem()
        {
            var mock = new Mock<NPath.FileSystem>(MockBehavior.Strict);
            mock.Setup(fs => fs.File_Exists("outer")).Returns(true);

            var mock2 = new Mock<NPath.FileSystem>(MockBehavior.Strict);
            mock2.Setup(fs => fs.File_Exists("inner")).Returns(true);

            using (NPath.WithFileSystem(mock.Object))
            {
                using (NPath.WithFileSystem(mock2.Object))
                {
                    Assert.IsTrue(new NPath("inner").FileExists());
                }

                Assert.IsTrue(new NPath("outer").FileExists());
            }

            mock.VerifyAll();
            mock2.VerifyAll();
        }

        [Test]
        public void ThrowWhenNotCurrentFileSystemDuringDispose()
        {
            var mock1 = new Mock<NPath.FileSystem>(MockBehavior.Strict);
            mock1.Setup(o => o.Dispose());
            var mock2 = new Mock<NPath.FileSystem>(MockBehavior.Strict);

            using (var o1 = NPath.WithFileSystem(mock1.Object))
            {
                using (NPath.WithFileSystem(mock2.Object))
                {
                    Assert.Throws<InvalidOperationException>(() => o1.Dispose());
                }
            }
        }
    }
}
