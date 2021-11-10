(function() {
    'use strict';

    /**
     * Used to set the current client culture on all requests API requests
     * @param {any} $routeParams
     */
    function cultureRequestInterceptor($injector) {
        return {
            //dealing with requests:
            'request': function (config) {

                if (!Sbn.Sys.ServerVariables.sbnSettings.sbnPath) {
                    // no settings available, we're probably on the login screen
                    return config;
                }

                if (!config.url.match(RegExp(Sbn.Sys.ServerVariables.sbnSettings.sbnPath + "\/backoffice\/", "i"))) {
                    // it's not an API request, no handling
                    return config;
                }

                var $routeParams = $injector.get("$routeParams");
                if ($routeParams) {
                    // it's an API request, add the current client culture as a header value
                    config.headers["X-UMB-CULTURE"] = $routeParams.cculture ? $routeParams.cculture : $routeParams.mculture;
                    config.headers["X-UMB-SEGMENT"] = $routeParams.csegment ? $routeParams.csegment : null;
                }
                
                return config;
            }
        };
    }

    angular.module('sbn.interceptors').factory('cultureRequestInterceptor', cultureRequestInterceptor);

})();
