RouteJs
=======
RouteJs allows you to use your ASP.NET MVC or WebForms routes from JavaScript. It does not depend on
jQuery or any other JavaScript framework, so can be used in any scenario. RouteJs works with your
existing MVC routes, you do not need to use a different routing syntax or modify any of your 
existing routes at all.

Bug reports and feature requests are welcome!

[![Build status](https://img.shields.io/appveyor/ci/Daniel15/RouteJs/master.svg)](https://ci.appveyor.com/project/Daniel15/routejs/branch/master)&nbsp;
[![NuGet downloads](http://img.shields.io/nuget/dt/RouteJs.Mvc4.svg)](https://www.nuget.org/packages/RouteJs.Mvc4/)&nbsp;
[![NuGet version](http://img.shields.io/nuget/v/RouteJs.Mvc4.svg)](https://www.nuget.org/packages/RouteJs.Mvc4/)&nbsp;

Requirements
============
Required:

 * ASP.NET 4.0 or higher
 * [Json.NET](http://james.newtonking.com/projects/json-net.aspx)
 * Any version of ASP.NET MVC from 2 onwards

Installation
============

For ASP.NET MVC 5 and older
---------------------------
```
Install-Package RouteJs.Mvc4
```
(replace `RouteJs.Mvc4` with `RouteJs.Mvc3` for ASP.NET MVC 3 or `RouteJs.Mvc2` for ASP.NET MVC 2)

Alternatively, you can get the latest development build from the
[build server](http://teamcity.codebetter.com/viewType.html?buildTypeId=routejs&guest=1).

Once installed, you need to reference the RouteJs handler in your view. This serves the JavaScript
and route information:
```html
<script src="@RouteJs.RouteJsHandler.HandlerUrl"></script>
```

For ASP.NET 5 and MVC 6 Beta
----------------------------

```
Install-Package RouteJs.AspNet
```
Once installed, you need to add RouteJs to your `Startup.cs` file:

```csharp
services.AddRouteJs();

```

The RouteJs handler also needs to be referenced in your view (generally `_Layout.cshtml` is a 
good place for this). This serves the JavaScript and route information:

```csharp
@inject RouteJs.IRouteJsHelper RouteJs
@RouteJs.Render()
```

Usage
=====

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

The routes that are exposed are controlled by the web.config "exposeAllRoutes" setting for ASP.NET MVC 5 and below:
```xml
<routeJs exposeAllRoutes="true" />
```

And via `Startup.cs` for ASP.NET MVC 6:
```csharp
services.AddRouteJs().ConfigureRouteJs(config => {
	config.ExposeAllRoutes = true;
});
```

If set to "true", all of your ASP.NET MVC routes will be exposed to JavaScript, unless you 
explicitly hide them via the `HideRoutesInJavaScript` attribute on a controller. If set to "false", 
all routes will be hidden unless you explicitly use the `ExposeRoutesInJavaScript` attribute on the
controller. These two attributes currently affect all routes for the controller.

Changelog
=========
2.0.2 - 25th October 2015
-------------------------
 - Updated ASP.NET 5 support to beta 8.
 - [#43](https://github.com/Daniel15/RouteJs/issues/43) - Add LowerCaseUrls
   option to convert generated URLs to lowercase. *Thanks to 
   [Mohammad Rahhal](https://github.com/mrahhal)*.
 - [#37](https://github.com/Daniel15/RouteJs/issues/37) - Handle empty URLs
   (ie. home page).
 
2.0.1 - 13th September 2015
---------------------------
 - Updated ASP.NET 5 support to beta 7
 - [#41](https://github.com/Daniel15/RouteJs/issues/41) - Correctly handle when 
   routes in areas have "area" default param

2.0 - 23th August 2015
----------------------
 - Added support for ASP.NET 5 and MVC 6

1.1.9 - 24th January 2015
-------------------------
 - [#38](https://github.com/Daniel15/RouteJs/issues/38) - Fix handling of constraints with 
   case-insensitive URL parameters.

1.1.8 - 26th October 2014
-------------------------
 - [#34](https://github.com/Daniel15/RouteJs/issues/34) - Ignore case of controller, action, area, 
   and keys of parameters.

1.1.7 - 6th July 2014
---------------------
 - [#32](https://github.com/Daniel15/RouteJs/issues/32) - Only include string (regular expression)
   constraints, ignore custom constraints as they can't be evaluated client-side.

1.1.6 - 27th April 2014
-----------------------
 - [#31](https://github.com/Daniel15/RouteJs/issues/31) - Defaults and optional parameters are 
   sometimes serialised as null

1.1.5 - 17th November 2013
--------------------------
 - [#27](https://github.com/Daniel15/RouteJs/issues/27) - NullReferenceException for routes without
   DataTokens.
 - Added package for ASP.NET MVC 5

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
