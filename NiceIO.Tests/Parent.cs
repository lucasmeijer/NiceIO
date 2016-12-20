using System;
using System.Linq;
using NUnit.Framework;

namespace NiceIO.Tests
{
	[TestFixture]
	public class Parent
	{
		[Test]
		public void ParentFromFileInDirectory()
		{
			var path = new NPath("mydir/myotherdir/myfile.exe").Parent;
			Assert.AreEqual("mydir/myotherdir", path.ToString(SlashMode.Forward));
		}

		[Test]
		public void ParentFromFile()
		{
			var path = new NPath("myfile.exe").Parent;
			Assert.AreEqual(".", path.ToString());
		}

		[Test]
		public void ParentFromFileInRoot()
		{
			var path = new NPath("/myfile.exe").Parent;
			Assert.AreEqual("/", path.ToString(SlashMode.Forward));
		}

		[Test]
		public void ParentFromEmpty()
		{
			Assert.Throws<InvalidOperationException>(() => { var p = new NPath ("/").Parent; });
		}

		[Test]
		public void RecursiveParentsStartFromFile()
		{
			var path = NPath.CurrentDirectory.Combine("mydir/myotherdir/myfile.exe");

			var result = path.RecursiveParents.ToArray();

			Assert.That(result[0], Is.EqualTo(path.Parent));
			Assert.That(result[1], Is.EqualTo(path.Parent.Parent));

			// We can only assert up to the root directory we started at.  After that we don't really know what to expect
			Assert.That(result[2], Is.EqualTo(NPath.CurrentDirectory));
		}

		[Test]
		[Platform(Include = "Win")]
		public void RecursiveParentsStartFromFileAllTheWayToRoot()
		{
			var path = new NPath("C:\\mydir\\myotherdir\\myfile.exe");

			Assert.That(path.RecursiveParents, Is.EqualTo(new[] { path.Parent, path.Parent.Parent, path.Parent.Parent.Parent }));
		}

		[Test]
		[Platform(Include = "Win")]
		public void RecursiveParentsStartFromRoot()
		{
			var path = new NPath("C:\\");

			Assert.That(path.RecursiveParents, Is.EqualTo(new NPath[] {}));
		}

		[Test]
		public void RecursiveParentsStartFromDirectory()
		{
			var path = NPath.CurrentDirectory.Combine("mydir/myotherdir/mydir/");

			var result = path.RecursiveParents.ToArray();

			Assert.That(result[0], Is.EqualTo(path.Parent));
			Assert.That(result[1], Is.EqualTo(path.Parent.Parent));

			// We can only assert up to the root directory we started at.  After that we don't really know what to expect
			Assert.That(result[2], Is.EqualTo(NPath.CurrentDirectory));
		}
	}
}