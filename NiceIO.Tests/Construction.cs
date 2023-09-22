using System;
using NUnit.Framework;

namespace NiceIO.Tests
{
    [TestFixture]
    public class Construction
    {
        [Test]
        public void OnlyFileName()
        {
            var path = new NPath("myfile.exe");
            Assert.AreEqual("myfile.exe", path.ToString());
        }

        [Test]
        public void FromStringWithForwardSlash()
        {
            var path = new NPath("mydir/myfile.exe");
            Assert.AreEqual("mydir/myfile.exe", path.ToString());
        }

        [Test]
        public void FromStringWithBackSlash()
        {
            var path = new NPath(@"mydir\myfile.exe");
            Assert.AreEqual("mydir/myfile.exe", path.ToString());
        }

        [Test]
        public void FromRootedString()
        {
            var path = new NPath(@"/mydir/myfile.exe");
            Assert.AreEqual("/mydir/myfile.exe", path.ToString());
        }

        [Test]
        public void FromSlash()
        {
            var path = new NPath("/");
            Assert.AreEqual("/", path.ToString());
        }

        [Test]
        public void FromStringWithWindowsDrive()
        {
            var path = new NPath(@"C:\mydir\myfile.exe");
            Assert.AreEqual("C:/mydir/myfile.exe", path.ToString());
            Assert.IsFalse(path.IsRelative);
        }

        [Test]
        public void FromStringWithWindowsuNCServer()
        {
            var path = new NPath(@"\\MyWindowsPC\mydir\myfile.exe");
            Assert.AreEqual(@"\\MyWindowsPC/mydir/myfile.exe", path.ToString());
            Assert.IsFalse(path.IsRelative);
        }

        [Test]
        public void FromStringWithTrailingSlash()
        {
            var path = new NPath("/mydir/myotherdir/");
            Assert.AreEqual("/mydir/myotherdir", path.ToString());
            Assert.AreEqual("myotherdir", path.FileName);
        }

        [Test]
        public void FromStringWithMultipleSlashes()
        {
            var path = new NPath("///mydir////myfile.txt");
            Assert.AreEqual("/mydir/myfile.txt", path.ToString());
        }

        [Test]
        public void WithNullArgument()
        {
            Assert.Throws<ArgumentNullException>(() => new NPath(null));
        }

        [Test]
        public void WithDotInPath()
        {
            var path = new NPath("/mydir/./myfile.txt");
            Assert.AreEqual("/mydir/myfile.txt", path.ToString());
        }

        [Test]
        public void WithDotDotInPath()
        {
            var path = new NPath("/mydir/../myotherdir/myfile.txt");
            Assert.AreEqual("/myotherdir/myfile.txt", path.ToString());
        }

        [Test]
        public void WithDotInStartOfRelativePath()
        {
            var path = new NPath("./myotherdir/myfile.txt");
            Assert.AreEqual("myotherdir/myfile.txt", path.ToString());
        }

        [Test]
        public void EndWithSingleDot()
        {
            var path = new NPath("myotherdir/myfile/.");
            Assert.AreEqual("myotherdir/myfile", path.ToString());
        }

        [Test]
        public void FilenameEndingInSingleDot()
        {
            var path = new NPath("myotherdir/myfile.");
            Assert.AreEqual("myotherdir/myfile.", path.ToString());
        }

        [Test]
        public void WithDotDotInStartOfRelativePath()
        {
            var path = new NPath("../../myotherdir/myfile.txt");
            Assert.AreEqual("../../myotherdir/myfile.txt", path.ToString());
        }

        [Test]
        public void WithDotDotInStartOfAbsolutePath()
        {
            Assert.Throws<ArgumentException>(() => new NPath("/../../myotherdir/myfile.txt"));
        }

        [Test]
        public void WithDotDotInStartOfAbsolutePathWithDriveLetter()
        {
            Assert.Throws<ArgumentException>(() => new NPath("C:/../../myotherdir/myfile.txt"));
        }

        [Test]
        public void WithDotDotInStartOfAbsolutePathWithUNCServerName()
        {
            Assert.Throws<ArgumentException>(() => new NPath("\\\\MyWindowsPC/../../myotherdir/myfile.txt"));
        }

        [Test]
        public void WithTooManyDoubleDotsInRootedPathWithDriveLetter()
        {
            Assert.Throws<ArgumentException>(() => new NPath("C:/looks/like/fine/../../../../but_is_not"));
        }

        [Test]
        public void WithTooManyDoubleDotsInRootedPathWithUNCServerName()
        {
            Assert.Throws<ArgumentException>(() => new NPath("\\\\MyWindowsPC/looks/like/fine/../../../../but_is_not"));
        }

        [Test]
        public void WithEmptyString()
        {
            Assert.AreEqual(".", new NPath("").ToString());
        }

        [Test]
        public void CurrentDirectoryAndEmptyPathAreEqual()
        {
            Assert.AreEqual(new NPath("."), new NPath(""));
        }

        [Test]
        public void LinuxRootDirectory()
        {
            Assert.AreEqual("/", new NPath("/").ToString());
        }

        [Test]
        public void LinuxRootDirectoryIsNotRelative()
        {
            Assert.IsFalse(new NPath("/").IsRelative);
        }

        [Test]
        public void WindowsRootDirectory()
        {
            Assert.AreEqual("C:\\", new NPath("C:\\").ToString(SlashMode.Backward));
        }

        [Test]
        public void WindowsRootDirectoryIsNotRelative()
        {
            Assert.IsFalse(new NPath("C:\\").IsRelative);
        }

        [Test]
        public void WindowsUNCRootDirectory()
        {
            Assert.AreEqual("\\\\MyWindowsPC\\", new NPath("\\\\MyWindowsPC\\").ToString(SlashMode.Backward));
        }

        [Test]
        public void WindowsUNCRootDirectoryIsNotRelative()
        {
            Assert.IsFalse(new NPath("\\\\MyWindowsPC\\").IsRelative);
        }

        [Test]
        public void WindowsUNCRootDirectoryWithMissingPathDelimiter()
        {
            Assert.AreEqual("\\\\MyWindowsPC\\", new NPath("\\\\MyWindowsPC").ToString(SlashMode.Backward));
        }

        [Test]
        public void ConstructionOfAbsolutePathWithDotDotsWindowsStyle()
        {
            var value = new NPath("c:\\this\\is\\so\\absolute\\..\\..\\yet_can_have_dots");
            Assert.AreEqual(3, value.Depth);
            Assert.AreEqual("c:\\this\\is\\yet_can_have_dots", value.ToString(SlashMode.Backward));
        }

        [Test]
        public void ConstructionOfAbsolutePathWithDotDotsLinuxStyle()
        {
            var value = new NPath("/this/is/so/absolute/../../yet_can_have_dots");
            Assert.AreEqual(3, value.Depth);
            Assert.AreEqual("/this/is/yet_can_have_dots", value.ToString());
        }

        [Test]
        public void ConstructionOfAbsolutePathWithDotDotsWindowsUNCStyle()
        {
            var value = new NPath("\\\\MyWindowsPC\\this\\is\\so\\absolute\\..\\..\\yet_can_have_dots");
            Assert.AreEqual(3, value.Depth);
            Assert.AreEqual("\\\\MyWindowsPC\\this\\is\\yet_can_have_dots", value.ToString(SlashMode.Backward));
        }

        [Test]
        public void WindowsRootToString()
        {
            Assert.AreEqual("C:/", new NPath("C:\\").ToString());
        }

        [Test]
        public void WindowsUNCRootToString()
        {
            Assert.AreEqual("\\\\MyWindowsPC/", new NPath("\\\\MyWindowsPC\\").ToString());
        }

        [Test]
        public void DoubleDot()
        {
            Assert.AreEqual("..", new NPath("..").ToString());
        }

        [Test]
        public void EndWithDoubleDot()
        {
            Assert.AreEqual("hello", new NPath("hello/there/..").ToString());
        }

        [Test]
        public void WithExtraSlashes()
        {
            Assert.AreEqual("a/b/c/d", new NPath("a//b////c/d/").ToString());
        }

        [Test]
        public void SingleDots()
        {
            Assert.AreEqual("a/b/c/d/e", new NPath("a/./b/./c/./d/e").ToString());
        }

        [Test]
        public void StringCastOperator()
        {
            const string stringValue = "/this_is_a_string";
            NPath path = stringValue;
            Assert.AreEqual(stringValue, path.ToString());
        }

        [Test]
        public void StringCastOperatorWithANullValue()
        {
            const string stringValue = null;
            NPath path = stringValue;
            Assert.IsNull(path);
        }
    }
}
