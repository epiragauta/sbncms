/**
 @ngdoc service
 * @name sbn.services.listViewPrevalueHelper
 *
 *
 * @description
 * Service for accessing the prevalues of a list view being edited in the inline list view editor in the doctype editor
 */
(function () {
    'use strict';

    function listViewPrevalueHelper() {

        var prevalues = [];

        /**
        * @ngdoc method
        * @name sbn.services.listViewPrevalueHelper#getPrevalues
        * @methodOf sbn.services.listViewPrevalueHelper
        *
        * @description
        * Set the collection of prevalues
        */

        function getPrevalues() {
            return prevalues;
        }

        /**
        * @ngdoc method
        * @name sbn.services.listViewPrevalueHelper#setPrevalues
        * @methodOf sbn.services.listViewPrevalueHelper
        *
        * @description
        * Changes the current layout used by the listview to the layout passed in. Stores selection in localstorage
        *
        * @param {Array} values Array of prevalues
        */

        function setPrevalues(values) {
            prevalues = values;
        }

        

        var service = {

            getPrevalues: getPrevalues,
            setPrevalues: setPrevalues

        };

        return service;

    }


    angular.module('sbn.services').factory('listViewPrevalueHelper', listViewPrevalueHelper);


})();
