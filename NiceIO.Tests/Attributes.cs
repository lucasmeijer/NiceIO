using System;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace NiceIO.Tests
{
    [TestFixture]
    class Attributes : TestWithTempDir
    {
        private NPath TestFile;
        private NPath TestFileLongPath;

        [SetUp]
        public void CreateTestFiles()
        {
            TestFile = _tempPath.Combine("TestFile").MakeAbsolute().CreateFile();
            TestFileLongPath = _tempPath.Combine(string.Concat(Enumerable.Repeat("subdirectorywithalongname/", 16))).CreateDirectory().Combine("TestFile").CreateFile();
        }

        [TearDown]
        public void CleanUpTestFiles()
        {
            if (TestFile.FileExists())
                TestFile.Attributes = FileAttributes.Normal;
            if (TestFileLongPath.FileExists())
                TestFileLongPath.Attributes = FileAttributes.Normal;
        }

        [Test]
        [TestCase(FileAttributes.ReadOnly)]
        [TestCase(FileAttributes.Hidden, IncludePlatform="Win")]
        public void CanRoundtripAttribute(FileAttributes attribute)
        {
            TestFile.Attributes = attribute;
            Assert.That(TestFile.Attributes & attribute, Is.EqualTo(attribute));

            TestFileLongPath.Attributes = attribute;
            Assert.That(TestFileLongPath.Attributes & attribute, Is.EqualTo(attribute));
        }
        
        [Test, Platform (Exclude="Win")]
        [TestCase(FileAttributes.Hidden)]
        public void AttributeIsNotSupportedOnNonWindowsPlatforms(FileAttributes attribute)
        {
            Assert.That(() => TestFile.Attributes = attribute, Throws.InstanceOf<NotSupportedException>());
            Assert.That(() => TestFileLongPath.Attributes = attribute, Throws.InstanceOf<NotSupportedException>());
        }

        [Test]
        public void GetAttributes_OnNonExistingFile_ThrowsFileNotFoundException()
        {
            var file = _tempPath.Combine("NonExistingFile").MakeAbsolute();
            Assume.That(!file.FileExists());

            Assert.That(() => file.Attributes, Throws.InstanceOf<FileNotFoundException>());
        }
    }
}
