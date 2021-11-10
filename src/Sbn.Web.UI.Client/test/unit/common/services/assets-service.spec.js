describe('Assets service tests', function () {
   var assetsService, $window, $rootScope;
   beforeEach(module('sbn.services'));
   beforeEach(module('sbn.mocks.services'));

   beforeEach(inject(function ($injector) {
        assetsService = $injector.get('assetsService');
        $window = $injector.get("$window");
        $rootScope = $injector.get('$rootScope');
   }));

   afterEach(inject(function($rootScope) {
     $rootScope.$apply();
   }));

   describe('Loading js assets', function () {
        
        it('Loads a javascript file', function () {

          var loaded = false;
          // runs( function(){
          //       assetsService.loadJs("lib/sbn/NamespaceManager.js").then(function(){
          //           expect(Sbn.Sys).toNotBe(undefined);
          //       });
          // });
          // runs(function(){
          //    expect(Sbn.Sys).toNotBe(undefined);
          // });
        });
    });
});