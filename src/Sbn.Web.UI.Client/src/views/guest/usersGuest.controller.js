(function () {
    "use strict";

    function UsersGuestController($scope, $timeout, $location, $routeParams, usersResource,
        userGroupsResource, userService, localizationService,
        $http, umbRequestHelper, formHelper, dateHelper, editorService,
        listViewHelper, externalLoginInfoService) {

        var vm = this;

        vm.page = {};
        vm.users = [];
        vm.userGroups = [];        
        vm.selection = [];
        vm.newUser = {};
        vm.usersOptions = {};
        vm.userSortData = [
            { label: "Name (A-Z)", key: "Name", direction: "Ascending" },
            { label: "Name (Z-A)", key: "Name", direction: "Descending" },
            { label: "Newest", key: "CreateDate", direction: "Descending" },
            { label: "Oldest", key: "CreateDate", direction: "Ascending" }
        ];

        localizationService.localizeMany(_.map(vm.userSortData, function (userSort) {
            return "user_sort" + userSort.key + userSort.direction;
        })).then(function (data) {
            var reg = /^\[[\S\s]*]$/g;
            _.each(data, function (value, index) {
                if (!reg.test(value)) {
                    // Only translate if key exists
                    vm.userSortData[index].label = value;
                }
            });
        });

        vm.labels = {};
        localizationService.localizeMany(["user_stateAll"]).then(function (data) {
            vm.labels.all = data[0];
        });
        
        vm.usersViewState = 'overview';
        vm.usernameIsEmail = Sbn.Sys.ServerVariables.sbnSettings.usernameIsEmail;

        vm.layouts = [
            {
                "icon": "icon-thumbnails-small",
                "path": "1",
                "selected": true
            },
            {
                "icon": "icon-list",
                "path": "2",
                "selected": true
            }
        ];

        // Get last selected layout for "users" (defaults to first layout = card layout)
        vm.activeLayout = listViewHelper.getLayout("users", vm.layouts);       

        vm.toggleFilter = toggleFilter;
        vm.selectLayout = selectLayout;        
        vm.getEditPath = getEditPath;         
        vm.searchUsers = searchUsers;
        vm.onBlurSearch = onBlurSearch;
        vm.getFilterName = getFilterName;        
        vm.setOrderByFilter = setOrderByFilter;
        vm.changePageNumber = changePageNumber;
        vm.getSortLabel = getSortLabel;        
        vm.goToUser = goToUser;

        function init() {

            initViewOptions();
            // Get users
            getGuestUsers();
        }

        function initViewOptions() {

            // Start with default view options.
            vm.usersOptions.filter = "";
            vm.usersOptions.orderBy = "Name";
            vm.usersOptions.orderDirection = "Ascending";

            // Update from querystring if available.
            initViewOptionFromQueryString("filter");
            initViewOptionFromQueryString("orderBy");
            initViewOptionFromQueryString("orderDirection");
            initViewOptionFromQueryString("pageNumber");           

        }

        function initViewOptionFromQueryString(key, isCollection) {
            var value = $location.search()[key];
            if (value) {
                if (isCollection) {
                    value = value.split(",");
                }

                vm.usersOptions[key] = value;
            }
        }        

        function getSortLabel(sortKey, sortDirection) {
            var found = _.find(vm.userSortData,
                function (i) {
                    return i.key === sortKey && i.direction === sortDirection;
                });
            return found ? found.label : sortKey;
        }

        function toggleFilter(type) {
            // hack: on-outside-click prevents us from closing the dropdown when clicking on another link
            // so I had to do this manually
            switch (type) {               
                case "orderBy":
                    vm.page.showOrderByFilter = !vm.page.showOrderByFilter;
                    vm.page.showStatusFilter = false;
                    vm.page.showGroupFilter = false;
                    break;
            }
        }

        function selectLayout(selectedLayout) {
            // save the selected layout for "users" so it's applied next time the user visits this section
            vm.activeLayout = listViewHelper.setLayout("users", selectedLayout, vm.layouts);
        }              
        
        var search = _.debounce(function () {
            $scope.$apply(function () {
                vm.usersOptions.pageNumber = 1;
                getUsers();
            });
        }, 500);

        function searchUsers() {
            search();
        }

        function onBlurSearch() {
            updateLocation("filter", vm.usersOptions.filter);
        }

        function getFilterName(array) {
            var name = vm.labels.all;
            var found = false;
            array.forEach(function (item) {
                if (item.selected) {
                    if (!found) {
                        name = item.name
                        found = true;
                    } else {
                        name = name + ", " + item.name;
                    }
                }
            });
            return name;
        }        
        
        function setOrderByFilter(value, direction) {
            vm.usersOptions.orderBy = value;
            vm.usersOptions.orderDirection = direction;
            updateLocation("orderBy", value);
            updateLocation("orderDirection", direction);
            getUsers();
        }

        function changePageNumber(pageNumber) {
            vm.usersOptions.pageNumber = pageNumber;
            updateLocation("pageNumber", pageNumber);
            getUsers();
        }

        function updateLocation(key, value) {
            $location.search("filter", vm.usersOptions.filter);// update filter, but first when something else requests a url update.
            $location.search(key, value);
        }

        function goToUser(user) {
            $location.path(pathToUser(user))
                .search("orderBy", vm.usersOptions.orderBy)
                .search("orderDirection", vm.usersOptions.orderDirection)
                .search("pageNumber", vm.usersOptions.pageNumber)                
                .search("userGroups", getUsersOptionsFilterCollectionAsDelimitedStringOrNull(vm.usersOptions.userGroups))
                .search("create", null)
                .search("invite", null);
        }

        function getUsersOptionsFilterCollectionAsDelimitedStringOrNull(collection) {
            if (collection && collection.length > 0) {
                return collection.join(",");
            }

            return null;
        }

        function getEditPath(user) {
            return pathToUser(user) + usersOptionsAsQueryString();
        }

        function pathToUser(user) {
            return "/users/users/user/" + user.id;
        }

        function usersOptionsAsQueryString() {
            var qs = "?orderBy=" + vm.usersOptions.orderBy +
                "&orderDirection=" + vm.usersOptions.orderDirection +
                "&pageNumber=" + vm.usersOptions.pageNumber +
                "&filter=" + vm.usersOptions.filter;

            
            qs += addUsersOptionsFilterCollectionToQueryString("userGroups", vm.usersOptions.userGroups);
            qs += "&mculture=" + $location.search().mculture;

            return qs;
        }

        function addUsersOptionsFilterCollectionToQueryString(name, collection) {
            if (collection && collection.length > 0) {
                return "&" + name + "=" + collection.join(",");
            }

            return "";
        }

        // helpers
        function getGuestUsers() {

            vm.loading = true;

            // Get users
            umbRequestHelper.resourcePromise(
                $http.get("/Sbn/Api/SbnData/GetAllUsers"),
                "Error retornando información de usuarios invitados"
            ).then(function(dataUsrs) {
                vm.loading = false;
                var items = [];
                dataUsrs.forEach(u =>{
                    var x = JSON.parse(u.value);
                    items.push({email:u.key, fullName:x.fullName, organization: x.organization,sector:x.sector});
                });
                var data = {items: items, pageNumber:1, pageSize:25, totalItems: items.length, totalPages: 1};
                vm.users = data.items;
                vm.usersOptions.pageNumber = data.pageNumber;
                vm.usersOptions.pageSize = data.pageSize;
                vm.usersOptions.totalItems = data.totalItems;
                vm.usersOptions.totalPages = data.totalPages;
            });           
        }
        init();
        
    }

    angular.module("sbn").controller("Sbn.Editors.Guest.UsersGuestController", UsersGuestController);

})();
