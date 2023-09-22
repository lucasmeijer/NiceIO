using System.IO;
using NUnit.Framework;

namespace NiceIO.Tests
{
	[TestFixture]
	public class CreateTempDirectory
	{
		NPath m_TempDir;

		[Test]
		public void Test()
		{
			const string prefix = "NiceIO-CreateTempDirectory";
			m_TempDir = NPath.CreateTempDirectory(prefix);
			Assert.That(Directory.Exists(m_TempDir.ToString()));
			Assert.That(m_TempDir.IsRelative, Is.False);
			Assert.That(m_TempDir.ToString(), Does.Contain("/" + prefix));
		}

		[TearDown]
		public void TearDown()
		{
			m_TempDir?.DeleteIfExists();
		}
	}
}