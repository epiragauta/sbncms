angular.module("sbn")
.controller("Sbn.PropertyEditors.TagsController",
    function ($scope) {

        $scope.valueChanged = function(value) {
            $scope.model.value = value;
        }

    }
);
