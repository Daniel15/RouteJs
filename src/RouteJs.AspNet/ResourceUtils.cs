using System;
using System.IO;
using System.Reflection;

namespace RouteJs
{
	/// <summary>
	/// Utility methods for dealing with embedded resources.
	/// </summary>
    public static class ResourceUtils
    {
		/// <summary>
		/// Gets the specified resource. Throws an exception if the resource is not found
		/// </summary>
		/// <param name="assembly">Assembly to get resource from</param>
		/// <param name="resourceName">Name of the resource to get</param>
		/// <returns>Contents of the resource</returns>
	    public static string GetResourceScript(this Assembly assembly, string resourceName)
	    {
		    using (var stream = assembly.GetManifestResourceStream(resourceName))
		    {
			    if (stream == null)
			    {
				    throw new Exception("Could not load RouteJs script (" + resourceName + ")");
			    }
			    using (var reader = new StreamReader(stream))
			    {
				    return reader.ReadToEnd();
			    }
		    }
	    }
    }
}
