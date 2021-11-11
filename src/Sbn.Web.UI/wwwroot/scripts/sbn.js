var r;
window.onload = function () {
    var labels;
    r = Raphael("holder", 850, 600),
        R = 150,
        init = true,
        param = {stroke: "#fff", "stroke-width": 30},
        attr = {font: "16px Open Sans", opacity: 0.9};
        font2 = {font: "16px Open Sans", opacity: 0.1};
        hash = document.location.hash,
        marksAttr = {fill: hash || "#444", stroke: "none"}
    // Custom Attribute
    r.customAttributes.arc = function (value, total, R) {
        var alpha = 360 / total * value,
            a = (90 - alpha) * Math.PI / 180,
            x = 300 + R * Math.cos(a),
            y = 300 - R * Math.sin(a),
            color = "hsb(".concat(Math.round(R) / 200, ",", value / total, ", .75)"),
            path;
        color = "#bad405";
        if (total == value) {
            path = [["M", 300, 300 - R], ["A", R, R, 0, 1, 1, 299.99, 300 - R]];
        } else {
            path = [["M", 300, 300 - R], ["A", R, R, 0, +(alpha > 180), 1, x, y]];
        }
        
        if (total == 60){
            color = "#ddd";
            path = [["M", 300, 300 - R], ["A", R, R, 0, +(alpha > 180), 1, x, y]];
        }
        
        return {path: path, stroke: color};
    };
    
    var circle2 = r.circle(300, 300, 300).attr({stroke: "none", fill: "#eee"});
    var circle1 = r.circle(300, 300, 200).attr({stroke: "none", fill: "#ddd"});

    drawMarks(R, 60);
    var sec = r.path().attr(param).attr({arc: [0, 60, R]});
    R -= 40;
    //drawMarks(R, 60);
    var min = r.path().attr(param).attr({arc: [0, 60, R]});
    R -= 40;
    drawMarks(R, 12);
    var hor = r.path().attr(param).attr({arc: [0, 12, R]});
    R -= 40;
    drawMarks(R, 31);
    var day = r.path().attr(param).attr({arc: [0, 31, R]});
    R -= 40;
    //drawMarks(R, 12);
    var mon = r.path().attr(param).attr({arc: [0, 12, R]});
    labels = r.set();
    labels.push(r.text(300, 100, "Preparación").attr(attr),
                r.text(478, 200, "Formulacion").attr(attr),
                r.text(465, 420, "Planificación").attr(attr),
                r.text(128, 420, "Implementación").attr(attr),
                r.text(126, 200, "Seguimiento").attr(attr)).attr("cursor", "pointer").hover(function(e){
            e.target.setAttribute("fill","#0a1d29");
        }, function(e){
            e.target.setAttribute("fill","#1B4F72");
        });
    
    var lblsPreparacion = r.set();
    var lblsFormulacion = r.set();
    var lblsPlanificacion = r.set();
    var lblsImplementacion = r.set();
    var lblsSeguimiento = r.set();

    var steps = preparation.steps;
    var data = { preparacion: {
            rect: {x:240,y:86,w:120,h:30},
            steps:[{x:509, y:20, label: "Diagnóstico Territorial", detail: steps[0].detail},
            {x:612, y:90, label:"Definición del problema", detail: steps[1].detail},
            {x:659, y:160, label:"Análisis del marco legal", detail: steps[2].detail},
            {x:660, y:230, label:"Mapa de actores", detail: steps[3].detail},
            {x:702, y:300, label:"Fuentes de financiamiento", detail: steps[4].detail}]
        },formulacion: {
            rect: {x:428,y:186,w:110,h:30},
            steps:[{x:505, y:20, label: "Delimitación del área", detail: steps[0].detail},
            {x:612, y:90, label:"Definición de mecanismos \n de participación", detail: steps[1].detail},
            {x:675, y:160, label:"Estrategia de comunicación", detail: steps[2].detail},
            {x:713, y:230, label:"Definición de objetivos y metas", detail: steps[3].detail},
            {x:715, y:300, label:"Identificación de alternativas", detail: steps[4].detail}]
        },planificacion: {
            rect: {x:415,y:406,w:110,h:30},
            steps:[{x:535, y:20, label: "Análisis de viabilidad y riesgos", detail: steps[0].detail},
            {x:622, y:90, label:"Selección de equipo técnico", detail: steps[1].detail},
            {x:685, y:160, label:"Creación de diseños y/o planos", detail: steps[2].detail},
            {x:690, y:230, label:"Construcción del sistema \n de monitoreo", detail: steps[3].detail},
            {x:715, y:300, label:"Plan de acción y presupuesto", detail: steps[4].detail}]
        },implementacion: {
            rect: {x:67,y:406,w:125,h:30},
            steps:[{x:645, y:90, label:"Firma de acuerdos de gobernanza", detail: steps[0].detail},
            {x:675, y:160, label:"Ejecución de intervenciones", detail: steps[1].detail},
            {x:685, y:230, label:"Monitoreo participativo", detail: steps[2].detail},
            {x:715, y:300, label: "Acciones de mantenimiento", detail: steps[3].detail},]
        },seguimiento: {
            rect: {x:73,y:186,w:102,h:30},
            steps:[{x:620, y:90, label:"Evaluación participativa", detail: steps[0].detail},
            {x:665, y:160, label:"Definición de propuestas \n de ajuste", detail: steps[1].detail}]
        }
    };
    
    var rectPreparacion = createEtapa(data.preparacion,lblsPreparacion, [lblsFormulacion, lblsPlanificacion, lblsImplementacion, lblsSeguimiento]);

    addAttrs2Rects(rectPreparacion);
    
    var rectFormulacion = createEtapa(data.formulacion, lblsFormulacion, [lblsPreparacion,lblsPlanificacion, lblsImplementacion, lblsSeguimiento]);

    addAttrs2Rects(rectFormulacion);
    
    var rectPlanificacion = createEtapa(data.planificacion, lblsPlanificacion, [lblsPreparacion,lblsFormulacion, lblsImplementacion, lblsSeguimiento]);

    addAttrs2Rects(rectPlanificacion);
    var rectImplementacion = createEtapa(data.implementacion, lblsImplementacion, [lblsPreparacion,lblsFormulacion,lblsPlanificacion, lblsSeguimiento]); //r.rect(67, 406, 125, 30);
    addAttrs2Rects(rectImplementacion);
    var rectSeguimiento = createEtapa(data.seguimiento, lblsSeguimiento, [lblsPreparacion,lblsFormulacion,lblsPlanificacion,lblsImplementacion]); //r.rect(73, 186, 102, 30);
    addAttrs2Rects(rectSeguimiento);
    
    function createEtapa(etapa, labels, lbls2Remove){
        var rectEtapa = r.rect(etapa.rect.x, etapa.rect.y, etapa.rect.w, etapa.rect.h).click(function(e){
            var attr = {font: "15px Open Sans", opacity: 0.9, "font-weight": 'bold', fill: "#1B4F72"};
            lbls2Remove.forEach(function (lbls){
                lbls.forEach(function(l){
                    l.remove();
                });  
            });
            etapa.steps.forEach(function (s){
                labels.push(r.text(s.x,s.y,s.label).attr(attr));
            });
            labels.attr("cursor", "pointer");
            labels.click(function(el){
                $(".titulo-step").html(el.target.innerHTML);
                $(".news-wrapper").show();
            }).hover(function(e){
                e.target.setAttribute("fill","#0a1d29");
            }, function(e){
                e.target.setAttribute("fill","#5ba8d7");
            });
        });
        return rectEtapa;
    }
    
    function addAttrs2Rects(r){
        r.attr("fill","#fefefe").attr("stroke","#ccc").attr("cursor", "pointer")
            .attr("fill-opacity","0.5").hover(function(e){
            e.target.setAttribute("fill","#bad405");
        }, function(e){
            e.target.setAttribute("fill","#fefefe");
        });
    }
    
    function updateVal(value, total, R, hand, id) {
        if (total == 31) { // month
            var d = new Date;
            d.setDate(1);
            d.setMonth(d.getMonth() + 1);
            d.setDate(-1);
            total = d.getDate();
        }
        var color = "hsb(".concat(Math.round(R) / 200, ",", value / total, ", .75)");
        if (init) {
            hand.animate({arc: [value, total, R]}, 900, ">");
        } else {
            if (!value || value == total) {
                value = total;
                hand.animate({arc: [value, total, R]}, 750, "bounce", function () {
                    hand.attr({arc: [0, total, R]});
                });
            } else {
                hand.animate({arc: [value, total, R]}, 750, "elastic");
            }
        }
        
    }

    function drawMarks(R, total) {

        var color = "hsb(".concat(Math.round(R) / 200, ", 1, .75)"),
            out = r.set();
        for (var value = 0; value < total; value++) {
            var alpha = 360 / total * value,
                a = (90 - alpha) * Math.PI / 180,
                x = 300 + R * Math.cos(a),
                y = 300 - R * Math.sin(a);
            out.push(r.circle(x, y, 2).attr(marksAttr));
        }
        return out;
    }

    (function () {
        
        //updateVal(d.getMinutes(), 60, 160, min, 1);
        //updateVal(h, 12, 120, hor, 0);
        //updateVal(31, 31, 160, day, 3);
        updateVal(12, 12, 120, mon, 4);
        //pm[(am ? "hide" : "show")]();
        //setTimeout(arguments.callee, 1000);
        init = false;
    })();
};