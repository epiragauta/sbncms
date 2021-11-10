(function () {
    "use strict";

    angular
    .module("sbn")
    .controller("Sbn.PropertyEditors.MediaPicker3PropertyEditor.CreateButtonController",
    function Controller($scope) {

        var vm = this;
        vm.plusPosY = 0;

        vm.onMouseMove = function($event) {
            vm.plusPosY = $event.offsetY;
        }

    });

})();
