using NUnit.Framework;

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
		public void Default()
		{
			Assert.AreEqual(@"a/b", new NPath("a/b").ToString());
		}

		[Test]
		public void InQuotesDefault()
		{
			Assert.AreEqual(@"""a/b""", new NPath(@"a/b").InQuotes());
		}

		[Test]
		public void InQuotesOnMultiplePaths()
		{
			CollectionAssert.AreEqual(new[] { @"""a/b""", @"""c/d""" }, new[] { new NPath("a/b"), new NPath("c/d"), }.InQuotes(SlashMode.Forward));	
		}
	}
}
