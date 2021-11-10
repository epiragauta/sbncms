//create the namespace (NOTE: This loads before any dependencies so we don't have a namespace mgr so we just create it manually)
var Sbn = {};
Sbn.Sys = {};
//define a global static object
Sbn.Sys.ServerVariables = {
    sbnUrls: {
        "contentApiBaseUrl": "/sbn/SbnApi/Content/",
        "mediaApiBaseUrl": "/sbn/SbnApi/Media/",
        "dataTypeApiBaseUrl": "/sbn/SbnApi/DataType/",
        "sectionApiBaseUrl": "/sbn/SbnApi/Section/",
        "treeApplicationApiBaseUrl": "/sbn/SbnTrees/ApplicationTreeApi/",
        "contentTypeApiBaseUrl": "/sbn/Api/ContentType/",
        "mediaTypeApiBaseUrl": "/sbn/Api/MediaType/",
        "macroApiBaseUrl": "/sbn/Api/Macro/",
        "authenticationApiBaseUrl": "/sbn/SbnApi/Authentication/",
        "serverVarsJs": "/belle/lib/lazyload/empty.js",
        "imagesApiBaseUrl": "/sbn/SbnApi/Images/",
        "entityApiBaseUrl": "/sbn/SbnApi/Entity/",
        "dashboardApiBaseUrl": "/sbn/SbnApi/Dashboard/",
        "updateCheckApiBaseUrl": "/sbn/Api/UpdateCheck/",
        "relationApiBaseUrl": "/sbn/SbnApi/Relation/",
        "rteApiBaseUrl": "/sbn/SbnApi/RichTextPreValue/",
        "iconApiBaseUrl": "/sbn/SbnApi/Icon/"
    },
    sbnSettings: {
        "sbnPath": "/sbn",
        "appPluginsPath" : "/App_Plugins",
        "imageFileTypes": "jpeg,jpg,gif,bmp,png,tiff,tif",
        "keepUserLoggedIn": true
    },
    sbnPlugins: {
        trees: [
            { alias: "myTree", packageFolder: "MyPackage" }
        ]
    },
    isDebuggingEnabled: true,
    application: {
        assemblyVersion: "1",
        version: "7"
    }
};
