using System;
using NUnit.Framework;

namespace NiceIO.Tests
{
	[TestFixture]
	public class WriteOperations : TestWithTempDir
	{
		[Test]
		public void WriteAllText()
		{
			Assert.AreEqual("hello there", _tempPath.Combine("mydir/myfile").WriteAllText("hello there").ReadAllText());
		}

		[Test]
		public void WriteAllText_LongPath()
		{
			var longPath = _tempPath.Combine(kLongPath).CreateDirectory();
			Assert.AreEqual("hello there", longPath.Combine("myfile").WriteAllText("hello there").ReadAllText());
		}

		[Test]
		public void WriteAllLines()
		{
			CollectionAssert.AreEqual(new[] { "hello", "there" }, _tempPath.Combine("mydir/myfile").WriteAllLines(new[] { "hello", "there" }).ReadAllLines());
		}

		[Test]
		public void WriteAllLines_LongPath()
		{
			var longPath = _tempPath.Combine(kLongPath).CreateDirectory();
			CollectionAssert.AreEqual(new[] { "hello", "there" }, longPath.Combine("mydir/myfile").WriteAllLines(new[] { "hello", "there" }).ReadAllLines());
		}

		[Test]
		public void WriteAllBytes()
		{
			var bytes = new[] { (byte)'h', (byte)'e', (byte)'l', (byte)'l', (byte)'o' };
			CollectionAssert.AreEqual(bytes, _tempPath.Combine("mydir/myfile").WriteAllBytes(bytes).ReadAllBytes());
		}

		[Test]
		public void WriteAllBytes_LongPath()
		{
			var bytes = new[] { (byte)'h', (byte)'e', (byte)'l', (byte)'l', (byte)'o' };
			var longPath = _tempPath.Combine(kLongPath).CreateDirectory();
			CollectionAssert.AreEqual(bytes, longPath.Combine("mydir/myfile").WriteAllBytes(bytes).ReadAllBytes());
		}

		[Test]
		public void CanSetAndGetLastWriteTime()
		{
			var timestamp = new DateTime(2018, 1, 1, 15, 16, 17, DateTimeKind.Utc);
			Assert.AreEqual(timestamp, _tempPath.Combine("mydir/myfile").WriteAllText("test").SetLastWriteTimeUtc(timestamp).GetLastWriteTimeUtc());
		}

		[Test]
		public void CanSetAndGetLastWriteTime_LongPath()
		{
			var timestamp = new DateTime(2018, 1, 1, 15, 16, 17, DateTimeKind.Utc);
			var longPath = _tempPath.Combine(kLongPath).CreateDirectory();
			Assert.AreEqual(timestamp, longPath.Combine("mydir/myfile").WriteAllText("test").SetLastWriteTimeUtc(timestamp).GetLastWriteTimeUtc());
		}
	}
}
