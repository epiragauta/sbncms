
// TODO: WE NEED TO CONVERT ALL OF THESE METHODS TO PROXY TO OUR APPLICATION SINCE MANY CUSTOM APPS USE THIS!

Sbn.Sys.registerNamespace("Sbn.Application");

(function($) {
    Sbn.Application.SpeechBubble = function() {

        /**
         * @ngdoc function
         * @name getRootScope
         * @methodOf UmbClientMgr
         * @function
         *
         * @description
         * Returns the root angular scope
         */
        function getRootScope() {
            return angular.element(document.getElementById("sbnMainPageBody")).scope();
        }
        
        /**
         * @ngdoc function
         * @name getRootInjector
         * @methodOf UmbClientMgr
         * @function
         *
         * @description
         * Returns the root angular injector
         */
        function getRootInjector() {
            return angular.element(document.getElementById("sbnMainPageBody")).injector();
        }


        return {
            
            /**
             * @ngdoc function
             * @name ShowMessage
             * @methodOf Sbn.Application.SpeechBubble
             * @function
             *
             * @description
             * Proxies a legacy call to the new notification service
             */               
            ShowMessage: function (icon, header, message) {
                //get our angular navigation service
                var injector = getRootInjector();
                var notifyService = injector.get("notificationsService");

                switch(icon){
                    case "save":
                        notifyService.success(header, message);
                        break;
                    case "success":
                        notifyService.success(header, message);
                        break;    
                    case "warning":
                        notifyService.warning(header, message);
                        break;
                    case "error":
                        notifyService.error(header, message);
                        break;
                    default:
                        notifyService.info(header, message);
                }

                
            }
        };
    };
})(jQuery);

//define alias for use throughout application
var UmbSpeechBubble = new Sbn.Application.SpeechBubble();
