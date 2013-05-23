To finish installing RouteJs, you need to reference it in your view.

For Razor (typically in _Layout.cshtml):
<script src="@RouteJs.RouteJsHandler.HandlerUrl"></script>

For Web Forms:
<script src="<%: RouteJs.RouteJsHandler.HandlerUrl %>"></script>

Please refer to the RouteJs site for more information, or to report bugs: https://github.com/Daniel15/RouteJs