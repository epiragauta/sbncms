/**
 * @ngdoc controller
 * @name Sbn.Editors.Member.CreateController
 * @function
 * 
 * @description
 * The controller for the member creation dialog
 */
function memberCreateController($scope, memberTypeResource, iconHelper, navigationService, $location) {
    
    memberTypeResource.getTypes($scope.currentNode.id).then(function (data) {
        $scope.allowedTypes = iconHelper.formatContentTypeIcons(data);
    });

    $scope.close = function() {
        const showMenu = true;
        navigationService.hideDialog(showMenu);
    };

    $scope.createMemberType = function (memberType) {        
        $location.path("/member/member/edit/" + $scope.currentNode.id).search("doctype", memberType.alias).search("create", "true");
        navigationService.hideNavigation();
    };
}

angular.module('sbn').controller("Sbn.Editors.Member.CreateController", memberCreateController);
