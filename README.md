RouteJs
=======
RouteJs allows you to use your ASP.NET MVC or WebForms routes from JavaScript. It does not depend on
jQuery or any other JavaScript framework, so can be used in any scenario.

It is currently under development, and not yet ready for production use.

Requirements
============
Required:
 * ASP.NET 4.0 or higher
 * [Json.NET](http://james.newtonking.com/projects/json-net.aspx)

Optional:
 * ASP.NET MVC 4

Example
=======

```javascript
var url = Router.action('Controller', 'Action'); 
// url will be '/Controller/Action' with the default route
```

TODO
====
- Write unit tests
- Add caching headers in handler
- Support optional ASP.NET MVC URLs (UrlParameter.Optional)
- Create NuGet package
- Support WebForms routes
- Support ASP.NET MVC 3
 
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
