using System;

namespace RouteJs
{
	/// <summary>
	/// Specifies that this controller should be exposed in the RouteJs routes.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, Inherited = true)]
	public class ExposeRoutesInJavaScriptAttribute : Attribute
	{
	}
}
