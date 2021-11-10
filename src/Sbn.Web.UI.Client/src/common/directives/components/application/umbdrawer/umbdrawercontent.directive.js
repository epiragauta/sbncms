/**
@ngdoc directive
@name sbn.directives.directive:umbDrawerContent
@restrict E
@scope

@description
Use this directive to render drawer content

<h3>Markup example</h3>
<pre>
	<umb-drawer-view>
        
        <umb-drawer-header
            title="Drawer Title"
            description="Drawer description">
        </umb-drawer-header>

        <umb-drawer-content>
            <!-- Your content here -->
            <pre>{{ model | json }}</pre>
        </umb-drawer-content>

        <umb-drawer-footer>
            <!-- Your content here -->
        </umb-drawer-footer>

	</umb-drawer-view>
</pre>


<h3>Use in combination with</h3>
<ul>
    <li>{@link sbn.directives.directive:umbDrawerView umbDrawerView}</li>
    <li>{@link sbn.directives.directive:umbDrawerHeader umbDrawerHeader}</li>
    <li>{@link sbn.directives.directive:umbDrawerFooter umbDrawerFooter}</li>
</ul>

**/

(function() {
    'use strict';

    function DrawerContentDirective() {

        var directive = {
            restrict: 'E',
            replace: true,
            transclude: true,
            templateUrl: 'views/components/application/umbdrawer/umb-drawer-content.html'
        };

        return directive;

    }

    angular.module('sbn.directives').directive('umbDrawerContent', DrawerContentDirective);

})();
