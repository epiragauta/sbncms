function startUpVideosDashboardController($scope, dashboardResource) {
    $scope.videos = [];
    $scope.init = function(url){

        dashboardResource.getRemoteXmlData('COM', url).then(function (data) {
            var feed = $(data.data);
              $('item', feed).each(function (i, item) {
                  var video = {};
                  video.thumbnail = $(item).find('thumbnail').attr('url');
                  video.title = $("title", item).text();
                  video.link = $("guid", item).text();
                  $scope.videos.push(video);
              });

        },
        function (exception) {
            console.error('ex from remote data', exception);
        });
    };
}

angular.module("sbn").controller("Sbn.Dashboard.StartupVideosController", startUpVideosDashboardController);


function startUpDynamicContentController($q, $timeout, $scope, dashboardResource, assetsService, tourService, eventsService) {
    var vm = this;
    var evts = [];

    vm.loading = true;
    vm.showDefault = false;

    vm.startTour = startTour;

    function onInit() {
        // load tours
        tourService.getGroupedTours().then(function(groupedTours) {
            vm.tours = groupedTours;
        });
    }

    function startTour(tour) {
        tourService.startTour(tour);
    }

    // default dashboard content
    vm.defaultDashboard = {
        infoBoxes: [
            {
                title: "Documentation",
                description: "Find the answers to your Sbn questions",
                url: "https://our.sbn.com/documentation/?utm_source=core&utm_medium=dashboard&utm_content=text&utm_campaign=documentation/"
            }
        ],
        articles: [
            {
                title: "Titulo",
                description: "Descripcion",
                img: "views/dashboard/default/sbntv.png",
                url: "https://ecopetrol.com.co",
                altText: "Sbn",
                buttonText: "Visitar SbN"
            }
        ]
    };

    evts.push(eventsService.on("appState.tour.complete", function (name, completedTour) {
        $timeout(function(){
            Utilities.forEach(vm.tours, tourGroup => {
                Utilities.forEach(tourGroup, tour => {
                    if (tour.alias === completedTour.alias) {
                        tour.completed = true;
                    }
                });
            });
        });
    }));

    //proxy remote css through the local server
    assetsService.loadCss(dashboardResource.getRemoteDashboardCssUrl("content"), $scope);

    dashboardResource.getRemoteDashboardContent("content").then(data => {

        vm.loading = false;

        //test if we have received valid data
        //we capture it like this, so we avoid UI errors - which automatically triggers ui based on http response code
        if (data && data.sections) {
            vm.dashboard = data;
        } else {
            vm.showDefault = true;
        }
    },
    function (exception) {
        console.error(exception);
        vm.loading = false;
        vm.showDefault = true;
    });

    onInit();

}

angular.module("sbn").controller("Sbn.Dashboard.StartUpDynamicContentController", startUpDynamicContentController);

function startupLatestEditsController($scope) {

}
angular.module("sbn").controller("Sbn.Dashboard.StartupLatestEditsController", startupLatestEditsController);

function MediaFolderBrowserDashboardController($scope, $routeParams, $location, contentTypeResource, userService) {

    var currentUser = {};

    userService.getCurrentUser().then(function (user) {

        currentUser = user;

        // check if the user has access to the root which they will require to see this dashboard
        if (currentUser.startMediaIds.indexOf(-1) >= 0) {

            //get the system media listview
            contentTypeResource.getPropertyTypeScaffold(-96)
                .then(function(dt) {

                    $scope.fakeProperty = {
                        alias: "contents",
                        config: dt.config,
                        description: "",
                        editor: dt.editor,
                        hideLabel: true,
                        id: 1,
                        label: "Contents:",
                        validation: {
                            mandatory: false,
                            pattern: null
                        },
                        value: "",
                        view: dt.view
                    };

                    // tell the list view to list content at root
                    $routeParams.id = -1;
            });

        } else if (currentUser.startMediaIds.length > 0){
            // redirect to start node
            $location.path("/media/media/edit/" + (currentUser.startMediaIds.length === 0 ? -1 : currentUser.startMediaIds[0]));
        }

    });

}
angular.module("sbn").controller("Sbn.Dashboard.MediaFolderBrowserDashboardController", MediaFolderBrowserDashboardController);
