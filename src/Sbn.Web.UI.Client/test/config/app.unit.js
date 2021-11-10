var app = angular.module('sbn', [
    'sbn.filters',
    'sbn.directives',
    'sbn.resources',
    'sbn.services',
    
	'sbn.mocks',
	'sbn.interceptors',

    'ngRoute',
    'ngAnimate',
    'ngCookies',
    'ngSanitize',
    
    //'ngMessages',
    'tmh.dynamicLocale',
    //'ngFileUpload',
    'LocalStorageModule'
    //'chart.js'
]);
