/*!
 * RouteJs by Daniel Lo Nigro (Daniel15) - http://dl.vc/routejs
 * Version {VERSION}
 * Released under the BSD license.
 */
(function (window) {
	'use strict';
	
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

	function escapeRegExp(string) {
		/// <summary>
		/// Escapes a string for usage in a regular expression
		/// </summary>
		/// <param name="string">Input string</param>
		/// <returns type="string">String suitable for inserting in a regex</returns>

		return string.replace(/[.*+?^${}()|[\]\\]/g, '\\$&');
	}
	
	////////////////////////////////////////////////////////////////////////////////////////////////
	
	var Route = function (route, settings) {
		///<summary>Handles route processing</summary>
		///<param name="route">Route information</param>
		
		var paramRegex = /\{(\w+)\}/g,
			matches;
			
		if (!route.optional) {
			route.optional = [];
		}
		
		this.settings = merge({ lowerCaseUrls: false }, settings);
		this.route = route;
		this._params = [];
		
		// Grab all the parameters from the URL
		while ((matches = paramRegex.exec(this.route.url)) !== null) {
			this._params.push(matches[1].toLowerCase());
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

			// Keys of values are case insensitive and are converted to lowercase server-side.
			// Convert keys of input to lowercase too.
			var routeValuesLowercase = {};
			for (var key in routeValues) {
				if (routeValues.hasOwnProperty(key)) {
					routeValuesLowercase[key.toLowerCase()] = routeValues[key];
				}
			}

			var finalValues = merge(this.route.defaults, routeValuesLowercase),
				processedParams = { controller: true, action: true },
				finalUrl;
			
			// Ensure area matches, if provided
			if (
				this.route.defaults.area &&
				this.route.defaults.area.toLowerCase() !== (routeValuesLowercase.area || '').toLowerCase()
			) {
				return null;
			}
			
			if (!this._checkConstraints(finalValues) || !this._checkNonDefaultValues(finalValues, processedParams)) {
				return null;
			}
		
			// Try to merge all URL parameters
			// If null, this means a required parameters was not specified.
			finalUrl = this._merge(finalValues, processedParams);
			if (finalUrl === null) {
				return null;
			}
			
			finalUrl = this._trimOptional(finalUrl) + this._extraParams(routeValues, processedParams, finalUrl.indexOf('?') > -1);
			return finalUrl;
		},
		
		_checkNonDefaultValues: function (finalValues, processedParams) {
			///<summary>Checks that any values using a non-default value have a matching merge field in the URL.</summary>
			///<param name="finalValues">Route values merged with defaults</param>
			///<param name="processedParams">Array of parameters that have already been processed</param>
			///<returns type="Boolean">true if all non-default parameters have a matching merge field, otherwise false.</returns>
			
			for (var key in this.route.defaults) {
				if (!this.route.defaults.hasOwnProperty(key)) {
					continue;
				}
				// We don't care about case when comparing defaults.
				if (
					(this.route.defaults[key] + '').toLowerCase() !== (finalValues[key] + '').toLowerCase() &&
					arrayIndexOf(this._params, key) === -1
				) {
					return false;
				} else {
					// Any defaults don't need to be explicitly specified in the querystring
					processedParams[key] = true;
				}
			}

			return true;
		},
		
		_merge: function (finalValues, processedParams) {
			///<summary>
			/// Merges parameters into the URL, keeping track of which parameters have been added and
			/// ensuring that all required parameters are specified.
			///</summary>
			///<param name="finalValues">Route values merged with defaults</param>
			///<param name="processedParams">Array of parameters that have already been processed</param>
			///<returns type="String">URL with parameters merged in, or null if not all parameters were specified</returns>
			
			var finalUrl = this.settings.lowerCaseUrls ? this.route.url.toLowerCase() : this.route.url;
			
			for (var i = 0, count = this._params.length; i < count; i++) {
				var paramName = this._params[i],
					isProvided = finalValues[paramName] !== undefined,
					isOptional = arrayIndexOf(this.route.optional, paramName) > -1;
				
				if (!isProvided && !isOptional) {
					return null;				
				}
				
				if (isProvided) {
					var paramRegex = new RegExp('\{' + escapeRegExp(paramName) + '}', 'i');
					var paramValue = this.settings.lowerCaseUrls && this._shouldConvertParam(paramName)
						? finalValues[paramName].toLowerCase()
						: finalValues[paramName];
					finalUrl = finalUrl.replace(paramRegex, encodeURIComponent(paramValue));
				}
				
				processedParams[paramName] = true;
			}

			return finalUrl;
		},
		
		_trimOptional: function (finalUrl) {
			///<summary>Trims any unused optional parameter segments from the end of the URL</summary>
			///<param name="finalUrl">URL with used parameters merged in</param>
			///<returns type="String">URL with unused optional parameters removed</returns>
			var urlPieces = finalUrl.split('/');
			for (var i = urlPieces.length - 1; i >= 0; i--) {
				// If it has a parameter, assume it's an ignored one (otherwise it would have been merged above)
				if (urlPieces[i].indexOf('{') > -1) {
					urlPieces.splice(i, 1);
				}
			}
			return urlPieces.join('/');
		},
		
		_extraParams: function (routeValues, processedParams, alreadyHasParams) {
			///<summary>Add any additional parameters not specified in the URL as querystring parameters</summary>
			///<param name="routeValues">Route values</param>
			///<param name="processedParams">Array of parameters that have already been processed</param>
			///<param name="alreadyHasParams">Whether this URL already has querystring parameters in it</param>
			///<returns type="String">URL encoded querystring parameters</returns>
			
			var params = '';
			
			// Add all other parameters to the querystring
			for (var key in routeValues) {
				if (!processedParams[key.toLowerCase()]) {
					params += (alreadyHasParams ? '&' : '?') + encodeURIComponent(key) + '=' + encodeURIComponent(routeValues[key]);
					alreadyHasParams = true;
				}
			}

			return params;
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
					parsedConstraints[key.toLowerCase()] =
						new RegExp('^(' + this.route.constraints[key].replace(/\\/g, '\\') + ')$', 'i');
				}
			}

			return parsedConstraints;
		},

		_shouldConvertParam: function (param) {
			///<summary>Gets if we should convert this param.</summary>
			///<param name="param">The param to check</param>

			return (
				param === 'controller' ||
				param === 'action' ||
				param === 'area'
				);
		},
	};

	var RouteManager = function (settings) {
		///<summary>Manages routes and selecting the correct route to use when routing URLs</summary>
		///<param name="routes">Raw route information</param>

		this.baseUrl = settings.baseUrl;
		this.lowerCaseUrls = settings.lowerCaseUrls;
		this.routes = [];
		var routeSettings = {
			lowerCaseUrls: this.lowerCaseUrls
		};
		for (var i = 0, count = settings.routes.length; i < count; i++) {
			this.routes.push(new Route(settings.routes[i], routeSettings));
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

			throw new Error('No route could be matched to route values: ' + routeValues);
		}
	};

	// Public API
	window.RouteJs = {
		version: '{VERSION}',
		Route: Route,
		RouteManager: RouteManager
	};
}(window));
