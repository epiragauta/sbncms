/**
@ngdoc directive
@name sbn.directives.directive:umbBoxContent
@restrict E

@description
Use this directive to render an empty container. Recommended to use it inside an {@link sbn.directives.directive:umbBox umbBox} directive. See documentation for {@link sbn.directives.directive:umbBox umbBox}.

<h3>Markup example</h3>
<pre>
    <umb-box>
        <umb-box-header title="this is a title"></umb-box-header>
        <umb-box-content>
            // Content here
        </umb-box-content>
    </umb-box>
</pre>

<h3>Use in combination with:</h3>
<ul>
    <li>{@link sbn.directives.directive:umbBox umbBox}</li>
    <li>{@link sbn.directives.directive:umbBoxHeader umbBoxHeader}</li>
</ul>
**/

(function(){
    'use strict';

    function BoxContentDirective() {

        var directive = {
            restrict: 'E',
            replace: true,
            transclude: true,
            templateUrl: 'views/components/html/umb-box/umb-box-content.html'
        };

        return directive;
    }

    angular.module('sbn.directives').directive('umbBoxContent', BoxContentDirective);

})();