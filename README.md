RouteJs
=======
RouteJs allows you to use your ASP.NET MVC or WebForms routes from JavaScript. It does not depend on
jQuery or any other JavaScript framework, so can be used in any scenario. RouteJs works with your
existing MVC routes, you do not need to use a different routing syntax or modify any of your 
existing routes at all.

It is currently under development. Please feel free to report any bugs you encounter!

Requirements
============
Required:
 * ASP.NET 4.0 or higher
 * [Json.NET](http://james.newtonking.com/projects/json-net.aspx)

Optional:
 * ASP.NET MVC 4

Manual Installation
===================

Once this project is more complete, a NuGet package will be created. For now, the installation is
manual:

- Compile RouteJs in Visual Studio 2010 or higher or via `msbuild RouteJs\RouteJs.csproj /p:Configuration=Release`
- Reference RouteJs.dll in your Web Application project
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
- Reference the RouteJs handler in your view:

```html
<script src="@RouteJs.RouteJsHandler.HandlerUrl"></script>
```
- See usage example below

Usage
=====

The main function is `Router.action`. This accepts three parameters:
- Name of the controller
- Name of the action
- Any additional parameters

Examples:

```javascript
var url = Router.action('Controller', 'Action'); 
// url will be '/Controller/Action' with the default route
```

TODO
====
- Add feature to only output certain routes (not all)
- Support optional ASP.NET MVC URLs (UrlParameter.Optional)
- Minify in production
- Create NuGet package
- Support WebForms routes
- Support ASP.NET MVC 3
- Test in Mono
 
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
