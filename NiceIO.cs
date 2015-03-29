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
	    private readonly bool _isRelative;
	    private string _driveLetter;

	    public Path(string path)
	    {
		    path = ParseDriveLetter(path);

		    var split = SplitOnSlashes(path);
		    
			_isRelative = IsRelativeFromSplitString(split);

		    _elements = split.Where(s => s.Length > 0).ToArray();
	    }

	    private string ParseDriveLetter(string path)
	    {
		    if (path.Length >= 2 && path[1] == ':')
		    {
			    _driveLetter = path[0].ToString();
			    return path.Substring(2);
		    }
		    return path;
	    }

	    private static bool IsRelativeFromSplitString(IEnumerable<string> split)
	    {
		    if (!split.Any())
			    return false;

			//did the string start with a slash? -> rooted
		    return split.First().Length != 0;
	    }

	    public bool IsRelative{ get { return _isRelative;}}

	    private static string[] SplitOnSlashes(string path)
	    {
		    return path.Split('/', '\\');
	    }

	    private Path(string[] elements, bool isRelative)
	    {
		    _elements = elements;
		    _isRelative = isRelative;
	    }

	    public override string ToString()
	    {
		    var sb = new StringBuilder();
		    if (_driveLetter != null)
		    {
			    sb.Append(_driveLetter);
			    sb.Append(":");
		    }
		    if (!_isRelative)
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

		    return new Path(newElements, _isRelative);
	    }

		public static Path CreateTempDirectory(string myprefix)
		{
			var random = new Random();
			while (true)
			{
				var candidate = new Path(System.IO.Path.GetTempPath() + "/" + myprefix + "_" + random.Next());
				if (!candidate.Exists())
					return candidate.CreateDirectory();
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
			return new Path(newElements, _isRelative);
		}

	    public void Delete(DeleteMode deleteMode = DeleteMode.Normal)
	    {
		    ThrowIfRelative();

		    if (FileExists())
				File.Delete(ToString());
		    else if (DirectoryExists())
			    try
			    {
				    Directory.Delete(ToString(), true);
			    }
			    catch (IOException)
			    {
				    if (deleteMode == DeleteMode.Normal)
					    throw;
			    }
		    else
				throw new InvalidOperationException("Trying to delete a path that does not exist: "+ToString());
	    }

	    private void ThrowIfRelative()
	    {
		    if (_isRelative)
				throw new InvalidOperationException("You are attempting an operation on a Path that requires an absolute path, but the path is relative");
	    }

	    public Path CreateFile()
	    {
		    EnsureDirectoryExists(Up());
		    File.WriteAllBytes(ToString(), new byte[0]);
		    return this;
	    }

	    private void EnsureDirectoryExists(Path directory)
	    {
		    if (directory.DirectoryExists())
			    return;
		    EnsureDirectoryExists(directory.Up());
		    directory.CreateDirectory();
	    }
    }

	public enum DeleteMode
	{
		Normal,
		Soft
	}
}
