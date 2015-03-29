using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace NiceIO.Tests
{
	[TestFixture]
    public class Construction
    {
		[Test]
		public void OnlyFileName()
		{
			var path = new Path("myfile.exe");
			Assert.AreEqual("myfile.exe",path.ToString());
		}

		[Test]
		public void FromStringWithForwardSlash()
		{
			var path = new Path("mydir/myfile.exe");
			Assert.AreEqual("mydir/myfile.exe",path.ToString());
		}

		[Test]
		public void FromStringWithBackSlash()
		{
			var path = new Path(@"mydir\myfile.exe");
			Assert.AreEqual("mydir/myfile.exe", path.ToString());
		}

		[Test]
		public void FromRootedString()
		{
			var path = new Path(@"/mydir/myfile.exe");
			Assert.AreEqual("/mydir/myfile.exe", path.ToString());			
		}

		[Test]
		public void FromStringWithWindowsDrive()
		{
			var path = new Path(@"C:\mydir\myfile.exe");
			Assert.AreEqual("C:/mydir/myfile.exe", path.ToString());				
		}

		[Test]
		public void FromStringWithTrailingSlash()
		{
			var path = new Path("/mydir/myotherdir/");
			Assert.AreEqual("/mydir/myotherdir", path.ToString());							
		}
    }
}
