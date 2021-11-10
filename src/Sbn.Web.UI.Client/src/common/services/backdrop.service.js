/**
 @ngdoc service
 * @name sbn.services.backdropService
 *
 * @description
 * <b>Added in Sbn 7.8</b>. Application-wide service for handling backdrops.
 * 
 */

(function () {
    "use strict";

    function backdropService(eventsService) {

        var args = {
            opacity: null,
            element: null,
            elementPreventClick: false,
            disableEventsOnClick: false,
            show: false
        };

        /**
         * @ngdoc method
         * @name sbn.services.backdropService#open
         * @methodOf sbn.services.backdropService
         *
         * @description
         * Raises an event to open a backdrop
		 * @param {Object} options The backdrop options
         * @param {Number} options.opacity Sets the opacity on the backdrop (default 0.4)
         * @param {DomElement} options.element Highlights a DOM-element (HTML-selector)
         * @param {Boolean} options.elementPreventClick Adds blocking element on top of highligted area to prevent all clicks
         * @param {Boolean} options.disableEventsOnClick Disables all raised events when the backdrop is clicked
		 */
        function open(options) {

            if (options && options.element) {
                args.element = options.element;
            }

            if (options && options.disableEventsOnClick) {
                args.disableEventsOnClick = options.disableEventsOnClick;
            }

            args.show = true;

            eventsService.emit("appState.backdrop", args);
        }

        /**
         * @ngdoc method
         * @name sbn.services.backdropService#close
         * @methodOf sbn.services.backdropService
         *
         * @description
         * Raises an event to close the backdrop
         * 
		 */
        function close() {
            args.opacity = null;
            args.element = null;
            args.elementPreventClick = false;
            args.disableEventsOnClick = false;
            args.show = false;
            eventsService.emit("appState.backdrop", args);
        }

        /**
         * @ngdoc method
         * @name sbn.services.backdropService#setOpacity
         * @methodOf sbn.services.backdropService
         *
         * @description
         * Raises an event which updates the opacity option on the backdrop
		 */
        function setOpacity(opacity) {
            args.opacity = opacity;
            eventsService.emit("appState.backdrop", args);
        }

        /**
         * @ngdoc method
         * @name sbn.services.backdropService#setHighlight
         * @methodOf sbn.services.backdropService
         *
         * @description
         * Raises an event which updates the element option on the backdrop
		 */
        function setHighlight(element, preventClick) {
            args.element = element;
            args.elementPreventClick = preventClick;
            eventsService.emit("appState.backdrop", args);
        }

        var service = {
            open: open,
            close: close,
            setOpacity: setOpacity,
            setHighlight: setHighlight
        };

        return service;

    }

    angular.module("sbn.services").factory("backdropService", backdropService);

})();
