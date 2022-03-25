$(document).ready(function() {

    const dot = '<svg xmlns="http://www.w3.org/2000/svg" width="80" height="80" fill="#c0c0c0" class="bi bi-dot" viewBox="0 0 16 16"><path d="M8 9.5a1.5 1.5 0 1 0 0-3 1.5 1.5 0 0 0 0 3z"/></svg>';
    const activeDot = '<svg xmlns="http://www.w3.org/2000/svg" width="80" height="80" fill="#bcd601" class="bi bi-dot" viewBox="0 0 16 16"><path d="M8 9.5a1.5 1.5 0 1 0 0-3 1.5 1.5 0 0 0 0 3z"/></svg>';

    let selected = null;
    loadData();
    
    function loadData(){
        for (let index = 0; index < dataDiagram.length; index++) {
            $(`#element-${index+1}-label`).html(dataDiagram[index].step);
            $(`#element-${index+1}`).html(dataDiagram[index].name);
            $(`#element-m-${index+1}`).html(dataDiagram[index].name); //mobile label
        }

        $(`#diagram-name`).html(InfoDiagram[0].name);
        $(`#diagram-center`).html(InfoDiagram[0].center);
        //$(`#diagram-center-m`).html(InfoDiagram[0].center);
        onInit();
    }

    function onInit(){
        removeClassAll();
        selected = 1;
        let item = 1;
        $(`#element-${selected}`).addClass("active");
        $(`#element-m-${selected}`).addClass("active text-white active-selected-m");
        for (let index = 0; index < dataDiagram[selected-1].steps.length; index++) {
            $(`#menu-dot-${index+1}`).html(dot);
            $(`#menu-text-${index+1}`).html(dataDiagram[selected-1].steps[index].name);           
        }
        removeClassSelection();
        $(`#menu-text-${item}`).addClass(`active-selected-${item}`);
        $(`#menu-dot-${item}`).html(activeDot);
        $(`#title-d`).html(dataDiagram[selected-1].steps[item-1].name)
        $(`#text-d`).html(dataDiagram[selected-1].steps[item-1].text)
        $(`#img-d`).attr("src",dataDiagram[selected-1].steps[item-1].img);
        loadMenuTextMobile();

    }
    
    $(".diagram-center").click(function(e){
        window.location="/implementar-sbn-2/";
        
    })

    $(`button[id^='element-']`).mouseover(function(e) {
        selectedMenu(e);
    });

    $(`a[id^='menu-text-']`).mouseover(function(e) {
        loadMenuText(e);
    });

    $(`a[id^='element-m-']`).click(function(e) {
        selectedMenu(e);
        loadMenuTextMobile();

    });

    function loadMenuText(e){
        console.log("loadMenuText", e);
        removeClassSelection();
        let item = e.target.id.substr(-1);
        $(`#menu-text-${item}`).addClass(`active-selected-${item}`);
        $(`#menu-dot-${item}`).html(activeDot);
        $(`#title-d`).html(dataDiagram[selected-1].steps[item-1].name.replace("-",""))
        $(`#text-d`).html(dataDiagram[selected-1].steps[item-1].text)
        $(`#img-d`).attr("src",dataDiagram[selected-1].steps[item-1].img);
    }

    function loadMenuTextMobile(){
        for (let imenu = 0; imenu < dataDiagram[selected-1].steps.length; imenu++) {
            //$('.carousel-indicators').append('<li data-target="#carouselIndicators" data-slide-to="'+ imenu + '"></li>');
            $('.carousel-indicators li:first-child').addClass('active');
            $('#cards-carousel div:first-child').addClass('active');
            $(`#cards-carousel`).append(`
                    <div class="carousel-item ${ imenu === 0  ? 'active' : '' }" >
                        <div class="card">
                            <img class="card-img-top img-d-m" src="${dataDiagram[selected-1].steps[imenu].img}" height="220">
                            <div class="card-body">
                            <h5 class="card-title">${dataDiagram[selected-1].steps[imenu].name}</h5>
                            <p class="card-text">${dataDiagram[selected-1].steps[imenu].text}</p>
                            </div>
                        </div>
                    </div>
                `)
        }
    }
      
    function selectedMenu(e){
        console.log("selectedMenu", e);
        removeClassAll();
        selected = e.target.id.substr(-1);
        if(selected == 5){
            //$(`#diagram-subtitle`).html(InfoDiagram[0].subtitle);
            $('.cicle_background').css({'background-image': `url('/media/assets/img/ciclo/cicle_background_active_.svg')`}); 
        }else{
            $(`#diagram-subtitle`).html('');
            $('.cicle_background').css({'background-image': `url('/media/assets/img/ciclo/cicle_background_.svg')`}); 
        }
        //$(`#element-${selected}`).addClass("active");
        $(`#element-m-${selected}`).addClass("active text-white active-selected-m");
        for (let index = 0; index < dataDiagram[selected-1].steps.length; index++) {
            $(`#menu-dot-${index+1}`).html(dot);
            $(`#menu-text-${index+1}`).html(dataDiagram[selected-1].steps[index].name);           
        }
        removeClassSelection();
    }

    function removeClassSelection(){
        console.log("removeClassSelection");
        if(selected !== null){
            for (let index = 0; index < dataDiagram[selected-1].steps.length; index++) {
                $(`#menu-dot-${index+1}`).html(dot);
                $(`#menu-text-${index+1}`).removeClass(`active-selected-${index+1}`);
            }
        }
    }

    function removeClassAll(){
        console.log("removeClassAll");
        $('#cards-carousel').empty();
        //$('.carousel-indicators').empty();
        for (let index = 0; index <= 6; index++) {
            $(`#menu-text-${index+1}`).empty();
            $(`#menu-dot-${index+1}`).empty();
            $(`#element-${index+1}`).removeClass("active");
            $(`#element-m-${index+1}`).removeClass("active text-white active-selected-m");
            $(`#menu-text-${index+1}`).removeClass(`active-selected-${index+1}`);
        }
    }

    
});