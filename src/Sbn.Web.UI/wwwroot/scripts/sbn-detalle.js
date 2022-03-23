$(document).ready(function() {
   
    var areaAplicacion = localStorage.getItem("areaAplicacion");
    var desafio = localStorage.getItem("desafio");
    var problema = localStorage.getItem("problema");
    var lblSinSeleccion = "Sin selección";
    areaAplicacion = areaAplicacion === "undefined" ? lblSinSeleccion : areaAplicacion;
    desafio = desafio === "undefined" ? lblSinSeleccion : desafio;
    problema = problema === "undefined" ? lblSinSeleccion : problema;
    
    $("#area").html(areaAplicacion);
    $("#desafio").html(desafio);
    $("#problema").html(problema);
    
    //const idocument = document.getElementsByName("media1").item(0).contentDocument;
    //idocument.onload = () => idocument.getElementsByName("media");
    
    //document.getElementsByTagName("video").autoplay = false;
    setTimeout(function(){
        document.getElementsByName("media1").item(0).contentDocument.activeElement.children[0].pause();   
    },200);
});