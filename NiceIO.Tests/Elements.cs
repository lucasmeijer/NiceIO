using NUnit.Framework;

namespace NiceIO.Tests
{
	[TestFixture]
	public class Elements
	{
		[Test]
		public void Test()
		{
			CollectionAssert.AreEqual(new[] {"my", "path", "to", "somewhere.txt"}, new NPath("/my/path/to/somewhere.txt").Elements);
		}
	}
}
