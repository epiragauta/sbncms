$(document).ready(function() {

    const dot = '<svg xmlns="http://www.w3.org/2000/svg" width="80" height="80" fill="#c0c0c0" class="bi bi-dot" viewBox="0 0 16 16"><path d="M8 9.5a1.5 1.5 0 1 0 0-3 1.5 1.5 0 0 0 0 3z"/></svg>';
    const activeDot = '<svg xmlns="http://www.w3.org/2000/svg" width="80" height="80" fill="#bcd601" class="bi bi-dot" viewBox="0 0 16 16"><path d="M8 9.5a1.5 1.5 0 1 0 0-3 1.5 1.5 0 0 0 0 3z"/></svg>';

    let selected = null;
    loadData();
    
    function loadData(){
        for (let index = 0; index < dataDiagram.length; index++) {
            $(`#element-${index+1}-label`).html(dataDiagram[index].step);
            $(`#element-${index+1}`).html(dataDiagram[index].name);
        }

        $(`#diagram-name`).html(InfoDiagram[0].name);
        $(`#diagram-center`).html(InfoDiagram[0].center);
        onInit();
    }

    $(`button[id^='element-']`).mouseover(function(e) {
        removeClassAll();
        selected = e.target.id.substr(-1);
        if(selected == 5){
            $(`#diagram-subtitle`).html(InfoDiagram[0].subtitle);
            $('.cicle_background').css({'background-image': `url('/media/assets/img/ciclo/cicle_background_active.svg')`}); 
        }else{
            $(`#diagram-subtitle`).html('');
            $('.cicle_background').css({'background-image': `url('/media/assets/img/ciclo/cicle_background.svg')`}); 
        }
        $(`#element-${selected}`).addClass("active");
        for (let index = 0; index < dataDiagram[selected-1].steps.length; index++) {
            $(`#menu-dot-${index+1}`).html(dot);
            $(`#menu-text-${index+1}`).html(dataDiagram[selected-1].steps[index].name);           
        }
        removeClassSelection();
    });

    $(`a[id^='menu-text-']`).mouseover(function(e) {
        removeClassSelection();
        let item = e.target.id.substr(-1);
        $(`#menu-text-${item}`).addClass("active-selected");
        $(`#menu-dot-${item}`).html(activeDot);
        $(`#title-d`).html(dataDiagram[selected-1].steps[item-1].name)
        $(`#text-d`).html(dataDiagram[selected-1].steps[item-1].text)
        $(`#img-d`).attr("src",dataDiagram[selected-1].steps[item-1].img);
        //$('#img-d').css({'background-image': `url('${dataDiagram[selected-1].steps[item-1].img}')`}); 
    });

    function onInit(){
        removeClassAll();
        selected = 1;
        let item = 1;
        $(`#element-${selected}`).addClass("active");
        for (let index = 0; index < dataDiagram[selected-1].steps.length; index++) {
            $(`#menu-dot-${index+1}`).html(dot);
            $(`#menu-text-${index+1}`).html(dataDiagram[selected-1].steps[index].name);           
        }
        removeClassSelection();
        $(`#menu-text-${item}`).addClass("active-selected");
        $(`#menu-dot-${item}`).html(activeDot);
        $(`#title-d`).html(dataDiagram[selected-1].steps[item-1].name)
        $(`#text-d`).html(dataDiagram[selected-1].steps[item-1].text)
        $(`#img-d`).attr("src",dataDiagram[selected-1].steps[item-1].img);
    }

    function removeClassSelection(){
        if(selected != null){
            for (let index = 0; index < dataDiagram[selected-1].steps.length; index++) {
                $(`#menu-dot-${index+1}`).html(dot);
                $(`#menu-text-${index+1}`).removeClass("active-selected");
            }
        }
    }

    function removeClassAll(){
        for (let index = 0; index <= 6; index++) {
            $(`#menu-text-${index+1}`).empty();
            $(`#menu-dot-${index+1}`).empty();
            $(`#element-${index+1}`).removeClass("active");
        }
    }
    
    $('.carousel').carousel('pause');
});