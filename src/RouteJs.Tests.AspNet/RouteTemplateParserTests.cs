using Microsoft.AspNetCore.Routing.Template;
using System.Collections.Generic;
using Xunit;

namespace RouteJs.Tests.AspNet
{
    public class RouteTemplateParserTests
    {
		[Fact]
	    public void HandlesOptional()
		{
			var info = Parse("hello/world/{id?}");
			Assert.Equal("hello/world/{id}", info.Url);
			Assert.Equal(new List<string> { "id" }, info.Optional);
		}

		[Fact]
		public void HandlesDefaults()
		{
			var info = Parse("hello/{action=World}/{id=123}");
			Assert.Equal("hello/{action}/{id}", info.Url);
			Assert.Equal(2, info.Defaults.Count);
			Assert.Equal("World", info.Defaults["action"]);
			Assert.Equal("123", info.Defaults["id"]);
		}

		[Fact]
		public void HandlesMultiplePartsInSegment()
		{
			var info = Parse("controller-{controller=Hello}/action-{action=World}");
			Assert.Equal("controller-{controller}/action-{action}", info.Url);
			Assert.Equal(2, info.Defaults.Count);
			Assert.Equal("Hello", info.Defaults["controller"]);
			Assert.Equal("World", info.Defaults["action"]);
		}

		private RouteInfo Parse(string template)
	    {
			var parser = new RouteTemplateParser();
		    var parsedTemplate = TemplateParser.Parse(template);
			var info = new RouteInfo
			{
				Defaults = new Dictionary<string, object>(),
				Optional = new List<string>(),
			};
			parser.Parse(parsedTemplate, info);
		    return info;
	    }
    }
}
