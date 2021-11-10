angular.module('sbn.resources').factory('Sbn.PropertyEditors.NestedContent.Resources',
    function ($q, $http, umbRequestHelper) {
        return {
            getContentTypes: function () {
                var url = Sbn.Sys.ServerVariables.sbnSettings.sbnPath + "/backoffice/SbnApi/NestedContent/GetContentTypes";
                return umbRequestHelper.resourcePromise(
                    $http.get(url),
                    'Failed to retrieve content types'
                );
            }
        };
    });