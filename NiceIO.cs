using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NiceIO
{
	public class Path
	{
		private readonly string[] _elements;
		private readonly bool _isRelative;
		private readonly string _driveLetter;

#region construction
		
		public Path(string path)
		{
			if (path==null)
				throw new ArgumentNullException();

			path = ParseDriveLetter(path, out _driveLetter);

			var split = path.Split('/', '\\');

			_isRelative = IsRelativeFromSplitString(split);

			_elements = ParseSplitStringIntoElements(split.Where(s => s.Length > 0).ToArray());
		}

		private string[] ParseSplitStringIntoElements(IEnumerable<string> inputs)
		{
			var stack = new List<string>();

			foreach (var input in inputs.Where(input => input.Length != 0))
			{
				if (input == "..")
				{
					if (HasNonDotDotLastElement(stack))
					{
						stack.RemoveAt(stack.Count - 1);
						continue;
					}
					if (!_isRelative)
						throw new ArgumentException("You cannot create a path that tries to .. past the root");
				}
				stack.Add(input);
			}
			return stack.ToArray();
		}

		private static bool HasNonDotDotLastElement(List<string> stack)
		{
			return stack.Count > 0 && stack[stack.Count-1] != "..";
		}

		private string ParseDriveLetter(string path, out string driveLetter)
		{
			if (path.Length >= 2 && path[1] == ':')
			{
				driveLetter = path[0].ToString();
				return path.Substring(2);
			}
			
			driveLetter = null;
			return path;
		}

		private static bool IsRelativeFromSplitString(IEnumerable<string> split)
		{
			if (!split.Any())
				return false;

			//did the string start with a slash? -> rooted
			return split.First().Length != 0;
		}

		private Path(string[] elements, bool isRelative, string driveLetter)
		{
			_elements = elements;
			_isRelative = isRelative;
			_driveLetter = driveLetter;
		}

		public Path Combine(string append)
		{
			return Combine(new Path(append));
		}

		public Path Combine(Path append)
		{
			if (!append.IsRelative)
				throw new ArgumentException("You cannot .Combine a non-relative path");

			return new Path(ParseSplitStringIntoElements(_elements.Concat(append._elements)), _isRelative, _driveLetter);
		}

		public Path Parent()
		{
			if (_elements.Length == 0)
				throw new InvalidOperationException("Parent() is called on an empty path");

			var newElements = _elements.Take(_elements.Length - 1).ToArray();

			return new Path(newElements, _isRelative, _driveLetter);
		}

		public Path RelativeTo(Path path)
		{
			if (!IsBelowOrEqual(path))
				throw new ArgumentException("Path.RelativeTo() was invoked with two paths that are unrelated. invoked on: " + ToString() + " asked to be made relative to: " + path);

			return new Path(_elements.Skip(path._elements.Length).ToArray(), true, null);
		}

		public Path ChangeExtension(string extension)
		{
			var newElements = (string[])_elements.Clone();
			newElements[newElements.Length - 1] = System.IO.Path.ChangeExtension(_elements[_elements.Length - 1], WithDot(extension));
			return new Path(newElements, _isRelative, _driveLetter);
		}
#endregion construction

#region inspection

		public bool IsRelative
		{
			get { return _isRelative; }
		}

		public string FileName
		{
			get { return _elements.Last(); }
		}

		public IEnumerable<string> Elements
		{
			get { return _elements; }
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

		public string ExtensionWithDot
		{
			get
			{
				var last = _elements.Last();
				var index = last.LastIndexOf(".");
				if (index < 0) return String.Empty;
				return last.Substring(index);
			}
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
			var first = true;
			foreach (var element in _elements)
			{
				if (!first)
					sb.Append("/");

				sb.Append(element);
				first = false;
			}
			return sb.ToString();
		}

		public override bool Equals(Object obj)
		{
			if (obj == null)
				return false;

			// If parameter cannot be cast to Point return false.
			var p = obj as Path;
			if ((Object)p == null)
				return false;

			if (p._isRelative != _isRelative)
				return false;
			if (p._driveLetter != _driveLetter)
				return false;

			if (p._elements.Length != _elements.Length)
				return false;

			for (var i = 0; i != _elements.Length; i++)
				if (p._elements[i] != _elements[i])
					return false;

			return true;
		}

		public static bool operator ==(Path a, Path b)
		{
			// If both are null, or both are same instance, return true.
			if (ReferenceEquals(a, b))
				return true;

			// If one is null, but not both, return false.
			if (((object)a == null) || ((object)b == null))
				return false;

			// Return true if the fields match:
			return a.Equals(b);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hash = 17;
				// Suitable nullity checks etc, of course :)
				hash = hash * 23 + _isRelative.GetHashCode();
				hash = hash * 23 + _elements.GetHashCode();
				hash = hash * 23 + _driveLetter.GetHashCode();
				return hash;
			}
		}

		public static bool operator !=(Path a, Path b)
		{
			return !(a == b);
		}

		public bool HasExtension(params string[] extensions)
		{
			return extensions.Any(e => WithDot(e) == ExtensionWithDot);
		}

		private static string WithDot(string extension)
		{
			return extension.StartsWith(".") ? extension : "." + extension;
		}

		private bool IsEmpty()
		{
			return _elements.Length == 0;
		}
#endregion inspection	

#region directory enumeration

		public IEnumerable<Path> Files(SearchOption searchOption = SearchOption.TopDirectoryOnly)
		{
			return Directory.GetFiles(ToString(), "*", searchOption).Select(s => new Path(s));
		}

		public IEnumerable<Path> Contents(SearchOption searchOption = SearchOption.TopDirectoryOnly)
		{
			return Files(searchOption).Concat(Directories(searchOption));
		}

		public IEnumerable<Path> Directories(SearchOption searchOption = SearchOption.TopDirectoryOnly)
		{
			return Directory.GetDirectories(ToString(), "*", searchOption).Select(s => new Path(s));
		}

#endregion

#region filesystem writing operations
		public Path CreateFile()
		{
			ThrowIfRelative();
			EnsureDirectoryExists(Parent());
			File.WriteAllBytes(ToString(), new byte[0]);
			return this;
		}

		public Path CreateFile(string file)
		{
			return CreateFile(new Path(file));
		}

		public Path CreateFile(Path file)
		{
			if (!file.IsRelative)
				throw new ArgumentException("You cannot call CreateFile() on an existing path with a non relative argument");
			return Combine(file).CreateFile();
		}

		public Path CreateDirectory()
		{
			ThrowIfRelative();
			Directory.CreateDirectory(ToString());
			return this;
		}

		public Path CreateDirectory(string directory)
		{
			return CreateDirectory(new Path(directory));
		}

		public Path CreateDirectory(Path directory)
		{
			if (!directory.IsRelative)
				throw new ArgumentException("Cannot call CreateDirectory with an absolute argument");

			return Combine(directory).CreateDirectory();
		}

		public Path Copy(string dest)
		{
			return Copy(new Path(dest));
		}

		public Path Copy(string dest, Func<Path,bool> fileFilter )
		{
			return Copy(new Path(dest), fileFilter);
		}

		public Path Copy(Path dest)
		{
			return Copy(dest,p => true);
		}

		public Path Copy(Path dest, Func<Path, bool> fileFilter)
		{
			ThrowIfRelative();
			if (dest.IsRelative)
				dest = Parent().Combine(dest);

			if (FileExists())
			{
				if (!fileFilter(dest))
					return null;

				EnsureDirectoryExists(dest.Parent());
				File.Copy(ToString(), dest.ToString(), true);
				return dest;
			}
			
			if (DirectoryExists())
			{
				EnsureDirectoryExists(dest);
				foreach (var thing in Contents())
					thing.Copy(dest.Combine(thing.RelativeTo(this)),fileFilter);
				return dest;
			}
			
			throw new ArgumentException("Copy() called on path that doesnt exist: " + ToString());
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
				throw new InvalidOperationException("Trying to delete a path that does not exist: " + ToString());
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

		public Path Move(string dest)
		{
			return Move(new Path(dest));
		}

		public Path Move(Path dest)
		{
			ThrowIfRelative();
			if (dest.IsRelative)
				return Move(Parent().Combine(dest));

			if (FileExists())
			{
				EnsureDirectoryExists(dest.Parent());
				File.Move(ToString(), dest.ToString());
				return dest;
			}

			if (DirectoryExists())
			{
				Directory.Move(ToString(), dest.ToString());
				return dest;
			}

			throw new ArgumentException("Move() called on a path that doesn't exist: "+ToString());
		}

		#endregion

		#region special paths

		public static Path CurrentDirectory
		{
			get
			{
				return new Path(Directory.GetCurrentDirectory());
			}
		}

		public static Path HomeDirectory
		{
			get
			{
				if (System.IO.Path.DirectorySeparatorChar=='\\')
					return new Path(Environment.GetEnvironmentVariable("USERPROFILE"));
				return new Path (Environment.GetFolderPath(Environment.SpecialFolder.Personal));
			}
		}

		#endregion

		private void ThrowIfRelative()
		{
			if (_isRelative)
				throw new ArgumentException("You are attempting an operation on a Path that requires an absolute path, but the path is relative");
		}

		private void EnsureDirectoryExists(Path directory)
		{
			if (directory.DirectoryExists())
				return;
			EnsureDirectoryExists(directory.Parent());
			directory.CreateDirectory();
		}

		private bool IsBelowOrEqual(Path potentialBasePath)
		{
			if (IsEmpty())
				return false;

			if (Equals(potentialBasePath))
				return true;

			return Parent().IsBelowOrEqual(potentialBasePath);
		}
	}

	public enum DeleteMode
	{
		Normal,
		Soft
	}
}