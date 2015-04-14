using System;
using NUnit.Framework;

namespace NiceIO.Tests
{
	[TestFixture]
	public class WriteOperations : TestWithTempDir
	{
		[Test]
		public void WriteAllText()
		{
			Assert.AreEqual("hello there", _tempPath.Combine("mydir/myfile").WriteAllText("hello there").ReadAllText());
		}

		[Test]
		public void WriteAllLines()
		{
			CollectionAssert.AreEqual(new[] {"hello", "there"}, _tempPath.Combine("mydir/myfile").WriteAllLines(new[] {"hello", "there"}).ReadAllLines());
		}

		[Test]
		public void OnRelativeFile_WriteText()
		{
			Assert.Throws<ArgumentException>(() => new NPath("relative").WriteAllText("hi"));
		}

		[Test]
		public void OnRelativeFile_WriteLines()
		{
			Assert.Throws<ArgumentException>(() => new NPath("relative").WriteAllLines(new[]{"hi"}));
		}

		[Test]
		public void OnRelativeFile_ReadText()
		{
			Assert.Throws<ArgumentException>(() => new NPath("relative").ReadAllText());
		}

		[Test]
		public void OnRelativeFile_ReadLines()
		{
			Assert.Throws<ArgumentException>(() => new NPath("relative").ReadAllLines());
		}
	}
}
