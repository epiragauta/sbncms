/**
    * @ngdoc service
    * @name sbn.resources.packageInstallResource
    * @description handles data for package installations
    **/
function packageResource($q, $http, umbDataFormatter, umbRequestHelper) {

  return {

    /**
     * @ngdoc method
     * @name sbn.resources.packageInstallResource#getInstalled
     * @methodOf sbn.resources.packageInstallResource
     *
     * @description
     * Gets a list of installed packages
     */
    getInstalled: function () {
      return umbRequestHelper.resourcePromise(
        $http.get(
          umbRequestHelper.getApiUrl(
            "packageApiBaseUrl",
            "GetInstalled")),
        'Failed to get installed packages');
    },

    runMigrations: function (packageName) {
      return umbRequestHelper.resourcePromise(
        $http.post(
          umbRequestHelper.getApiUrl(
            "packageApiBaseUrl",
            "RunMigrations",
            { packageName: packageName })),
        'Failed to run migrations for package');
    },

    /**
     * @ngdoc method
     * @name sbn.resources.packageInstallResource#getCreated
     * @methodOf sbn.resources.packageInstallResource
     *
     * @description
     * Gets a list of created packages
     */
    getAllCreated: function () {
      return umbRequestHelper.resourcePromise(
        $http.get(
          umbRequestHelper.getApiUrl(
            "packageApiBaseUrl",
            "GetCreatedPackages")),
        'Failed to get created packages');
    },

    /**
     * @ngdoc method
     * @name sbn.resources.packageInstallResource#getCreatedById
     * @methodOf sbn.resources.packageInstallResource
     *
     * @description
     * Gets a created package by id
     */
    getCreatedById: function (id) {
      return umbRequestHelper.resourcePromise(
        $http.get(
          umbRequestHelper.getApiUrl(
            "packageApiBaseUrl",
            "GetCreatedPackageById",
            { id: id })),
        'Failed to get package');
    },

    getInstalledByName: function (packageName) {
      return umbRequestHelper.resourcePromise(
        $http.get(
          umbRequestHelper.getApiUrl(
            "packageApiBaseUrl",
            "GetInstalledPackageByName",
            { packageName: packageName })),
        'Failed to get package');
    },

    getEmpty: function () {
      return umbRequestHelper.resourcePromise(
        $http.get(
          umbRequestHelper.getApiUrl(
            "packageApiBaseUrl",
            "getEmpty")),
        'Failed to get scaffold');
    },

    /**
     * @ngdoc method
     * @name sbn.resources.packageInstallResource#savePackage
     * @methodOf sbn.resources.packageInstallResource
     *
     * @description
     * Creates or updates a package
     */
    savePackage: function (umbPackage) {
      return umbRequestHelper.resourcePromise(
        $http.post(
          umbRequestHelper.getApiUrl(
            "packageApiBaseUrl",
            "PostSavePackage"), umbPackage),
        'Failed to create package');
    },

    /**
     * @ngdoc method
     * @name sbn.resources.packageInstallResource#deleteCreatedPackage
     * @methodOf sbn.resources.packageInstallResource
     *
     * @description
     * Detes a created package
     */
    deleteCreatedPackage: function (packageId) {
      return umbRequestHelper.resourcePromise(
        $http.post(
          umbRequestHelper.getApiUrl(
            "packageApiBaseUrl",
            "DeleteCreatedPackage", { packageId: packageId })),
        'Failed to delete package ' + packageId);
    }
  };
}

angular.module('sbn.resources').factory('packageResource', packageResource);
