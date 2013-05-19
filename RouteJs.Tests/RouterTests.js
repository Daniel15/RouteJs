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

	describe('Simple constraints', function() {
		var route;
		
		beforeEach(function() {
			route = new RouteJs.Route({
				url: 'blog/page-{page}',
				defaults: { controller: 'Blog', action: 'Index' },
				constraints: { page: '\\d+' }
			});
		});

		it('should allow valid values', function() {
			var url = route.build({ controller: 'Blog', action: 'Index', page: 2 });
			expect(url).toEqual('blog/page-2');
		});
		
		it('should not allow valid values', function() {
			var url = route.build({ controller: 'Blog', action: 'Index', page: 'foobar' });
			expect(url).toBeNull();
		});
	});
});

describe('RouteManager', function() {
	describe('MVC routes', function() {
		var router;
		
		beforeEach(function() {
			router = new RouteJs.RouteManager({
				routes: [
					{
						url: 'hello/world',
						defaults: { controller: 'Hello', action: 'HelloWorld' }
					},
					{
						url: 'hello/mvc/{message}',
						defaults: { controller: 'Hello', action: 'HelloWorld2' }
					}
				],
				baseUrl: ''
			});
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
	
	describe('MVC routes with default', function() {
		var router;
			
		beforeEach(function() {
			router = new RouteJs.RouteManager({
				routes: [
					{
						url: 'hello/world',
						defaults: { controller: 'Hello', action: 'HelloWorld' }
					},
					{
						url: 'hello/mvc/{message}',
						defaults: { controller: 'Hello', action: 'HelloWorld2' }
					},
					{
						url: 'hellohome/{action}',
						defaults: { controller: 'Home', action: 'Index' }
					},
					{
						url: '{controller}/{action}',
						defaults: { controller: 'Home', action: 'Index' }
					}
				],
				baseUrl: ''
			});
		});

		it('should select the correct route if one exists', function() {
			var url = router.action('Hello', 'HelloWorld2', { message: 'asd' });
			expect(url).toEqual('hello/mvc/asd');
		});

		it('should use the {action} merge field', function() {
			var url = router.action('Home', 'NotInCustom');
			expect(url).toEqual('hellohome/NotInCustom');
		});
		
		it('should select the default route for unknown actions', function() {
			var url = router.action('Hello', 'NotInCustom');
			expect(url).toEqual('Hello/NotInCustom');
		});
	});

	describe('MVC routes with multiple overloads', function() {
		var router;
		
		beforeEach(function() {
			router = new RouteJs.RouteManager({
				routes: [
					{
						url: 'category/{slug}',
						defaults: { controller: 'Blog', action: 'Category', page: 1 }
					},
					{
						url: 'category/{slug}/page-{page}',
						defaults: { controller: 'Blog', action: 'Category' }
					}
				],
				baseUrl: ''
			});
		});

		it('should use the first overload when page number is 1', function() {
			var url = router.action('Blog', 'Category', { slug: 'hello-world', page: 1 });
			expect(url).toEqual('category/hello-world');
		});
		
		it('should use the second overload when page number is 2', function() {
			var url = router.action('Blog', 'Category', { slug: 'hello-world', page: 2 });
			expect(url).toEqual('category/hello-world/page-2');
		});
	});
});