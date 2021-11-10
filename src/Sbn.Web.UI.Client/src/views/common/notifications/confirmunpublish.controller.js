angular.module("sbn").controller("Sbn.Notifications.ConfirmUnpublishController",
	function ($scope, notificationsService, eventsService) {	

		$scope.confirm = function(not, action){
			eventsService.emit('content.confirmUnpublish', action);
			notificationsService.remove(not);
		};
	
	});