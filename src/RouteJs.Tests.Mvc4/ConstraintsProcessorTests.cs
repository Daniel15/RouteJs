using System.Web.Routing;
using Moq;
using NUnit.Framework;

namespace RouteJs.Tests.Mvc
{
	[TestFixture]
	public class ConstraintsProcessorTests
	{
		[Test]
		public void HandlesRoutesWithNoConstraints()
		{
			var constraints = new RouteValueDictionary();
			var routeInfo = new RouteInfo { Constraints = new RouteValueDictionary() };

			var processor = new ConstraintsProcessor();
			processor.ProcessConstraints(constraints, routeInfo);
			Assert.AreEqual(0, routeInfo.Constraints.Count);
		}

		[Test]
		public void IncludesRegExpConstraints()
		{
			var constraints = new RouteValueDictionary {{"id", @"\d+"}};
			var routeInfo = new RouteInfo { Constraints = new RouteValueDictionary() };

			var processor = new ConstraintsProcessor();
			processor.ProcessConstraints(constraints, routeInfo);
			Assert.AreEqual(1, routeInfo.Constraints.Count);
			Assert.AreEqual(@"\d+", routeInfo.Constraints["id"]);
		}

		[Test]
		public void ExcludesCustomConstraints()
		{
			var customConstraint = new Mock<IRouteConstraint>();
			var constraints = new RouteValueDictionary {
				{ "id", @"\d+" },
				{ "awesome", customConstraint },
			};
			var routeInfo = new RouteInfo { Constraints = new RouteValueDictionary() };

			var processor = new ConstraintsProcessor();
			processor.ProcessConstraints(constraints, routeInfo);
			Assert.AreEqual(1, routeInfo.Constraints.Count);
		}
	}
}