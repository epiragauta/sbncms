(function () {
    "use strict";

    function focusLockService($document) {
        var elementToInert = $document[0].querySelector('#mainwrapper');

        function addInertAttribute() {
            if (elementToInert) {
                elementToInert.setAttribute('inert', true);
            }
        }

        function removeInertAttribute() {
            if (elementToInert) {
                elementToInert.removeAttribute('inert');
            }
        }

        var service = {
            addInertAttribute: addInertAttribute,
            removeInertAttribute: removeInertAttribute
        }

        return service;

    }

    angular.module("sbn.services").factory("focusLockService", focusLockService);

})();
