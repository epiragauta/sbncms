angular.module("sbn")
    .controller("Sbn.Editors.DocumentTypes.ExportController",
        function ($scope, contentTypeResource, navigationService) {

            $scope.export = function () {
                contentTypeResource.export($scope.currentNode.id);
                navigationService.hideMenu();
            };

            $scope.cancel = function () {
                navigationService.hideDialog();
            };
        });
