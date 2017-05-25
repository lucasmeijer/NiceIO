using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace NiceIO.Tests
{
	[TestFixture]
	public class ToString
	{
		[Test]
		public void Forward()
		{
			Assert.AreEqual("a/b", new NPath("a/b").ToString(SlashMode.Forward));
		}
		
		[Test]
		public void Backward()
		{
			Assert.AreEqual(@"a\b", new NPath(@"a/b").ToString(SlashMode.Backward));
		}

		[Test]
		public void InQuotesForward()
		{
			Assert.AreEqual(@"""a/b""", new NPath("a/b").InQuotes(SlashMode.Forward));
		}

		[Test]
		public void InQuotesBackward()
		{
			Assert.AreEqual(@"""a\b""", new NPath(@"a/b").InQuotes(SlashMode.Backward));
		}

		[Test]
		public void InQuotesOnMultiplePaths()
		{
			CollectionAssert.AreEqual(new[] { @"""a/b""", @"""c/d""" }, new[] { new NPath("a/b"), new NPath("c/d"), }.InQuotes(SlashMode.Forward));	
		}

		[Test]
		public void ImplicitStringHasNativeSlashes()
		{
			string expected = "a" + Path.DirectorySeparatorChar + "b";
			string actualfwd = new NPath("a/b");
			string actualbck = new NPath(@"a\b");

			Assert.AreEqual(expected, actualfwd);
			Assert.AreEqual(expected, actualbck);
		}
	}
}
