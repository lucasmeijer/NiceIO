using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace NiceIO.Tests
{
	[TestFixture]
	public class ChangeExtension
	{
		[Test]
		public void WithoutDot()
		{
			var p1 = new Path("/my/path/file.txt");
			var p2 = new Path("/my/path/file.mp4").ChangeExtension("txt");
			Assert.AreEqual(p1, p2);
		}

		[Test]
		public void WithDot()
		{
			var p1 = new Path("/my/path/file.txt");
			var p2 = new Path("/my/path/file.mp4").ChangeExtension(".txt");
			Assert.AreEqual(p1, p2);
		}

		[Test]
		public void OnPathWithMultipleDots()
		{
			var p1 = new Path("/my/path/file.something.txt");
			var p2 = new Path("/my/path/file.something.mp4").ChangeExtension(".txt");
			Assert.AreEqual(p1, p2);
		}
	}
}
