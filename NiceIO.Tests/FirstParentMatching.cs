using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace NiceIO.Tests
{
	[TestFixture]
	public class FirstParentMatching : TestWithTempDir
	{
		[Test]
		public void TwoLevelsDown()
		{
			PopulateTempDir(new[] { "somedir/", "somedir/dir2/", "somedir/dir2/myfile", "somedir/needle" });

			Assert.AreEqual(_tempPath.Combine("somedir"), _tempPath.Combine("somedir/dir2/myfile").FirstParentMatching(p => p.FileName == "somedir"));
		}

		[Test]
		public void InRelativePath()
		{
			Assert.Throws<ArgumentException>(() => new NPath("this/is/relative").FirstParentMatching(p => true));
		}
	}
}
