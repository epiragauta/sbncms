(function () {
    "use strict";

    /**
     * @ngdoc directive
     * @name sbn.directives.directive:umbBlockListRow
     * @description
     * renders each row for the block list editor
     */
    
    angular
        .module("sbn")
        .component("umbBlockListRow", {
            templateUrl: 'views/propertyeditors/blocklist/umb-block-list-row.html',
            controller: BlockListRowController,
            controllerAs: "vm",
            bindings: {
                blockEditorApi: "<",
                layout: "<",
                index: "<"
            },
            require: {
                valFormManager: "^^valFormManager"
            }
        }
    );

    function BlockListRowController($scope) {

        var vm = this;

        vm.$onInit = function () {
          
        };
    }   

})();
