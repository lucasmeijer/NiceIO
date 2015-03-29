using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;

namespace NiceIO
{
    public class Path
    {
	    private readonly string[] _elements;
	    private readonly bool _rooted;

	    public Path(string path)
	    {
		    var split = SplitOnSlashes(path);
		    _rooted = split.Length > 0 && split[0].Length == 0;
		    _elements = split.Where(s => s.Length > 0).ToArray();
	    }

	    private static string[] SplitOnSlashes(string path)
	    {
		    return path.Split('/', '\\');
	    }

	    private Path(string[] elements, bool rooted)
	    {
		    _elements = elements;
		    _rooted = rooted;
	    }

	    public override string ToString()
	    {
		    var sb = new StringBuilder();
		    if (_rooted)
			    sb.Append("/");
		    bool first = true;
		    foreach (var element in _elements)
		    {
			    if (!first)
				    sb.Append("/");
					
			    sb.Append(element);
			    first = false;
		    }
		    return sb.ToString();
	    }

	    public Path Up()
	    {
			if (_elements.Length == 0)
				throw new InvalidOperationException("Up() is called on an empty path");

		    var newElements = _elements.Take(_elements.Length - 1).ToArray();

		    return new Path(newElements, _rooted);
	    }

		public static DeleteOnDisposePath CreateTempDirectory(string myprefix)
		{
			var random = new Random();
			while (true)
			{
				var candidate = new DeleteOnDisposePath(System.IO.Path.GetTempPath() + "/" + myprefix + "_" + random.Next());
				if (!candidate.Exists())
				{
					candidate.CreateDirectory();
					return candidate;
				}
			}
		}

	    public bool Exists()
	    {
		    return FileExists() || DirectoryExists();
	    }

	    public bool DirectoryExists()
	    {
		    return Directory.Exists(ToString());
	    }

	    public bool FileExists()
	    {
		    return File.Exists(ToString());
	    }

		public Path CreateDirectory()
		{
			Directory.CreateDirectory(ToString());
			return this;
		}

		public Path Combine(string append)
		{
			var split = SplitOnSlashes(append);
			var newElements = _elements.Concat(split).ToArray();
			return new Path(newElements, _rooted);
		}
    }

	public class DeleteOnDisposePath : Path, IDisposable
	{
		public DeleteOnDisposePath(string path) : base(path)
		{
		}

		public void Dispose()
		{
			Directory.Delete(ToString(), true);
		}
	}
}
