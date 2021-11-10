/**
* @ngdoc directive
* @name sbn.directives.directive:umbNavigation
* @restrict E
**/
function umbNavigationDirective() {
    return {
        restrict: "E",    // restrict to an element
        replace: true,   // replace the html element with the template
        templateUrl: 'views/components/application/umb-navigation.html'
    };
}

angular.module('sbn.directives').directive("umbNavigation", umbNavigationDirective);
