(function () {
    "use strict";

    function ConfirmController($scope, userService) {

        var vm = this;
        vm.userEmailAddress = "";

        userService.getCurrentUser().then(function(user){
            vm.userEmailAddress = user.email;
        });
    }

    angular.module("sbn").controller("Sbn.Tours.UmbEmailMarketing.ConfirmController", ConfirmController);
})();
