using NUnit.Framework;

namespace NiceIO.Tests
{
	[TestFixture]
	public class GetHashCode
	{
		[Test]
		public void DifferentForDifferentPaths()
		{
			var p1 = new NPath("/b/c");
			var p2 = new NPath("c/d");

			Assert.AreNotEqual(p1.GetHashCode(), p2.GetHashCode());
		}
		
		[TestCase("aaa", "aaa")]
		[TestCase("caSE senSItiVE", "CASE sensitive", ExcludePlatform = "Linux")]
		[TestCase("/b/c", "/b\\c\\")]
		public void IdenticalForEqualPaths(string s1, string s2)
		{
			var p1 = new NPath(s1);
			var p2 = new NPath(s2);

			Assume.That(p1, Is.EqualTo(p2));
			Assert.That(p1.GetHashCode(), Is.EqualTo(p2.GetHashCode()));
		}

		[Platform(Exclude = "Linux")]
		[TestCase("ø", "æ")]
		[TestCase("abc-ïïï", "abc-ååå")]
		public void IdenticalForDifferentNonAsciiChars(string s1, string s2)
		{
			var p1 = new NPath(s1);
			var p2 = new NPath(s2);

			Assert.That(p1.GetHashCode(), Is.EqualTo(p2.GetHashCode()));
		}
	}
}
