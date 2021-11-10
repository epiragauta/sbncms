
/**
* @ngdoc service
* @name sbn.resources.modelsBuilderManagementResource
* @description Resources to get information on modelsbuilder status and build models
**/
function modelsBuilderManagementResource($q, $http, umbRequestHelper) {

    return {

        /**
        * @ngdoc method
        * @name sbn.resources.modelsBuilderManagementResource#getModelsOutOfDateStatus
        * @methodOf sbn.resources.modelsBuilderManagementResource
        *
        * @description
        * Gets the status of modelsbuilder 
        *
        * ##usage
        * <pre>
        * modelsBuilderManagementResource.getModelsOutOfDateStatus()
        *  .then(function() {
        *        Do stuff...*
        * });
        * </pre>
        * 
        * @returns {Promise} resourcePromise object.
        *
        */
        getModelsOutOfDateStatus: function () {
            return umbRequestHelper.resourcePromise(
                $http.get(umbRequestHelper.getApiUrl("modelsBuilderBaseUrl", "GetModelsOutOfDateStatus")),
                "Failed to get models out-of-date status");
        },

        /**
        * @ngdoc method
        * @name sbn.resources.modelsBuilderManagementResource#buildModels
        * @methodOf sbn.resources.modelsBuilderManagementResource
        *
        * @description
        * Builds the models
        *
        * ##usage
        * <pre>
        * modelsBuilderManagementResource.buildModels()
        *  .then(function() {
        *        Do stuff...*
        * });
        * </pre>
        *
        * @returns {Promise} resourcePromise object.
        *
        */
        buildModels: function () {
            return umbRequestHelper.resourcePromise(
                $http.post(umbRequestHelper.getApiUrl("modelsBuilderBaseUrl", "BuildModels")),
                "Failed to build models");
        },

        /**
        * @ngdoc method
        * @name sbn.resources.modelsBuilderManagementResource#getDashboard
        * @methodOf sbn.resources.modelsBuilderManagementResource
        *
        * @description
        * Gets the modelsbuilder dashboard
        *
        * ##usage
        * <pre>
        * modelsBuilderManagementResource.getDashboard()
        *  .then(function() {
        *        Do stuff...*
        * });
        * </pre>
        *
        * @returns {Promise} resourcePromise object.
        *
        */
        getDashboard: function () {
            return umbRequestHelper.resourcePromise(
                $http.get(umbRequestHelper.getApiUrl("modelsBuilderBaseUrl", "GetDashboard")),
                "Failed to get dashboard");
        }
    };
}
angular.module("sbn.resources").factory("modelsBuilderManagementResource", modelsBuilderManagementResource);
