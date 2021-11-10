/**
    * @ngdoc service
    * @name sbn.resources.stylesheetResource
    * @description service to retrieve available stylesheets
    * 
    *
    **/
function stylesheetResource($q, $http, umbRequestHelper) {

    //the factory object returned
    return {
        
        /**
         * @ngdoc method
         * @name sbn.resources.stylesheetResource#getAll
         * @methodOf sbn.resources.stylesheetResource
         *
         * @description
         * Gets all registered stylesheets
         *
         * ##usage
         * <pre>
         * stylesheetResource.getAll()
         *    .then(function(stylesheets) {
         *        alert('its here!');
         *    });
         * </pre> 
         * 
         * @returns {Promise} resourcePromise object containing the stylesheets.
         *
         */
        getAll: function () {            
            return umbRequestHelper.resourcePromise(
               $http.get(
                   umbRequestHelper.getApiUrl(
                       "stylesheetApiBaseUrl",
                       "GetAll")),
               'Failed to retrieve stylesheets ');
        },       

        /**
         * @ngdoc method
         * @name sbn.resources.stylesheetResource#getRulesByName
         * @methodOf sbn.resources.stylesheetResource
         *
         * @description
         * Returns all defined child rules for a stylesheet with a given name
         *
         * ##usage
         * <pre>
         * stylesheetResource.getRulesByName("ie7stylesheet")
         *    .then(function(rules) {
         *        alert('its here!');
         *    });
         * </pre> 
         * 
         * @returns {Promise} resourcePromise object containing the rules.
         *
         */
        getRulesByName: function (name) {            
            return umbRequestHelper.resourcePromise(
               $http.get(
                   umbRequestHelper.getApiUrl(
                       "stylesheetApiBaseUrl",
                       "GetRulesByName",
                       [{ name: name }])),
               'Failed to retrieve stylesheets ');
        }
    };
}

angular.module('sbn.resources').factory('stylesheetResource', stylesheetResource);
