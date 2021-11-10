'use strict';

module.exports = {
    compile: {
        build: {
            sourcemaps: false,
            embedtemplates: true,
            minify: true
        },
        dev: {
            sourcemaps: true,
            embedtemplates: true,
            minify: false
        },
        test: {
            sourcemaps: false,
            embedtemplates: true,
            minify: true
        }
    },
    sources: {

        // less files used by backoffice and preview
        // processed in the less task
        less: {
            installer: { files: "./src/less/installer.less", watch: "./src/less/**/*.less", out: "installer.min.css" },
            nonodes: { files: "./src/less/pages/nonodes.less", watch: "./src/less/**/*.less", out: "nonodes.style.min.css"},
            preview: { files: "./src/less/canvas-designer.less", watch: "./src/less/**/*.less", out: "canvasdesigner.min.css" },
            sbn: { files: "./src/less/belle.less", watch: "./src/**/*.less", out: "sbn.min.css" },
            rteContent: { files: "./src/less/rte-content.less", watch: "./src/less/**/*.less", out: "rte-content.css" }
        },

        // js files for backoffice
        // processed in the js task
        js: {
            websitepreview: { files: "./src/websitepreview/**/*.js", out: "sbn.websitepreview.js" },
            preview: { files: "./src/preview/**/*.js", out: "sbn.preview.js" },
            installer: { files: "./src/installer/**/*.js", out: "sbn.installer.js" },
            filters: { files: "./src/common/filters/**/*.js", out: "sbn.filters.js" },
            resources: { files: "./src/common/resources/**/*.js", out: "sbn.resources.js" },
            services: { files: ["./src/common/services/**/*.js", "./src/utilities.js"], out: "sbn.services.js" },
            security: { files: "./src/common/interceptors/**/*.js", out: "sbn.interceptors.js" },

            //the controllers for views
            controllers: {
                files: [
                    "./src/views/**/*.controller.js",
                    "./src/*.controller.js"
                ], out: "sbn.controllers.js"
            },

            //directives/components
            // - any JS file found in common / directives or common/ components
            // - any JS file found inside views that has the suffix .directive.js or .component.js
            directives: {
                files: [
                    "./src/common/directives/_module.js",
                    "./src/{common/directives,common/components}/**/*.js",
                    "./src/views/**/*.{directive,component}.js"
                ],
                out: "sbn.directives.js"
            }

        },

        //selectors for copying all views into the build
        //processed in the views task
        views:{
            views: {files: "./src/views/**/*.html", folder: ""},
            directives: {files: "./src/common/directives/**/*.html", folder: ""},
            components: {files: "./src/common/components/**/*.html", folder: ""},
            installer: {files: "./src/installer/steps/*.html", folder: "install/"}
        },

        //globs for file-watching
        globs:{
            views: ["./src/views/**/*.html", "./src/common/directives/**/*.html", "./src/common/components/**/*.html" ],
            less: "./src/less/**/*.less",
            js: "./src/*.js",
            lib: "./lib/**/*",
            assets: "./src/assets/**"
        }
    },
    roots: ["../Sbn.Web.UI/wwwroot/"],
    targets: {
        js: "sbn/js/",
        lib: "sbn/lib/",
        views: "sbn/views/",
        css: "sbn/assets/css/",
        assets: "sbn/assets/"
    }
};
