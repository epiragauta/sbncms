$(document).ready(function(){
    
    $("#bi-search").on('click', function(){
      $('#hdr-search').toggle();  
    });
    
    $('#hdr-search').on('keypress', function(e){
        var keycode = (event.keyCode ? event.keyCode : event.which);
        if(keycode == '13'){
            console.log('You pressed a "enter" key in textbox : ', $('#hdr-search').val());
            if ($('#hdr-search').val().length > 2){
                window.location.href="/search?search=" + $('#hdr-search').val();
           }
        }
    });
    
    /*
    Carousel
    */
    $('#carousel-casos').on('slide.bs.carousel', function (e) {
       
        var $e = $(e.relatedTarget);
        var idx = $e.index();
        var itemsPerSlide = 5;
        var totalItems = $('.carousel-item').length;
     
        if (idx >= totalItems-(itemsPerSlide-1)) {
            var it = itemsPerSlide - (totalItems - idx);
            for (var i=0; i<it; i++) {
                // append slides to end
                if (e.direction=="left") {
                    $('.carousel-item').eq(i).appendTo('.carousel-inner-mc');
                }
                else {
                    $('.carousel-item').eq(0).appendTo('.carousel-inner-mc');
                }
            }
        }
    });
    
    $('#carousel-desafios').on('slide.bs.carousel', function (e) {
       
        var $e = $(e.relatedTarget);
        var idx = $e.index();
        var itemsPerSlide = 5;
        var totalItems = $('.carousel-item').length;
     
        if (idx >= totalItems-(itemsPerSlide-1)) {
            var it = itemsPerSlide - (totalItems - idx);
            for (var i=0; i<it; i++) {
                // append slides to end
                if (e.direction=="left") {
                    $('.carousel-item').eq(i).appendTo('.carousel-inner');
                }
                else {
                    $('.carousel-item').eq(0).appendTo('.carousel-inner');
                }
            }
        }
    });
    
    try{
        // SmartWizard initialize
        $('#smartwizard').smartWizard({theme:'dots'});
    }catch(e){
        console.log(e);   
    }
    
    $(".sw-btn-next").on('click', function(){
        $(window).scrollTop(0);
    })
    
    $(".sw-btn-prev").on('click', function(){
        $(window).scrollTop(0);
    })
  
});