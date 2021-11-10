(function () {
    "use strict";

    function DetailsController($scope, externalLoginInfoService) {

        var vm = this;

        vm.denyLocalLogin = externalLoginInfoService.hasDenyLocalLogin();
    }

    angular.module("sbn").controller("Sbn.Editors.Users.DetailsController", DetailsController);

})();
