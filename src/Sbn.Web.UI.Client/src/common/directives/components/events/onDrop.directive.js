(function () {
    'use strict';

    function onDropDirective(){

        function link(scope, elm, attrs) {
            var f = function () {
                scope.$apply(attrs.onDrop);
            };
            elm.on("drop", f);
            scope.$on("$destroy", function () { elm.off("drop", f); });
        }

        var directive = {
            link: link
        };

        return directive;

    }

    angular.module('sbn.directives').directive('onDrop', onDropDirective);

})();
