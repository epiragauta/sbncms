/**
 * @ngdoc service
 * @name sbn.resources.healthCheckResource
 * @function
 *
 * @description
 * Used by the health check dashboard to get checks and send requests to fix checks.
 */
(function () {
    'use strict';

    function healthCheckResource($http, umbRequestHelper) {

        /**
         * @ngdoc function
         * @name sbn.resources.healthCheckService#getAllChecks
         * @methodOf sbn.resources.healthCheckResource
         * @function
         *
         * @description
         * Called to get all available health checks
         */
        function getAllChecks() {
            return umbRequestHelper.resourcePromise(
                $http.get(Sbn.Sys.ServerVariables.sbnUrls.healthCheckBaseUrl + "GetAllHealthChecks"),
                "Failed to retrieve health checks"
            );
        }

        /**
         * @ngdoc function
         * @name sbn.resources.healthCheckService#getStatus
         * @methodOf sbn.resources.healthCheckResource
         * @function
         *
         * @description
         * Called to get execute a health check and return the check status
         */
        function getStatus(id) {
            return umbRequestHelper.resourcePromise(
                $http.get(Sbn.Sys.ServerVariables.sbnUrls.healthCheckBaseUrl + 'GetStatus?id=' + id),
                'Failed to retrieve status for health check with ID ' + id
            );
        }

        /**
         * @ngdoc function
         * @name sbn.resources.healthCheckService#executeAction
         * @methodOf sbn.resources.healthCheckResource
         * @function
         *
         * @description
         * Called to execute a health check action (rectifying an issue)
         */
        function executeAction(action) {
            return umbRequestHelper.resourcePromise(
                $http.post(Sbn.Sys.ServerVariables.sbnUrls.healthCheckBaseUrl + 'ExecuteAction', action),
                'Failed to execute action with alias ' + action.alias + ' and healthCheckId + ' + action.healthCheckId
            );
        }

        var resource = {
            getAllChecks: getAllChecks,
            getStatus: getStatus,
            executeAction: executeAction
        };

        return resource;

    }


    angular.module('sbn.resources').factory('healthCheckResource', healthCheckResource);


})();
