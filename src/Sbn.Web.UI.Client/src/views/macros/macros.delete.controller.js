/**
 * @ngdoc controller
 * @name Sbn.Editors.Macros.DeleteController
 * @function
 * 
 * @description
 * The controller for deleting macro items
 */
function MacrosDeleteController($scope, macroResource, navigationService, treeService) {
    var vm = this;
    
    vm.name = $scope.currentNode.name;
    function performDelete() {
        $scope.currentNode.loading = true;
        macroResource.deleteById($scope.currentNode.id).then(function () {
            $scope.currentNode.loading = false;

            treeService.removeNode($scope.currentNode);
            
            navigationService.hideMenu();
        });
    }

    function cancel() {
        navigationService.hideDialog();
    }
    
    vm.performDelete = performDelete;
    vm.cancel = cancel;
}

angular.module("sbn").controller("Sbn.Editors.Macros.DeleteController", MacrosDeleteController);
