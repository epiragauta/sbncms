/**
 * @ngdoc controller
 * @name Sbn.Editors.StyleSheets.DeleteController
 * @function
 *
 * @description
 * The controller for deleting stylesheets
 */
function StyleSheetsDeleteController($scope, codefileResource, treeService, navigationService) {

    $scope.performDelete = function() {

        //mark it for deletion (used in the UI)
        $scope.currentNode.loading = true;
        
        codefileResource.deleteByPath('stylesheets', $scope.currentNode.id)
            .then(function() {
                $scope.currentNode.loading = false;
                treeService.removeNode($scope.currentNode);
                navigationService.hideMenu();
            });
    };

    $scope.cancel = function() {
        navigationService.hideDialog();
    };
}

angular.module("sbn").controller("Sbn.Editors.StyleSheets.DeleteController", StyleSheetsDeleteController);
