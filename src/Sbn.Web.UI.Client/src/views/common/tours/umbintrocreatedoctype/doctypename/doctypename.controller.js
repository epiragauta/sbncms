(function () {
    "use strict";

    function DocTypeNameController($scope) {

        var vm = this;
        var element = $($scope.model.currentStep.element);

        vm.error = false;

        vm.initNextStep = initNextStep;

        function initNextStep() {
            if (element.val().toLowerCase() === 'home page') {
                $scope.model.nextStep();
            } else {
                vm.error = true;
            }
        }

    }

    angular.module("sbn").controller("Sbn.Tours.UmbIntroCreateDocType.DocTypeNameController", DocTypeNameController);
})();
