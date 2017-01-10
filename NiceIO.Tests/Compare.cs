using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace NiceIO.Tests
{
	[TestFixture]
	public class Compare
	{
		[Test]
		public void FullPathToSameFullPath()
		{
			var path1 = NPath.CurrentDirectory.Combine("myfile.txt");
			var path2 = NPath.CurrentDirectory.Combine("myfile.txt");

			Assert.That(path1.CompareTo(path2), Is.EqualTo(0));
		}

		[Test]
		public void FullPathToNull()
		{
			var path1 = NPath.CurrentDirectory.Combine("myfile.txt");

			Assert.That(path1.CompareTo(null), Is.EqualTo(-1));
		}

		[Test]
		[Platform(Include = "Win")]
		public void FullPathToSameFullPath_DifferentSlashes()
		{
			var path1 = NPath.CurrentDirectory.Combine("myfile.txt");
			var path2 = new NPath(NPath.CurrentDirectory.Combine("myfile.txt").ToString().Replace('\\', '/'));

			Assert.That(path1.CompareTo(path2), Is.EqualTo(0));
		}

		[Test]
		public void FullPathToRelativePathAreNotEqual()
		{
			var path1 = NPath.CurrentDirectory.Combine("myfile.txt");
			var path2 = new NPath("myfile.txt");

			Assert.That(path1.CompareTo(path2), Is.EqualTo(-1));
		}

		[Test]
		public void RelativeToFullPathAreNotEqual()
		{
			var path1 = new NPath("myfile.txt");
			var path2 = NPath.CurrentDirectory.Combine("myfile.txt");

			Assert.That(path1.CompareTo(path2), Is.EqualTo(1));
		}

		[Test]
		public void FullPathToDifferentFullPath()
		{
			var path1 = NPath.CurrentDirectory.Combine("myfile.txt");
			var path2 = NPath.CurrentDirectory.Combine("otherfile.txt");

			Assert.That(path1.CompareTo(path2), Is.EqualTo(-1));
		}
	}
}
