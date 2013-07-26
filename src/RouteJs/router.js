/*!
 * RouteJs by Daniel Lo Nigro (Daniel15) - http://dl.vc/routejs
 * Version {VERSION}
 * Released under the BSD license.
 */
(function (window) {
	// Helper methods
	function merge (first, second) {
		///<summary>
		/// Return a new object that contains the properties from both of these objects. If a property
		/// exists in both objects, the property in `second` will override the property in `first`.
		///</summary>
		var result = {},
			key;
		
		for (key in first) {
			if (first.hasOwnProperty(key)) {
				result[key] = first[key];
			}
		}
		
		for (key in second) {
			if (second.hasOwnProperty(key)) {
				result[key] = second[key];
			}
		}		
		
		return result;
	}

	var arrayIndexOf;
	
	// Check for native Array.indexOf support
	if (Array.prototype.indexOf) {
		arrayIndexOf = function (array, searchElement) {
			return array.indexOf(searchElement);
		};
	} else {
		arrayIndexOf = function (array, searchElement) {
			for (var i = 0, count = array.length; i < count; i++) {
				if (array[i] === searchElement) {
					return i;
				}
			}
			return -1;
		};		
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////
	
	var Route = function (route) {
		///<summary>Handles route processing</summary>
		///<param name="route">Route information</param>
		
		var paramRegex = /\{(\w+)\}/g,
			matches;
			
		if (!route.optional) {
			route.optional = [];
		}
		
		this.route = route;
		this._params = [];
		
		// Grab all the parameters from the URL
		while ((matches = paramRegex.exec(this.route.url)) !== null) {
			this._params.push(matches[1]);
		}
	};

	Route.prototype = {
		build: function (routeValues) {
			///<summary>
			/// Build a URL using this route, based on the passed route values. Returns null if the
			/// route values provided are not sufficent to build a URL using this route.
			///</summary>
			///<param name="routeValues">Route values</param>
			///<returns type="String">URL, or null when building a URL is not possible</returns>
			var finalValues = merge(this.route.defaults, routeValues),
				finalUrl = this.route.url,
				processedParams = { controller: true, action: true },
			    ignoredParams = [],
				key;
			
			// Ensure area matches, if provided
			if (this.route.defaults.area && this.route.defaults.area !== routeValues.area) {
				return null;
			}
			
			// Ensure constraints match
			if (!this._checkConstraints(finalValues)) {
				return null;
			}

			// Any values using a non-default value need to have a matching merge field in the URL
			for (key in this.route.defaults) {
				if (!this.route.defaults.hasOwnProperty(key)) {
					continue;
				}
				
				if (this.route.defaults[key] !== finalValues[key] && arrayIndexOf(this._params, key) === -1) {
					return null;
				} else {
					// Any defaults don't need to be explicitly specified in the querystring
					processedParams[key] = true;
				}
			}
		
			// Ensure all URL parameters are supplied (either in the route values or defaults)
			for (var i = 0, count = this._params.length; i < count; i++) {
				var paramName = this._params[i],
				    isProvided = finalValues[paramName] !== undefined,
					isOptional = arrayIndexOf(this.route.optional, paramName) > -1;
				
				if (!isProvided && !isOptional) {
					return null;				
				}
				
				if (isProvided) {
					finalUrl = finalUrl.replace('{' + paramName + '}', encodeURIComponent(finalValues[paramName]));	
				} else {
					ignoredParams.push(paramName);
				}
				
				processedParams[paramName] = true;
			}
			
			// Remove all the segments that have optional parameters which were not provided
			// Loop backwards to make deleting easier
			var urlPieces = finalUrl.split('/');
			for (var i = urlPieces.length - 1; i >= 0; i--) {
				// If it has a parameter, assume it's an ignored one (otherwise it would have been merged above)
				if (urlPieces[i].indexOf('{') > -1) {
					urlPieces.splice(i, 1);
				}
			}
			finalUrl = urlPieces.join('/');

			// Add all other parameters to the querystring
			for (var key in routeValues) {
				if (!processedParams[key]) {
					finalUrl += (finalUrl.indexOf('?') > -1 ? '&' : '?') + encodeURIComponent(key) + '=' + encodeURIComponent(routeValues[key]);
				}
			}
			
			return finalUrl;
		},
		
		_checkConstraints: function (routeValues) {
			///<summary>Validate that the route constraints match the specified route values</summary>
			///<param name="routeValues">Route values</param>
			///<returns type="Boolean"><c>true</c> if the route validation succeeds, otherwise <c>false</c>.</returns>
			
			// Bail out early if there's no constraints on this route
			if (!this.route.constraints) {
				return true;
			}
			
			if (!this._parsedConstraints) {
				this._parsedConstraints = this._parseConstraints();
			}
			
			// Check every constraint matches
			for (var key in this._parsedConstraints) {
				if (this._parsedConstraints.hasOwnProperty(key) && !this._parsedConstraints[key].test(routeValues[key])) {
					return false;
				}
			}

			return true;
		},
		
		_parseConstraints: function () {
			///<summary>Parse the string constraints into regular expressions</summary>
			
			var parsedConstraints = {};
			
			for (var key in this.route.constraints) {
				if (this.route.constraints.hasOwnProperty(key)) {
					parsedConstraints[key] = new RegExp('^(' + this.route.constraints[key].replace(/\\/g, '\\') + ')');
				}
			}

			return parsedConstraints;
		}
	};

	var RouteManager = function (settings) {
		///<summary>Manages routes and selecting the correct route to use when routing URLs</summary>
		///<param name="routes">Raw route information</param>

		this.baseUrl = settings.baseUrl;
		this.routes = [];
		for (var i = 0, count = settings.routes.length; i < count; i++) {
			this.routes.push(new Route(settings.routes[i]));
		}
	};

	RouteManager.prototype = {
		action: function (controller, action, routeValues) {
			///<summary>Generate a URL to an action</summary>
			///<param name="controller">Name of the controller</param>
			///<param name="action">Name of the action</param>
			///<param name="routeValues">Route values</param>
			///<returns type="String">URL for the specified action</returns>
			
			routeValues = routeValues || { };
			routeValues.controller = controller;
			routeValues.action = action;
			return this.route(routeValues);
		},
		
		route: function (routeValues) {
			///<summary>Generate a URL to an action</summary>
			///<param name="routeValues">Route values</param>
			///<returns type="String">URL for the specified action</returns>
			
			for (var i = 0, count = this.routes.length; i < count; i++) {
				var url = this.routes[i].build(routeValues);
				if (url) {
					return this.baseUrl + url;
				}
			}

			throw Error('No route could be matched to route values: ' + routeValues);
		}
	};

	// Public API
	window.RouteJs = {
		version: '{VERSION}',
		Route: Route,
		RouteManager: RouteManager
	};
}(window));