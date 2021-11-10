/**
* @ngdoc directive
* @name sbn.directives.directive:umbPane
* @restrict E
**/
angular.module("sbn.directives.html")
    .directive('umbPane', function () {
        return {
            transclude: true,
            restrict: 'E',
            replace: true,
            templateUrl: 'views/components/html/umb-pane.html'
        };
    });
