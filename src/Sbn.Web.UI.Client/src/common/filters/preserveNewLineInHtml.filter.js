/**
* @ngdoc filter
* @name sbn.filters.preserveNewLineInHtml
* @description 
* Used when rendering a string as HTML (i.e. with ng-bind-html) to convert line-breaks to <br /> tags
**/
angular.module("sbn.filters").filter('preserveNewLineInHtml', function () {
  return function (text) {
	if (!text) {
		return '';
	}
    return text.replace(/\n/g, '<br />');
  };
});
	