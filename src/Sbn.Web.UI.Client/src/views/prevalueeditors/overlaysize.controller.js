angular.module("sbn").controller("Sbn.PrevalueEditors.OverlaySizeController",
    function ($scope) {
        if (!$scope.model.value) {
            $scope.model.value = "small";
        }
    });
