/**
* @ngdoc service
* @name sbn.services.imageHelper
* @deprecated
**/
function imageHelper(umbRequestHelper, mediaHelper) {
    return {
        /**
         * @ngdoc function
         * @name sbn.services.imageHelper#getImagePropertyValue
         * @methodOf sbn.services.imageHelper
         * @function    
         *
         * @deprecated
         */
        getImagePropertyValue: function (options) {
            return mediaHelper.getImagePropertyValue(options);
        },
        /**
         * @ngdoc function
         * @name sbn.services.imageHelper#getThumbnail
         * @methodOf sbn.services.imageHelper
         * @function    
         *
         * @deprecated
         */
        getThumbnail: function (options) {
            return mediaHelper.getThumbnail(options);
        },

        /**
         * @ngdoc function
         * @name sbn.services.imageHelper#scaleToMaxSize
         * @methodOf sbn.services.imageHelper
         * @function    
         *
         * @deprecated
         */
        scaleToMaxSize: function (maxSize, width, height) {
            return mediaHelper.scaleToMaxSize(maxSize, width, height);
        },

        /**
         * @ngdoc function
         * @name sbn.services.imageHelper#getThumbnailFromPath
         * @methodOf sbn.services.imageHelper
         * @function    
         *
         * @deprecated
         */
        getThumbnailFromPath: function (imagePath) {
            return mediaHelper.getThumbnailFromPath(imagePath);
        },

        /**
         * @ngdoc function
         * @name sbn.services.imageHelper#detectIfImageByExtension
         * @methodOf sbn.services.imageHelper
         * @function    
         *
         * @deprecated
         */
        detectIfImageByExtension: function (imagePath) {
            return mediaHelper.detectIfImageByExtension(imagePath);
        }
    };
}
angular.module('sbn.services').factory('imageHelper', imageHelper);