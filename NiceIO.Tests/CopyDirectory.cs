using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace NiceIO.Tests
{
	[TestFixture]
	public class CopyDirectory : TestWithTempDir
	{
		[Test]
		public void InsideSameDirectory()
		{
			PopulateTempDir(new [] { "somedir/","somedir/myfile"});

			_tempPath.Combine("somedir").Copy(_tempPath.Combine("somedir2"));

			AssertTempDir(new[]
			{
				"somedir/",
				"somedir/myfile",
				"somedir2/",
				"somedir2/myfile"
			});
		}
	}
}
