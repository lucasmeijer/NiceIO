using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using NUnit.Framework;

namespace NiceIO.Tests
{
	[TestFixture]
	public class ParentContaining : TestWithTempDir
	{
		[Test]
		public void TwoLevelsDown()
		{
			PopulateTempDir(new[] { "somedir/", "somedir/dir2/", "somedir/dir2/myfile", "somedir/needle" });

			Assert.AreEqual(_tempPath.Combine("somedir"), _tempPath.Combine("somedir/dir2/myfile").ParentContaining("needle"));
		}

		[Test]
		public void NonExisting()
		{
			PopulateTempDir(new[] { "somedir/", "somedir/dir2/", "somedir/dir2/myfile" });

			var nonExistingParent = _tempPath.Combine("somedir/dir2/myfile").ParentContaining("nonexisting");
			Assert.IsFalse(nonExistingParent.IsInitialized);
			Assert.AreEqual(NPath.Default, nonExistingParent);
		}

		[Test]
		public void WithComplexNeedle()
		{
			PopulateTempDir(new[] { "somedir/", "somedir/dir2/", "somedir/dir2/myfile", "needledir/", "needledir/needlefile" });

			Assert.AreEqual(_tempPath, _tempPath.Combine("somedir/dir2/myfile").ParentContaining(new NPath("needledir/needlefile")));
		}

		[Test]
		public void InRelativePath()
		{
			Assert.Throws<ArgumentException>(() => new NPath("this/is/relative").ParentContaining("needle"));
		}
	}
}
