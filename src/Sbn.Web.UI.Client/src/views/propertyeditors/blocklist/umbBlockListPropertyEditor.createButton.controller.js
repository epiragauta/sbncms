(function () {
    "use strict";

    angular
    .module("sbn")
    .controller("Sbn.PropertyEditors.BlockListPropertyEditor.CreateButtonController", 
    function Controller($scope) {
        
        var vm = this;
        vm.plusPosX = 0;

        vm.onMouseMove = function ($event) {
            vm.plusPosX = $event.offsetX;
        };

    });

})();
