///<reference path="jasmine/jasmine.js"/>
///<reference path="router.js"/>

describe('Route', function() {
	describe('Simple MVC routes', function() {
		var route;
		
		beforeEach(function() {
			route = new RouteJs.Route({
				url: 'hello/world',
				defaults: { controller: 'Hello', action: 'HelloWorld' }
			});
		});

		it('should build a URL when the correct controller action is used', function() {
			var url = route.build({ controller: 'Hello', action: 'HelloWorld' });
			expect(url).toEqual('hello/world');
		});

		it('should return null when the controller doesn\'t match', function() {
			var url = route.build({ controller: 'Oops', action: 'HelloWorld' });
			expect(url).toBeNull();
		});
		
		it('should return null when the action doesn\'t match', function() {
			var url = route.build({ controller: 'Hello', action: 'Oops' });
			expect(url).toBeNull();
		});

		it('should add additional parameters to the query string', function() {
			var url = route.build({ controller: 'Hello', action: 'HelloWorld', extra: 'param' });
			expect(url).toEqual('hello/world?extra=param');
		});
	});

	describe('Simple route with parameter', function() {
		var route;

		beforeEach(function() {
			route = new RouteJs.Route({
				url: 'hello/mvc/{message}',
				defaults: { controller: 'Hello', action: 'HelloWorld2' }
			});
		});

		it('should insert the parameter', function() {
			var url = route.build({ controller: 'Hello', action: 'HelloWorld2', message: 'Foobar' });
			expect(url).toEqual('hello/mvc/Foobar');
		});

		it('should return null if the parameter isn\'t provided', function() {
			var url = route.build({ controller: 'Hello', action: 'HelloWorld2' });
			expect(url).toBeNull();
		});
	});
	
	describe('Simple route with parameter and default', function() {
		var route;

		beforeEach(function() {
			route = new RouteJs.Route({
				url: 'hello/mvc/{message}',
				defaults: { controller: 'Hello', action: 'HelloWorld2', message: 'default' }
			});
		});
		
		it('should use the default if the parameter isn\'t provided', function() {
			var url = route.build({ controller: 'Hello', action: 'HelloWorld2' });
			expect(url).toEqual('hello/mvc/default');
		});

		it('should insert the parameter', function() {
			var url = route.build({ controller: 'Hello', action: 'HelloWorld2', message: 'Foobar' });
			expect(url).toEqual('hello/mvc/Foobar');
		});
	});
});

describe('RouteManager', function() {
	describe('MVC routes', function() {
		var router;
		
		beforeEach(function() {
			router = new RouteJs.RouteManager([
				{
					url: 'hello/world',
					defaults: { controller: 'Hello', action: 'HelloWorld' }
				},
				{
					url: 'hello/mvc/{message}',
					defaults: { controller: 'Hello', action: 'HelloWorld2' }
				},
			]);
		});

		it('should select the correct route based on controller and action', function() {
			var url = router.action('Hello', 'HelloWorld');
			expect(url).toEqual('hello/world');
		});

		it('should throw an exception if a matching route doesn\'t exist', function() {
			var buildUrl = function() {
				var url = router.action('Invalid', 'Invalid');
			};
			expect(buildUrl).toThrow();
		});

		it('should handle routes with parameters', function() {
			var url = router.action('Hello', 'HelloWorld2', { message: 'foobar' });
			expect(url).toEqual('hello/mvc/foobar');
		});
	});
});