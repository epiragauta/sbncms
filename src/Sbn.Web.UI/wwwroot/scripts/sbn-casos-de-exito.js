$(document).ready(function() {
  $(document).keypress(
      function(event){
        if (event.which == '13') {
          event.preventDefault();
        }
    });    
  $('#select-sbn').on('change', function() {
    var query="sbn";
    var value=$('#select-sbn').val();
    $('#query').val(query);
    $('#value').val(value);
    //ocument.forms["query-form"].submit();
    window.location.href="?query=" + query + "&value=" + value;
  });
  
  $('#select-challenge').on('change', function() {
    var query="desafio";
    var value=$('#select-challenge').val();
    $('#query').val(query);
    $('#value').val(value);
    window.location.href="?query=" + query + "&value=" + value;
  });
  
  $('#select-country').on('change', function() {
    var query="pais";
    var value=$('#select-country').val();
    $('#query').val(query);
    $('#value').val(value);
    window.location.href="?query=" + query + "&value=" + value;
  });
  $('#search-sbn').keypress(function(event){
        var keycode = (event.keyCode ? event.keyCode : event.which);
        var value=$('#search-sbn').val();
        if(keycode == '13'){
            window.location.href="?search=" + value;
        }
    });
  
  let query = $("#query").val();
  let val = $("#value").val()
  switch (query){
    case "desafio":
      $("#select-challenge").val(val);
      break;
    case "pais":
      $("#select-country").val(val);
      break;
  }
  
});