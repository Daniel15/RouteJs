using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNet.Mvc;
using Moq;
using Xunit;
using Xunit.Sdk;

namespace RouteJs.Tests.AspNet
{
    public class DefaultRouteFilterTests
    {
		[Theory]
		// Hidden controller is always hidden
		[InlineData(typeof(HiddenController), true, false)]
		[InlineData(typeof(HiddenController), false, false)]
		// Visible controller is always visible
		[InlineData(typeof(VisibleController), true, true)]
		[InlineData(typeof(VisibleController), false, true)]
		// Undecorated controller depends on config
		[InlineData(typeof(UndecoratedController), true, true)]
		[InlineData(typeof(UndecoratedController), false, false)]
		public void VisiblityCanBeChangedViaAttribute(Type controllerType, bool exposeAllRoutes, bool expected)
		{
			var controllerName = controllerType.Name.Replace("Controller", string.Empty);
			var config = new RouteJsConfiguration { ExposeAllRoutes = exposeAllRoutes };

			var descriptor = new ControllerActionDescriptor
			{
				ControllerName = controllerName,
				ControllerTypeInfo = controllerType.GetTypeInfo(),
			};

			var descriptorProvider = new Mock<IActionDescriptorsCollectionProvider>();
			descriptorProvider.Setup(x => x.ActionDescriptors).Returns(new ActionDescriptorsCollection(
				new List<ActionDescriptor> { descriptor },
				version: 1
			));

			var filter = new DefaultRouteFilter(config, descriptorProvider.Object);
			var result = filter.AllowRoute(new RouteInfo
			{
				Defaults = new Dictionary<string, object>
				{
					{"controller", controllerName},
				},
			});
			Assert.Equal(expected, result);
	    }

		[HideRoutesInJavaScript]
	    private class HiddenController : Controller
	    {
	    }

		[ExposeRoutesInJavaScript]
	    public class VisibleController : Controller
	    {
	    }

	    public class UndecoratedController : Controller
	    {
	    }
    }
}
