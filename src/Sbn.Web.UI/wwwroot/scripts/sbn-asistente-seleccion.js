const AREA_APLICACION = "area-aplicacion";
const DESAFIO = "desafio";
const PROBLEMA = "problema";
var selectedFilters = [];
let allChallengesInSbns = [];
let allIssuesInSbns = [];
let allAreasInSbns = [];
let allChallenges = [];
let allIssues = [];
let allAreas = [];
let hidedChallenges;
let hidedIssues;
let hidedAreas;

$(document).ready(function () {

    $(`input[type=radio][name=${AREA_APLICACION}]`).change(function () {
        // console.log(this.value);
        $("#clearImplementacion").show();
        if (selectedFilters.indexOf(AREA_APLICACION) == -1) {
            selectedFilters.push(AREA_APLICACION); 
        }
        allChallengesInSbns = [];
        allIssuesInSbns = [];
        allAreasInSbns = [];
        filterSbn({ areaAplicacion: this.value });
    });

    $("#clearImplementacion").click(function () {
        $(`input[name="${AREA_APLICACION}"]`).prop('checked', false);
        $("#clearImplementacion").hide();
        if (selectedFilters.length == 1) {
            filterRadios(DESAFIO, allChallenges);
            filterRadios(PROBLEMA, allIssues);
            filterRadios(AREA_APLICACION, allAreas);
            selectedFilters = [];
            $("#msg-without-sbn").hide();
            hideAllSbn();
        } else if (selectedFilters.length == 2) {
            if (selectedFilters.indexOf(AREA_APLICACION) === 0) {
                selectedFilters.shift();
                filterRadios(PROBLEMA, allIssues);
                filterRadios(DESAFIO, allChallenges);
            } else {
                selectedFilters.pop();
                if (selectedFilters[0] == DESAFIO) {
                    filterSbn({ desafio: $("input[name='desafio']:checked").val() });
                } else {
                    filterSbn({ problema: $("input[name='problema']:checked").val() });
                }
            }
        } else if (selectedFilters.length == 3) {
            if (selectedFilters.indexOf(AREA_APLICACION) === 0) {
                selectedFilters.shift();
            } else if (selectedFilters.indexOf(AREA_APLICACION) === 2) {
                selectedFilters.pop();
            } else {
                selectedFilters = [selectedFilters[0], selectedFilters[2]];
            }
        }
    });

    $(`input[type=radio][name=${DESAFIO}]`).change(function () {
        // console.log(this.value);
        $("#clearDesafio").show();
        if (selectedFilters.indexOf(DESAFIO) == -1) {
            selectedFilters.push(DESAFIO);
        }
        allChallengesInSbns = [];
        allIssuesInSbns = [];
        allAreasInSbns = [];
        filterSbn({ desafio: this.value });
    });

    $("#clearDesafio").click(function () {
        $(`input[name=${DESAFIO}]`).prop('checked', false);
        $("#clearDesafio").hide();
        if (selectedFilters.length == 1) {
            filterRadios(DESAFIO, allChallenges);
            filterRadios(PROBLEMA, allIssues);
            filterRadios(AREA_APLICACION, allAreas);
            selectedFilters = [];
            $("#msg-without-sbn").hide();
            hideAllSbn();
        } else if (selectedFilters.length == 2) {
            if (selectedFilters.indexOf(DESAFIO) === 0) {
                selectedFilters.shift();
                filterRadios(PROBLEMA, allIssues);
                filterRadios(AREA_APLICACION, allAreas);
            } else {
                selectedFilters.pop();
                if (selectedFilters[0] == AREA_APLICACION) {
                    filterSbn({ areaAplicacion: $("input[name='area-aplicacion']:checked").val() });
                } else {
                    filterSbn({ problema: $("input[name='problema']:checked").val() });
                }
            }
        } else if (selectedFilters.length == 3) {
            if (selectedFilters.indexOf(DESAFIO) === 0) {
                selectedFilters.shift();
            } else if (selectedFilters.indexOf(DESAFIO) === 2) {
                selectedFilters.pop();
            } else {
                selectedFilters = [selectedFilters[0], selectedFilters[2]];
            }
        }
    });

    $(`input[type=radio][name=${PROBLEMA}]`).change(function () {
        // console.log(this.value);
        $("#clearProblema").show();
        if (selectedFilters.indexOf(PROBLEMA) == -1) {
            selectedFilters.push(PROBLEMA);
        }
        allChallengesInSbns = [];
        allIssuesInSbns = [];
        allAreasInSbns = [];
        filterSbn({ problema: this.value });
    });

    $("#clearProblema").click(function () {
        $(`input[name=${PROBLEMA}]`).prop('checked', false);
        $("#clearProblema").hide();
        if (selectedFilters.length == 1) {
            filterRadios(DESAFIO, allChallenges);
            filterRadios(PROBLEMA, allIssues);
            filterRadios(AREA_APLICACION, allAreas);
            selectedFilters = [];
            $("#msg-without-sbn").hide();
            hideAllSbn();
        } else if (selectedFilters.length == 2) {
            if (selectedFilters.indexOf(PROBLEMA) === 0) {
                selectedFilters.shift();
                filterRadios(DESAFIO, allChallenges);
                filterRadios(AREA_APLICACION, allAreas);
            } else {
                selectedFilters.pop();
                if (selectedFilters[0] == AREA_APLICACION) {
                    filterSbn({ areaAplicacion: $("input[name='area-aplicacion']:checked").val() });
                } else {
                    filterSbn({ desafio: $("input[name='desafio']:checked").val() });
                }
            }
        } else if (selectedFilters.length == 3) {
            if (selectedFilters.indexOf(PROBLEMA) === 0) {
                selectedFilters.shift();
            } else if (selectedFilters.indexOf(PROBLEMA) === 2) {
                selectedFilters.pop();
            } else {
                selectedFilters = [selectedFilters[0], selectedFilters[2]];
            }
        }
    });

    $(".title-area-aplicacion").dblclick(e => {
        hideAllSbn();
    });

    $(".title-desafio").dblclick(e => {
        hideAllSbn();
    });

    $(".title-problema").dblclick(e => {
        hideAllSbn();
    });

    $('input:radio[name=problema]').each(function (e, l) {
        allIssues.push(l.id);
    });
    $('input:radio[name=desafio]').each(function (e, l) {
        allChallenges.push(l.id);
    });
    $(`input:radio[name=${AREA_APLICACION}]`).each(function (e, l) {
        allAreas.push(l.id);
    });

    let sbns = $(".sbn-filter-title");
    hidedChallenges = JSON.parse(JSON.stringify(allChallenges));
    hidedIssues = JSON.parse(JSON.stringify(allIssues));
    hidedAreas = JSON.parse(JSON.stringify(allAreas));
    sbns.each((i, sbn) => {
        let desafios = JSON.parse(sbn.getAttribute("challenges"));
        let problemas = JSON.parse(sbn.getAttribute("issues"));
        let areas = JSON.parse(sbn.getAttribute("areas"));
        desafios.forEach(d => {
            if (hidedChallenges.indexOf(d) != -1) {
                delete hidedChallenges[hidedChallenges.indexOf(d)];
            }
        });
        problemas.forEach(p => {
            if (hidedIssues.indexOf(p) != -1) {
                delete hidedIssues[hidedIssues.indexOf(p)];
            }
        });
        areas.forEach(a => {
            if (hidedAreas.indexOf(a) != -1) {
                delete hidedAreas[hidedAreas.indexOf(a)];
            }
        });
    });


    hidedIssues.forEach(el => {
        document.getElementById(el).parentElement.style.display = "none";
        delete allIssues[allIssues.indexOf(el)];
    });
    hidedAreas.forEach(el => {
        document.getElementById(el).parentElement.style.display = "none";
        delete allAreas[allAreas.indexOf(el)];
    });
    hidedChallenges.forEach(el => {
        document.getElementById(el).parentElement.style.display = "none";
        delete allChallenges[allChallenges.indexOf(el)];
    });
});

function hideAllSbn() {
    $(`input:radio[name=${AREA_APLICACION}]`).each(function () { $(this).prop('checked', false); });
    $(`input:radio[name=${DESAFIO}]`).each(function () { $(this).prop('checked', false); });
    $(`input:radio[name=${PROBLEMA}]`).each(function () { $(this).prop('checked', false); });
    let sbns = $(".sbn-filter-title");
    sbns.each((i, sbn) => {
        sbn.classList.add("sbn-hide");
    });
}

function filterSbn(filter) {
    let desafio;
    let areaAplicacion;
    let problema;
    if (filter.areaAplicacion) {
        areaAplicacion = filter.areaAplicacion;
        desafio = $(`input[name=${DESAFIO}]:checked`).val();
        problema = $(`input[name=${PROBLEMA}]:checked`).val();
    } else if (filter.desafio) {
        areaAplicacion = $(`input[name="${AREA_APLICACION}"]:checked`).val();
        desafio = filter.desafio;
        problema = $(`input[name=${PROBLEMA}]:checked`).val();
    } else if (filter.problema) {
        areaAplicacion = $(`input[name="${AREA_APLICACION}"]:checked`).val();
        desafio = $(`input[name=${DESAFIO}]:checked`).val();
        problema = filter.problema;
    }
    localStorage.setItem("areaAplicacion", areaAplicacion);
    localStorage.setItem(DESAFIO, desafio);
    localStorage.setItem(PROBLEMA, problema);

    let sbns = $(".sbn-filter-title");
    let countFilteredSbn = 0;
    sbns.each((i, sbn) => {
        if (sbn.childNodes[1].innerText.trim() == 'Restauración de ecosistemas forestales') {
            //console.log(sbn.childNodes[1].innerText);
        }
        let desafios = JSON.parse(sbn.getAttribute("challenges"));
        let problemas = JSON.parse(sbn.getAttribute("issues"));
        let areas = JSON.parse(sbn.getAttribute("areas"));
        let exist = true;
        if (desafio !== undefined && exist) {
            exist = (desafios.indexOf(desafio) != -1);
        } else {
            exist = false;
        }
        if (problema !== undefined && exist) {
            exist = (problemas.indexOf(problema) != -1);
        } else {
            exist = false;
        }

        if (areaAplicacion !== undefined && exist) {
            exist = (areas.indexOf(areaAplicacion) != -1);
        } else {
            exist = false;
        }

        let existeDesafio = desafio !== undefined ? (desafios.indexOf(desafio) != -1) : false;
        let existeProblema = problema !== undefined ? (problemas.indexOf(problema) != -1) : false;
        let existeArea = areaAplicacion !== undefined ? (areas.indexOf(areaAplicacion) != -1) : false;
        if (existeDesafio || existeProblema || existeArea) {
            sbn.classList.remove("sbn-hide");
            if (selectedFilters.length == 3) {
                let selectedFilter = selectedFilters[0];
                let secondSelectedFilter = selectedFilters[1];
                let thirdSelectedFilter = selectedFilters[2];

            } else if (selectedFilters.length == 2) {
                let selectedFilter = selectedFilters[0];
                let secondSelectedFilter = selectedFilters[1];
                switch (secondSelectedFilter) {
                    case AREA_APLICACION:
                        allAreasInSbns = [];
                        allAreasInSbns.push(areaAplicacion);
                        if (selectedFilter == DESAFIO) {
                            allIssuesInSbns = [];
                            problemas.forEach(p => {
                                if (allIssuesInSbns.indexOf(p) == -1) {
                                    allIssuesInSbns.push(p);
                                }
                            });
                        } else {
                            allChallengesInSbns = [];
                            desafios.forEach(d => {
                                if (allChallengesInSbns.indexOf(d) == -1) {
                                    allChallengesInSbns.push(d);
                                }
                            });
                        }

                        break;
                    case DESAFIO:
                        allChallengesInSbns = [];
                        allChallengesInSbns.push(desafio);
                        if (selectedFilter == AREA_APLICACION) {
                            allIssuesInSbns = [];
                            problemas.forEach(p => {
                                if (allIssuesInSbns.indexOf(p) == -1) {
                                    allIssuesInSbns.push(p);
                                }
                            });
                        } else {
                            allAreasInSbns = [];
                            areas.forEach(d => {
                                if (allAreasInSbns.indexOf(d) == -1) {
                                    allAreasInSbns.push(d);
                                }
                            });
                        }
                        break;
                    case PROBLEMA:
                        allIssuesInSbns = [];
                        allIssuesInSbns.push(problema);
                        if (selectedFilter == AREA_APLICACION) {
                            allChallengesInSbns = [];
                            desafios.forEach(d => {
                                if (allChallengesInSbns.indexOf(d) == -1) {
                                    allChallengesInSbns.push(d);
                                }
                            });
                        } else {
                            allAreasInSbns = [];
                            areas.forEach(d => {
                                if (allAreasInSbns.indexOf(d) == -1) {
                                    allAreasInSbns.push(d);
                                }
                            });
                        }
                        break;
                }
            } else if (selectedFilters.length > 0) {
                let selectedFilter = selectedFilters[0];
                switch (selectedFilter) {
                    case AREA_APLICACION:
                        desafios.forEach(d => {
                            if (allChallengesInSbns.indexOf(d) == -1) {
                                allChallengesInSbns.push(d);
                            }
                        });
                        problemas.forEach(p => {
                            if (allIssuesInSbns.indexOf(p) == -1) {
                                allIssuesInSbns.push(p);
                            }
                        });
                        break;
                    case DESAFIO:
                        areas.forEach(d => {
                            if (allAreasInSbns.indexOf(d) == -1) {
                                allAreasInSbns.push(d);
                            }
                        });
                        problemas.forEach(p => {
                            if (allIssuesInSbns.indexOf(p) == -1) {
                                allIssuesInSbns.push(p);
                            }
                        });
                        break;
                    case PROBLEMA:
                        areas.forEach(d => {
                            if (allAreasInSbns.indexOf(d) == -1) {
                                allAreasInSbns.push(d);
                            }
                        });
                        desafios.forEach(d => {
                            if (allChallengesInSbns.indexOf(d) == -1) {
                                allChallengesInSbns.push(d);
                            }
                        });
                        break;
                }

            }

            var bibliografias = sbn.getElementsByClassName("sbn-bibliografia");
            // console.log(sbn.childNodes[1].innerText.trim());
            let countBiblio = 0;
            Array.from(bibliografias).forEach(biblio => {
                let desafios = JSON.parse(biblio.getAttribute("challenges"));
                let problemas = JSON.parse(biblio.getAttribute("issues"));
                let areas = JSON.parse(biblio.getAttribute("areas"));
                let exist = false;

                if (selectedFilters.length == 1) {
                    let selectedFilter = selectedFilters[0];
                    switch (selectedFilter) {
                        case AREA_APLICACION:
                            exist = (areas.indexOf(areaAplicacion) != -1);
                            break;
                        case DESAFIO:
                            exist = (desafios.indexOf(desafio) != -1);
                            break;
                        case PROBLEMA:
                            exist = (problemas.indexOf(problema) != -1);
                            break;
                    }
                    if (exist) {
                        biblio.classList.remove("sbn-resource-hide");
                        biblio.classList.add("sbn-item");
                        countBiblio++;
                    } else {
                        biblio.classList.add("sbn-resource-hide");
                        biblio.classList.remove("sbn-item");
                    }
                } else if (selectedFilters.length == 2) {
                    let selectedFilter = selectedFilters[0];
                    let secondSelectedFilter = selectedFilters[1];
                    switch (selectedFilter) {
                        case AREA_APLICACION:
                            exist = (areas.indexOf(areaAplicacion) != -1);
                            switch (secondSelectedFilter) {
                                case DESAFIO:
                                    exist = exist && (desafios.indexOf(desafio) != -1);
                                    break;
                                case PROBLEMA:
                                    exist = exist && (problemas.indexOf(problema) != -1);
                                    break;
                            }
                            break;
                        case DESAFIO:
                            exist = (desafios.indexOf(desafio) != -1);
                            switch (secondSelectedFilter) {
                                case AREA_APLICACION:
                                    exist = exist && (areas.indexOf(areaAplicacion) != -1);
                                    break;
                                case PROBLEMA:
                                    exist = exist && (problemas.indexOf(problema) != -1);
                                    break;
                            }
                            break;
                        case PROBLEMA:
                            exist = (problemas.indexOf(problema) != -1);
                            switch (secondSelectedFilter) {
                                case AREA_APLICACION:
                                    exist = exist && (areas.indexOf(areaAplicacion) != -1);
                                    break;
                                case PROBLEMA:
                                    exist = exist && (problemas.indexOf(problema) != -1);
                                    break;
                            }
                            break;
                    }
                    if (exist) {
                        biblio.classList.remove("sbn-resource-hide");
                        biblio.classList.add("sbn-item");
                        countBiblio++;
                    } else {
                        biblio.classList.add("sbn-resource-hide");
                        biblio.classList.remove("sbn-item");
                    }
                } else if (selectedFilters.length == 3) {
                    var existArea = areas.indexOf(areaAplicacion) != -1;
                    var existProblema = (problemas.indexOf(problema) != -1);
                    var existDesafio = (desafios.indexOf(desafio) != -1);
                    exist = existArea && existProblema && existDesafio;
                    if (exist) {
                        biblio.classList.remove("sbn-resource-hide");
                        biblio.classList.add("sbn-item");
                        countBiblio++;
                    } else {
                        biblio.classList.add("sbn-resource-hide");
                        biblio.classList.remove("sbn-item");
                    }
                }
            });
            if (countBiblio == 0) {
                sbn.classList.add("sbn-hide");
                //$("#msg-without-sbn").show();
            } else {
                countFilteredSbn++;
                //$("#msg-without-sbn").hide();
                if (countBiblio < 5) {
                    $('.collapse').collapse('show');
                }
            }
        } else {
            sbn.classList.add("sbn-hide");
        }
        
        itemsHideShow(document.getElementsByClassName(`resource_${i} sbn-item`),i)
        

        var sbnsVisible = $(".sbn-filter-title").not(".sbn-hide");
        var b = $(sbns[0]).children(".mb-4").children("div").children("div").children("ul").children("li").not(".sbn-resource-hide");
        var c = $(sbns[0]).children(".mb-4").children("div").children("div").children("div").children("ul").children("li").not(".sbn-resource-hide");
        var d = b.length + c.length;
        var i = 0;
        var elWithMoreBiblio = 0;
        sbnsVisible.each((e, l) => {
            var b = $(l).children(".mb-4").children("div").children("div").children("ul").children("li").not(".sbn-resource-hide");
            var c = $(l).children(".mb-4").children("div").children("div").children("div").children("ul").children("li").not(".sbn-resource-hide");
            var e = b.length + c.length;
            if (e > d) {
                elWithMoreBiblio = i;
                d = e;
            }
            i++;
        });
        if (elWithMoreBiblio != 0) {
            $(sbnsVisible[elWithMoreBiblio]).insertBefore($(sbnsVisible[0]));
        }

    });

    (countFilteredSbn > 0) ? $("#msg-without-sbn").hide() : $("#msg-without-sbn").show();

    if (selectedFilters.length > 0 && selectedFilters.length < 3) {
        let selectedFilter = selectedFilters[0];
        switch (selectedFilter) {
            case AREA_APLICACION:
                if (selectedFilters.length == 1) {
                    filterRadios(DESAFIO, allChallengesInSbns);
                    filterRadios(PROBLEMA, allIssuesInSbns);
                } else {
                    let secondSelectedFilter = selectedFilters[1];
                    switch (secondSelectedFilter) {
                        case DESAFIO:
                            filterRadios(PROBLEMA, allIssuesInSbns);
                            break;
                        case PROBLEMA:
                            filterRadios(DESAFIO, allChallengesInSbns);
                            break;
                    }
                }
                break;
            case DESAFIO:
                if (selectedFilters.length == 1) {
                    filterRadios(AREA_APLICACION, allAreasInSbns);
                    filterRadios(PROBLEMA, allIssuesInSbns);
                } else {
                    let secondSelectedFilter = selectedFilters[1];
                    switch (secondSelectedFilter) {
                        case AREA_APLICACION:
                            filterRadios(PROBLEMA, allIssuesInSbns);
                            break;
                        case PROBLEMA:
                            filterRadios(AREA_APLICACION, allAreasInSbns);
                            break;
                    }
                }
                break;
            case PROBLEMA:
                if (selectedFilters.length == 1) {
                    filterRadios(AREA_APLICACION, allAreasInSbns);
                    filterRadios(DESAFIO, allChallengesInSbns);
                } else {
                    let secondSelectedFilter = selectedFilters[1];
                    switch (secondSelectedFilter) {
                        case AREA_APLICACION:
                            filterRadios(DESAFIO, allChallengesInSbns);
                            break;
                        case DESAFIO:
                            filterRadios(AREA_APLICACION, allAreasInSbns);
                            break;
                    }
                }
                break;
        }
    }
}

function filterRadios(groupName, list) {
    let radios = $(`input:radio[name=${groupName}]`);
    radios.each((i, l) => {
        if (list.indexOf(l.id) == -1) {
            $(l.parentElement).hide();
        } else {
            $(l.parentElement).show();
        }
    });

}
function itemsHideShow(itemsRecursosBibli, i) {
    itemsRecursosBibli = [...itemsRecursosBibli]
    // validamos tamaño de todos los items filtrados ( hide/show ) para que se muestre el boton
    if (itemsRecursosBibli.length > 6) {
        document.getElementById(`botonRef_${i}`).classList.remove('sbn-resource-hide');
        let hide = itemsRecursosBibli.slice(5, itemsRecursosBibli.length - 1)
        let show = itemsRecursosBibli.slice(0, 4)
        itemsRecursosBibli.map(
            (element) => {
                if (!(element.classList.contains('sbn-resource-hide'))) {
                    hide.map(
                        op => {
                            op.classList.add(`sbn-resource-hide`)
                            op.classList.remove('show')
                            op.classList.remove('sbn-item')
                        })
                }
            }
        )
        document.getElementById(`botonRef_${i}`).onclick = toShow
        
        function toShow(event) {
            hide.map(
                op => {
                    if (op.classList.contains('sbn-resource-hide')) {
                        op.classList.remove('sbn-resource-hide')
                        op.classList.add('sbn-item')
                    } else {
                        op.classList.add('sbn-resource-hide')
                        op.classList.remove('sbn-item')
                    }
                })
        }
    } else {
        document.getElementById(`botonRef_${i}`).classList.add('sbn-resource-hide');
        document.getElementById(`botonRef_${i}`).onclick = toShow
    }
    
}