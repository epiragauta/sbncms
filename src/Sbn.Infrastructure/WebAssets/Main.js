LazyLoad.js("##JsInitialize##", function () {
    //we need to set the legacy UmbClientMgr path
    if ((typeof UmbClientMgr) !== "undefined") {
        UmbClientMgr.setSbnPath('"##SbnPath##"');
    }

    jQuery(document).ready(function () {

        angular.bootstrap(document, ['"##AngularModule##"']);

    });
});
