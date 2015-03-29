using System;
using System.Collections.Generic;
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
		    var split = path.Split('/', '\\');
		    _rooted = split.Length > 0 && split[0].Length == 0;
		    _elements = split.Where(s => s.Length > 0).ToArray();
	    }

	    private Path(string[] elements)
	    {
		    _elements = elements;
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
		    var newElements = _elements.Take(_elements.Length - 1).ToArray();

		    return new Path(newElements);
	    }
    }
}
