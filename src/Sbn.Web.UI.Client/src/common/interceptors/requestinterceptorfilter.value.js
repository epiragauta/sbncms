(function() {
    'use strict';

    /** Used to filter the request interceptor */
    function requestInterceptorFilter() {
        return ["www.gravatar.com"];
    }

    angular.module('sbn.interceptors').value('requestInterceptorFilter', requestInterceptorFilter);

})();
