angular.module('sbn.services')
	.factory('gridService', function ($http, $q){

	    var configPath = Sbn.Sys.ServerVariables.sbnUrls.gridConfig;
        var service = {
			getGridEditors: function () {
				return $http.get(configPath);
			}
		};

		return service;

	});
