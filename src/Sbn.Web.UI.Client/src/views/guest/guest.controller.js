(function () {
    "use strict";

    function GuestOverviewController($scope, $location, $routeParams, localizationService) {

        var vm = this;
        let usersUri = $routeParams.method;
        
        //note on the below, we dont assign a view unless it's the right route since if we did that it will load in that controller
        //for the view which is unecessary and will cause extra overhead/requests to occur
        vm.page = {};
        vm.page.labels = {};
        vm.page.name = "";
        vm.page.navigation = [];

        function onInit() {
            loadNavigation();
        }

        function loadNavigation() {

            var labels = ["sections_users", "general_groups", "user_userManagement"];

            localizationService.localizeMany(labels).then(function (data) {
                vm.page.labels.users = "Usuarios";
                vm.page.labels.groups = data[1];
                vm.page.name =  "Usuarios Invitados";  // data[2];

                vm.page.navigation = [
                    {
                        "name": vm.page.labels.users,
                        "icon": "icon-user",
                        "action": function () {
                            $location.path("/guest/guest/guest").search("create", null);
                        },
                        "view": "views/guest/users-guest.html",
                        "active": true,
                        "alias": "guest"
                    }
                ];
            });
        }

        onInit();

    }

    angular.module("sbn").controller("Sbn.Editors.Guest.OverviewController", GuestOverviewController);

})();
