/**
 * @ngdoc controller
 * @name Sbn.Editors.Content.EmptyRecycleBinController
 * @function
 * 
 * @description
 * The controller for deleting content
 */
function ContentEmptyRecycleBinController($scope, contentResource, treeService, navigationService, notificationsService, $route) {

    $scope.busy = false;

    $scope.performDelete = function() {

        //(used in the UI)
        $scope.busy = true;
        $scope.currentNode.loading = true;

        contentResource.emptyRecycleBin($scope.currentNode.id).then(function (result) {

            $scope.busy = false;
            $scope.currentNode.loading = false;
            
            treeService.removeChildNodes($scope.currentNode);
            navigationService.hideMenu();

            //reload the current view
            $route.reload();
        });

    };

    $scope.cancel = function() {
        navigationService.hideDialog();
    };
}

angular.module("sbn").controller("Sbn.Editors.Content.EmptyRecycleBinController", ContentEmptyRecycleBinController);
