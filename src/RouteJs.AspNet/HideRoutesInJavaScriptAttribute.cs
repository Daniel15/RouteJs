using System;

namespace RouteJs
{
	/// <summary>
	/// Specifies that this controller should not be exposed in the RouteJs routes
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, Inherited = true)]
	public class HideRoutesInJavaScriptAttribute : Attribute
	{
	}
}
