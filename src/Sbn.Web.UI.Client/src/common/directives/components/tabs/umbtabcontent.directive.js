/**
@ngdoc directive
@name sbn.directives.directive:umbTabContent
@restrict E
@scope

@description
Use this directive to render tab content. For an example see: {@link sbn.directives.directive:umbTabContent umbTabContent}

@param {string=} tab The tab.

**/
(function () {
    'use strict';

    angular
        .module('sbn.directives')
        .component('umbTabContent', {
            transclude: true,
            templateUrl: 'views/components/tabs/umb-tab-content.html',
            controllerAs: 'vm',
            bindings: {
                tab: '<'
            }
        });

})();
