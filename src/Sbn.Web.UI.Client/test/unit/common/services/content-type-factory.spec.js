describe('content type factory tests', function () {
    var $rootScope, $httpBackend, contentTypeResource, mocks;

    // beforeEach(module('ngMockE2E'));
    beforeEach(module('sbn.services'));
    beforeEach(module('sbn.resources'));
    beforeEach(module('sbn.mocks'));
    
    beforeEach(inject(function ($injector, mocksUtils) {
        
        //for these tests we don't want any authorization to occur
        mocksUtils.disableAuth();

        $rootScope = $injector.get('$rootScope');
        $httpBackend = $injector.get('$httpBackend'); 
        mocks = $injector.get("contentTypeMocks");
        mocks.register();
        contentTypeResource = $injector.get('contentTypeResource');
    }));

    describe('global content type factory crud', function () {
      
        it('should return a allowed content type collection given a document id', function(){

           // m.expectAllowedChildren();

            var collection;
            contentTypeResource.getAllowedTypes(1234).then(function(result){
                collection = result;
            });


            $rootScope.$digest();
            $httpBackend.flush();

            expect(collection.length).toBe(3);
        });      
  });
});