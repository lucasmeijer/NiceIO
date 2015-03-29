using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace NiceIO.Tests
{
	[TestFixture]
	public class Up
	{
		[Test]
		public void UpFromFile()
		{
			var path = new Path("mydir/myotherdir/myfile.exe").Up();
			Assert.AreEqual("mydir/myotherdir", path.ToString());
		}
	}
}
