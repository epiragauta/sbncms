/**
@ngdoc directive
@name sbn.directives.directive:umbEditorFooterContentRight
@restrict E

@description
Use this directive to align content right inside the main editor footer.

<h3>Markup example</h3>
<pre>
    <div ng-controller="MySection.Controller as vm">

        <form name="mySectionForm" novalidate>

            <umb-editor-view>

                <umb-editor-footer>

                    <umb-editor-footer-content-left>
                        // align content left
                    </umb-editor-footer-content-left>

                    <umb-editor-footer-content-right>
                        // align content right
                    </umb-editor-footer-content-right>

                </umb-editor-footer>

            </umb-editor-view>

        </form>

    </div>
</pre>

<h3>Use in combination with</h3>
<ul>
    <li>{@link sbn.directives.directive:umbEditorView umbEditorView}</li>
    <li>{@link sbn.directives.directive:umbEditorHeader umbEditorHeader}</li>
    <li>{@link sbn.directives.directive:umbEditorContainer umbEditorContainer}</li>
    <li>{@link sbn.directives.directive:umbEditorFooter umbEditorFooter}</li>
    <li>{@link sbn.directives.directive:umbEditorFooterContentLeft umbEditorFooterContentLeft}</li>
</ul>
**/

(function() {
   'use strict';

   function EditorFooterContentRightDirective() {

      var directive = {
         transclude: true,
         restrict: 'E',
         replace: true,
         templateUrl: 'views/components/editor/umb-editor-footer-content-right.html'
      };

      return directive;
   }

   angular.module('sbn.directives').directive('umbEditorFooterContentRight', EditorFooterContentRightDirective);

})();
