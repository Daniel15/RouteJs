RouteJs [![Build status](http://teamcity.codebetter.com/app/rest/builds/buildType:\(id:bt1035\)/statusIcon)](http://teamcity.codebetter.com/viewType.html?buildTypeId=bt1035&guest=1)
=======
RouteJs allows you to use your ASP.NET MVC or WebForms routes from JavaScript. It does not depend on
jQuery or any other JavaScript framework, so can be used in any scenario. RouteJs works with your
existing MVC routes, you do not need to use a different routing syntax or modify any of your 
existing routes at all.

Bug reports and feature requests are welcome!

Requirements
============
Required:
 * ASP.NET 4.0 or higher
 * [Json.NET](http://james.newtonking.com/projects/json-net.aspx)
 * ASP.NET MVC 2, 3 or 4 (while RouteJs doesn't explicitly require MVC, it's pretty useless without it!)

Installation
============

Via released [NuGet Package](https://nuget.org/packages?q=RouteJs)
----------------------------
```
Install-Package RouteJs.Mvc4
```
(replace `RouteJs.Mvc4` with `RouteJs.Mvc3` for ASP.NET MVC 3 or `RouteJs.Mvc2` for ASP.NET MVC 2)

Now skip down to the [usage section](#usage)

Via latest development build
----------------------------
Get the latest builds from the [build server](http://teamcity.codebetter.com/viewType.html?buildTypeId=bt1035&guest=1)

Now skip down to the [usage section](#usage)

Manual Installation
-------------------

- Compile RouteJs by running `build.bat`
- Reference RouteJs.dll and RouteJs.Mvc4.dll (if using MVC 4) in your Web Application project
- Add the configuration to your Web.config:

```xml
<configSections>
	...
	<section name="routeJs" type="RouteJs.RouteJsConfigurationSection, RouteJs" />
</configSections>
<system.web>
	...
	<httpHandlers>
		<add verb="GET" path="routejs.axd" type="RouteJs.RouteJsHandler, RouteJs" />
	</httpHandlers>
</system.web>

<system.webServer>
	...
	<handlers>
		<add name="RouteJs" verb="GET" path="routejs.axd" type="RouteJs.RouteJsHandler, RouteJs" />
	</handlers>
</system.webServer>

<!--
	Sets whether to expose all routes to the site. 
	If true, all routes will be exposed unless explicitly hidden using the [HideRoutesInJavaScript] 
	attribute on the controller. If false, all routes will be hidden unless explicitly exposed 
	using the [ExposeRoutesInJavaScript] attribute.
-->
<routeJs exposeAllRoutes="true" />
```
- See usage example below

Usage
=====

Firstly, you need to reference the RouteJs handler in your view. This serves the JavaScript
and route information:
```html
<script src="@RouteJs.RouteJsHandler.HandlerUrl"></script>
```

The main function is `Router.action`. This accepts three parameters:
- Name of the controller
- Name of the action
- Any additional parameters

Examples:

```javascript
// Using the default route
var url = Router.action('Controller', 'Action'); // eg. /Controller/Action

// Handling optional parameters
var url = Router.action('Foo', 'Bar', { id: 123 }); // eg. /Foo/Bar/123

// Appending querystring parameters
var url = Router.action('Foo', 'Bar', { hello: 'world' }); // eg. /Foo/Bar?hello=world
```

The routes that are exposed are controlled by the web.config "exposeAllRoutes" setting:
```xml
<routeJs exposeAllRoutes="true" />
```

If set to "true", all of your ASP.NET MVC routes will be exposed to JavaScript, unless you 
explicitly hide them via the `HideRoutesInJavaScript` attribute on a controller. If set to "false", 
all routes will be hidden unless you explicitly use the `ExposeRoutesInJavaScript` attribute on the
controller. These two attributes currently affect all routes for the controller.

Changelog
=========
1.1.4 - 10th September 2013
---------------------------
 - [#22](https://github.com/Daniel15/RouteJs/pull/22) and [#24](https://github.com/Daniel15/RouteJs/issues/24) -
   Handle error when loading types from referenced assemblies.

1.1.3 - 7th August 2013
-----------------------
 - [#21](https://github.com/Daniel15/RouteJs/issues/21) - NullReferenceException thrown on ignored
   routes.

1.1.2 - 6th August 2013
-----------------------
 - [#18](https://github.com/Daniel15/RouteJs/issues/18) - Only expose a route's default area if at 
   least one controller in that route is exposed
 - Small JavaScript cleanup (split huge route method into several smaller methods)

1.1.1 - 26th July 2013
----------------------
 - [#14](https://github.com/Daniel15/RouteJs/issues/14) - Cache JavaScript for one year
 - [#17](https://github.com/Daniel15/RouteJs/issues/17) - Ensure area route isn't used if area is 
   not specified in `Router.action` call

1.1.0 - 10th June 2013
----------------------
 - Added support for ASP.NET MVC 2 and 3
 - Bug fixes around T4MVC routes
 - Changed cachebusting hash from querystring parameter to URL path parameter

1.0.1 - 4th June 2013
---------------------
 - Fixed issue with routes in areas not working correctly

1.0 - 23rd May 2013
-------------------
 - Initial release
 
Licence
=======
(The MIT licence)

Copyright (C) 2013 Daniel Lo Nigro (Daniel15)

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
of the Software, and to permit persons to whom the Software is furnished to do
so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
