(function() {
    'use strict';

    function SelectWhen($timeout) {

        function link(scope, el, attr, ctrl) {

            attr.$observe("umbSelectWhen", function(newValue) {
                if (newValue === "true") {
                    $timeout(function() {
                        el.select();
                    });
                }
            });

        }

        var directive = {
            restrict: 'A',
            link: link
        };

        return directive;
    }

    angular.module('sbn.directives').directive('umbSelectWhen', SelectWhen);

})();
