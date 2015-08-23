using Microsoft.AspNet.Routing;
using Microsoft.Framework.OptionsModel;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace RouteJs.Tests.AspNet
{
	/// <summary>
	/// Unit tests for <see cref="ConstraintsProcessor"/>
	/// </summary>
	public class ConstraintsProcessorTests
    {
		[Theory]
		[InlineData("alpha", "^[a-z]*$")]
		[InlineData("bool", "^(True|False)$")]
		[InlineData("decimal", @"^[+\-]?^\d+\.?\d+$")]
		[InlineData("double", @"^[+\-]?^\d+\.?\d+$")]
		[InlineData("float", @"^[+\-]?^\d+\.?\d+$")]
		[InlineData("guid", "^[0-9a-f]{8}-[0-9a-f]{4}-[1-5][0-9a-f]{3}-[89ab][0-9a-f]{3}-[0-9a-f]{12}$")]
		[InlineData("int", @"^[+\-]?\d+$")]
		[InlineData("long", @"^[+\-]?^\d+\.?\d+$")]
		[InlineData("regex(^[A-Z]+$)", "^[A-Z]+$")]
		public void HandlesRegexConstraints(string inlineConstraint, string expected)
		{
			var routeOptions = new Mock<IOptions<RouteOptions>>();
			routeOptions.Setup(x => x.Options).Returns(new RouteOptions());
			var constraintResolver = new DefaultInlineConstraintResolver(routeOptions.Object);
			var constraint = constraintResolver.ResolveConstraint(inlineConstraint);

			var constraintsProcessor = new ConstraintsProcessor();
			var result = constraintsProcessor.ProcessConstraints(new Dictionary<string, IRouteConstraint>
			{
				{"test", constraint}
			});

			Assert.Equal(1, result.Count);
			Assert.Equal(expected, result["test"]);
		}

		//[InlineData("datetime", "TODO")]
		//[InlineData("length(10)", "TODO")]
		//[InlineData("length(5,10)", "TODO")]
		//[InlineData("max(10)", "TODO")]
		//[InlineData("maxlength(10)", "TODO")]
		//[InlineData("min(10)", "TODO")]
		//[InlineData("minlength(10)", "TODO")]
		//[InlineData("range(10,20)", "TODO")]
	}
}
