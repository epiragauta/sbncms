/**
* @ngdoc directive
* @name sbn.directives.directive:umbPanel
* @restrict E
**/
angular.module("sbn.directives.html")
	.directive('umbPanel', function($timeout, $log){
		return {
			restrict: 'E',
			replace: true,
			transclude: 'true',
			templateUrl: 'views/components/html/umb-panel.html'
		};
	});
