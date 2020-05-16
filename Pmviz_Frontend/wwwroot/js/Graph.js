//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

function renderCytoscapeElementConformance(cy) {
    cy = cytoscape(
        {
            wheelSensitivity: 0.5,
            minZoom: 0.1,
            maxZoom: 1,
            container: document.getElementById('cyCompare'),
            style: [
                //Nodes styles
                {
                    //white
                    selector: 'node[type=0]',
                    style: {
                        "shape": 'rectangle',
                        "background-color": "#FFFFFF",
                        "label": "data(label)",
                        'width': '350',
                        "height": "40",
                        "border-width": 4,
                        "border-color": "#484848",
                        "font-size": "16px",
                        "text-valign": "center",
                        "text-halign": "center",
                        "text-wrap": "wrap",
                        "text-max-width": "1000px",
                        "color": "#222222"
                    }
                },
                {
                    //green with green text
                    selector: 'node[type=1]',
                    style: {
                        "shape": 'rectangle',
                        "background-color": "#FFFFFF",
                        "label": "data(label)",
                        'width': '350',
                        "height": "40",
                        "border-width": 4,
                        "border-color": "#82EB71",
                        "font-size": "16px",
                        "text-valign": "center",
                        "text-halign": "center",
                        "text-wrap": "wrap",
                        "text-max-width": "1000px",
                        "color": "#82EB71",
                        "text-outline-color": "#222222",
                        "text-outline-width": "0.15px"
                    }
                },
                {
                    //red with green text
                    selector: 'node[type=2]',
                    style: {
                        "shape": 'rectangle',
                        "background-color": "#FFFFFF",
                        "label": "data(label)",
                        'width': '350',
                        "height": "40",
                        "border-width": 4,
                        "border-color": "#FF4E4E",
                        "font-size": "16px",
                        "text-valign": "center",
                        "text-halign": "center",
                        "color": "#82EB71"
                    }
                },
                {
                    //green with red text
                    selector: 'node[type=3]',
                    style: {
                        "shape": 'rectangle',
                        "background-color": "#FFFFFF",
                        "label": "data(label)",
                        'width': '350',
                        "height": "40",
                        "border-width": 4,
                        "border-color": "#82EB71",
                        "font-size": "16px",
                        "text-valign": "center",
                        "text-halign": "center",
                        "text-wrap": "wrap",
                        "text-max-width": "1000px",
                        "color": "#FF4E4E"
                    }
                },
                {
                    //red with red text
                    selector: 'node[type=4]',
                    style: {
                        "shape": 'rectangle',
                        "background-color": "#FFFFFF",
                        "label": "data(label)",
                        'width': '350',
                        "height": "40",
                        "border-width": 4,
                        "border-color": "#FF4E4E",
                        "font-size": "16px",
                        "text-valign": "center",
                        "text-halign": "center",
                        "text-wrap": "wrap",
                        "text-max-width": "1000px",
                        "color": "#FF4E4E"
                    }
                },
                {
                    //process start node
                    selector: 'node[type=20]',
                    style: {
                        "shape": 'ellipse',
                        "background-color": "#3f3f3f",
                        "border-width": 4,
                        "border-color": "#131313",
                        'width': '50',
                        "height": "50",
                        "font-size": "16px",
                        "text-valign": "center",
                        "text-halign": "center",
                        "text-wrap": "wrap",
                        "text-max-width": "1000px",
                        "color": "#FF2222"
                    }
                },

                //Edges styles
                {
                    //grey
                    selector: 'edge[type=0]',
                    style: {
                        'width': 5,
                        'curve-style': 'bezier',
                        "line-color": "#989898",
                        'target-arrow-color': '#989898',
                        "font-size": "32px",
                        "color": "#222222",
                        "loop-direction": "0deg",
                        'target-arrow-shape': 'triangle',
                        "loop-sweep": "45deg",
                        "text-margin-y": "-15px",
                        "source-text-offset": "50px",
                        "text-outline-color": "#222222",
                        "text-outline-width": "0.3px"
                    }
                },
                {
                    //green with green text
                    selector: 'edge[type=1]',
                    style: {
                        'width': 5,
                        'curve-style': 'bezier',
                        "line-color": "#82EB71",
                        'target-arrow-color': '#82EB71',
                        "label": "data(label)",
                        "font-size": "32px",
                        "color": "#82EB71",
                        "loop-direction": "0deg",
                        'target-arrow-shape': 'triangle',
                        "loop-sweep": "45deg",
                        "text-outline-color": "#222222",
                        "text-outline-width": "0.3px",
                        "text-rotation": "0deg",
                        "text-margin-y": "-20px"
                    }
                },
                {
                    //red with green text
                    selector: 'edge[type=2]',
                    style: {
                        'width': 5,
                        'curve-style': 'bezier',
                        "line-color": "#FF4E4E",
                        'target-arrow-color': '#FF4E4E',
                        "content": "data(label)",
                        "font-size": "32px",
                        "color": "#82EB71",
                        "loop-direction": "0deg",
                        'target-arrow-shape': 'triangle',
                        "loop-sweep": "45deg",
                        "text-margin-y": "-15px",
                        "source-text-offset": "50px",
                        "text-outline-color": "#222222",
                        "text-outline-width": "0.3px",
                        "text-rotation": "0deg"
                    }
                },
                {
                    //red with red text
                    selector: 'edge[type=4]',
                    style: {
                        'width': 5,
                        'curve-style': 'bezier',
                        "line-color": "#FF4E4E",
                        'target-arrow-color': '#FF4E4E',
                        "content": "data(label)",
                        "font-size": "32px",
                        "color": "#FF4E4E",
                        "loop-direction": "0deg",
                        'target-arrow-shape': 'triangle',
                        "loop-sweep": "45deg",
                        "text-margin-y": "-15px",
                        "source-text-offset": "50px",
                        "text-outline-color": "#222222",
                        "text-outline-width": "0.3px",
                        "text-rotation": "0deg"
                    }
                },
                {
                    //green with red text
                    selector: 'edge[type=5]',
                    style: {
                        'width': 5,
                        'curve-style': 'bezier',
                        "line-color": "#82EB71",
                        'target-arrow-color': '#82EB71',
                        "content": "data(label)",
                        "font-size": "32px",
                        "color": "#FF4E4E",
                        "loop-direction": "0deg",
                        'target-arrow-shape': 'triangle',
                        "loop-sweep": "45deg",
                        "text-margin-y": "-15px",
                        "source-text-offset": "50px",
                        "text-outline-color": "#222222",
                        "text-outline-width": "0.3px",
                        "text-rotation": "0deg"
                    }
                },

                {
                    //green with green text
                    selector: 'edge[type=6]',
                    style: {
                        'width': 5,
                        'curve-style': 'bezier',
                        "line-color": "#82EB71",
                        'target-arrow-color': '#82EB71',
                        "label": "data(label)",
                        "font-size": "32px",
                        "color": "#82EB71",
                        "loop-direction": "0deg",
                        'target-arrow-shape': 'triangle',
                        "loop-sweep": "45deg",
                        "text-outline-color": "#222222",
                        "text-outline-width": "0.3px",
                        "text-margin-y": "20px"
                    }
                },


                //temporary type to flag node as having non conform time, and to later be changed to type 5 if it is associated to the case (if not stays grey)
                {
                    selector: 'edge[type=10]',
                    style: {
                        'width': 5,
                        'curve-style': 'bezier',
                        "line-color": "#989898",
                        'target-arrow-color': '#989898',
                        "font-size": "32px",
                        "color": "#222222",
                        "loop-direction": "0deg",
                        'target-arrow-shape': 'triangle',
                        "loop-sweep": "45deg",
                        "text-margin-y": "-15px",
                        "source-text-offset": "50px",
                        "text-outline-color": "#222222",
                        "text-outline-width": "0.3px",
                        "text-rotation": "0deg"
                    }
                },
                {
                    //process start edge
                    selector: 'edge[type=20]',
                    style: {
                        'width': 8,
                        'curve-style': 'bezier',
                        'line-color': "#232323",
                        'target-arrow-color': '#232323',
                        "font-size": "32px",
                        "color": "#222222",
                        "loop-direction": "0deg",
                        'target-arrow-shape': 'triangle',
                        "loop-sweep": "45deg",
                        "text-margin-y": "-15px",
                        "source-text-offset": "50px",
                        "text-outline-color": "#222222",
                        "text-outline-width": "0.3px"
                    }
                },
            ],
        });

    //Nodes\\
    for (let i = 0; i < process.baseData.nodes.length; i++) {
        //determinar cor do nó
        let typeValue = 0;

        //verificar se faz parte da Path do case escolhido -> verde
        for (let k = 0; k < process.caseData.nodes.length; k++) {
            if (process.caseData.nodes[k].node === i) {
                typeValue = 1;
            }
        }

        //verificar se tempo execução é não conforme -> texto vermelho
        //procurar a ocorrência do nó em questão com maior duração (pior caso)
        let maxDuration = {};
        maxDuration.days = 0;
        maxDuration.hours = 0;
        maxDuration.minutes = 0;
        maxDuration.seconds = 0;

        for (let k = 0; k < process.caseData.nodes.length; k++) {
            if (process.caseData.nodes[k].node === i && convertToSeconds(process.caseData.nodes[k].duration) > convertToSeconds(maxDuration)) {
                maxDuration = process.caseData.nodes[k].duration;
            }
        }

        if (convertToSeconds(maxDuration) > convertToSeconds(process.baseData.taskDurations[i].duration)) {
            typeValue = 3;
        }

        //format label acording to time value
        let prevLabel;
        if (process.baseData.taskDurations[i].duration.days !== 0) {
            prevLabel = "\n Prev: " + process.baseData.taskDurations[i].duration.days + "D " + process.baseData.taskDurations[i].duration.hours + "H " + process.baseData.taskDurations[i].duration.minutes + "M " + process.baseData.taskDurations[i].duration.seconds + "S";
        } else if (process.baseData.taskDurations[i].duration.hours !== 0) {
            prevLabel = "\n Prev: " + process.baseData.taskDurations[i].duration.hours + "H " + process.baseData.taskDurations[i].duration.minutes + "M " + process.baseData.taskDurations[i].duration.seconds + "S";
        } else if (process.baseData.taskDurations[i].duration.minutes !== 0) {
            prevLabel = "\n Prev: " + process.baseData.taskDurations[i].duration.minutes + "M " + process.baseData.taskDurations[i].duration.seconds + "S";
        } else {
            prevLabel = "\n Prev: " + process.baseData.taskDurations[i].duration.seconds + "S";
        }

        let realLabel;
        if (process.baseData.taskDurations[i].duration.days !== 0) {
            realLabel = " / Real: " + maxDuration.days + "D " + maxDuration.hours + "H " + maxDuration.minutes + "M " + maxDuration.seconds + "S";
        } else if (process.baseData.taskDurations[i].duration.hours !== 0) {
            realLabel = " / Real: " + maxDuration.hours + "H " + maxDuration.minutes + "M " + maxDuration.seconds + "S";
        } else if (process.baseData.taskDurations[i].duration.minutes !== 0) {
            realLabel = " / Real: " + maxDuration.minutes + "M " + maxDuration.seconds + "S";
        } else {
            realLabel = " / Real: " + maxDuration.seconds + "S";
        }

        //se typevalue for 0 (não faz parte do caso em análise) formata texto de forma diferente
        if (typeValue === 0) {
            cy.add({
                data: {
                    id: i,
                    label: process.baseData.nodes[i] + prevLabel,
                    type: typeValue
                },
            }
            );
        } else {
            cy.add({
                data: {
                    id: i,
                    label: process.baseData.nodes[i] + prevLabel + realLabel,
                    type: typeValue
                },
            }
            );
        }

    }

    //Edges\\
    for (let i = 0; i < process.baseData.relations.length; i++) {
        for (let j = 0; j < process.baseData.relations[i].to.length; j++) {

            //search for max duration for this relation, if it occurs more then once
            let maxDuration = {};
            maxDuration.days = 0;
            maxDuration.hours = 0;
            maxDuration.minutes = 0;
            maxDuration.seconds = 0;
            let edgeTypeValue = 0;
            for (let k = 0; k < process.caseData.relations.length; k++) {
                if (process.caseData.relations[k].from === process.baseData.relations[i].from) {
                    for (let l = 0; l < process.caseData.relations[k].to.length; l++) {
                        if (process.caseData.relations[k].to[l].node === process.baseData.relations[i].to[j].node) {
                            if (convertToSeconds(process.caseData.relations[k].to[l].duration) > convertToSeconds(maxDuration)) {
                                maxDuration = process.caseData.relations[k].to[l].duration;
                            }
                        }
                    }
                }
            }

            //mark edge as having nonconform time
            if (convertToSeconds(maxDuration) > convertToSeconds(process.baseData.relations[i].to[j].duration)) {
                edgeTypeValue = 10;
            }

            let prevLabel = 0;
            if (process.baseData.relations[i].to[j].duration.days !== 0) {
                prevLabel = "\n Prev: " + process.baseData.relations[i].to[j].duration.days + "D " + process.baseData.relations[i].to[j].duration.hours + "H " + process.baseData.relations[i].to[j].duration.minutes + "M ";
            } else if (process.baseData.relations[i].to[j].duration.hours !== 0) {
                prevLabel = "\n Prev: " + process.baseData.relations[i].to[j].duration.hours + "H " + process.baseData.relations[i].to[j].duration.minutes + "M ";
            } else if (process.baseData.relations[i].to[j].duration.minutes !== 0) {
                prevLabel = "\n Prev: " + process.baseData.relations[i].to[j].duration.minutes + "M " + process.baseData.relations[i].to[j].duration.seconds + "S";
            } else {
                prevLabel = "\n Prev: " + process.baseData.relations[i].to[j].duration.seconds + "S";
            }

            let realLabel = 0;
            if (maxDuration.days !== 0) {
                realLabel = "Real: " + maxDuration.days + "D " + maxDuration.hours + "H " + maxDuration.minutes + "M ";
            } else if (maxDuration.hours !== 0) {
                realLabel = "Real: " + maxDuration.hours + "H " + maxDuration.minutes + "M ";
            } else if (maxDuration.minutes !== 0) {
                realLabel = "Real: " + maxDuration.minutes + "M " + maxDuration.seconds + "S";
            } else {
                realLabel = "Real: " + maxDuration.seconds + "S";
            }

            cy.add({
                data: {
                    id: 'edge' + process.baseData.relations[i].from + '-' + process.baseData.relations[i].to[j].node,
                    source: process.baseData.relations[i].from,
                    target: process.baseData.relations[i].to[j].node,
                    label: prevLabel + " / " + realLabel,
                    type: edgeTypeValue
                }
            });
        }
    }

    let edges = [];
    //identify realtions which are part of the case(s) selected
    for (let i = 0; i < process.caseData.relations.length; i++) {
        for (let j = 0; j < process.caseData.relations[i].to.length; j++) {

            //check if already exists connection between the two nodes, in order to shift the text slightly so it doesn't overlap
            let control = 0;
            let edge = process.caseData.relations[i].from + "-" + process.caseData.relations[i].to[j].node;
            for (let k = 0; k < edges.length; k++) {
                if (edges[k] === process.caseData.relations[i].to[j].node + "-" + process.caseData.relations[i].from) {
                    control = 1;
                }
            }
            edges.push(edge);


            if (cy.getElementById("edge" + process.caseData.relations[i].from + "-" + process.caseData.relations[i].to[j].node).data('type') === 10) {
                cy.getElementById("edge" + process.caseData.relations[i].from + "-" + process.caseData.relations[i].to[j].node).data('type', 5);
            } else {
                if (control === 0) {
                    cy.getElementById("edge" + process.caseData.relations[i].from + "-" + process.caseData.relations[i].to[j].node).data('type', 1);
                } else {
                    cy.getElementById("edge" + process.caseData.relations[i].from + "-" + process.caseData.relations[i].to[j].node).data('type', 6);
                }

            }
        }
    }

    //change nodes which are outliers to red with green or red text
    for (let i = 0; i < process.baseData.deviations.nodes.length; i++) {
        if (cy.getElementById(process.baseData.deviations.nodes[i].toString()).data('type') === 3) {
            cy.getElementById(process.baseData.deviations.nodes[i].toString()).data('type', 4);
        } else if (cy.getElementById(process.baseData.deviations.nodes[i].toString()).data('type') === 1) {
            cy.getElementById(process.baseData.deviations.nodes[i].toString()).data('type', 2);
        } else {
            //cy.remove(cy.getElementById("edge" + process.baseData.deviations.nodes[i].from + "-" + process.baseData.deviations.nodes[i].to))
        }
    }

    //change edges which are outliers to red with green or red text
    for (let i = 0; i < process.baseData.deviations.relations.length; i++) {
        if (cy.getElementById("edge" + process.baseData.deviations.relations[i].from + "-" + process.baseData.deviations.relations[i].to).data('type') === 5) {
            cy.getElementById("edge" + process.baseData.deviations.relations[i].from + "-" + process.baseData.deviations.relations[i].to).data('type', 4);
        } else if (cy.getElementById("edge" + process.baseData.deviations.relations[i].from + "-" + process.baseData.deviations.relations[i].to).data('type') === 1) {
            cy.getElementById("edge" + process.baseData.deviations.relations[i].from + "-" + process.baseData.deviations.relations[i].to).data('type', 2);
        } else {
            //cy.remove(cy.getElementById("edge" + process.baseData.deviations.relations[i].from + "-" + process.baseData.deviations.relations[i].to))
        }
    }

    /*cy.getElementById("edge6-18_conformance").data('name', "8")*/

    //add process start and end nodes
    for (let i = 0; i < process.caseData.startEvents.length; i++) {
        cy.add({
            data: {
                id: 'start-' + i,
                type: 20
            },
        });

        cy.add({
            data: {
                id: 'edge_start-' + i,
                source: 'start-' + i,
                target: process.caseData.startEvents[i].node,
                type: 20
            }
        });
    }

    for (let i = 0; i < process.caseData.endEvents.length; i++) {
        cy.add({
            data: {
                id: 'end-' + i,
                type: 20
            },
        });
        console.log(process.caseData.endEvents[i]);
        cy.add({
            data: {
                id: 'edge_end-' + i,
                source: process.caseData.endEvents[i].node,
                target: 'end-' + i,
                type: 20
            }
        });
    }


    let customBreadthfirst = {
        name: 'breadthfirst',

        fit: true, // whether to fit the viewport to the graph
        directed: true, // whether the tree is directed downwards (or edges can point in any direction if false)
        padding: 10, // padding on fit
        circle: false, // put depths in concentric circles if true, put depths top down if false
        grid: false, // whether to create an even grid into which the DAG is placed (circle:false only)
        spacingFactor: 0.90, // positive spacing factor, larger => more space between nodes (N.B. n/a if causes overlap)
        boundingBox: undefined, // constrain layout bounds; { x1, y1, x2, y2 } or { x1, y1, w, h }
        avoidOverlap: true, // prevents node overlap, may overflow boundingBox if not enough space
        nodeDimensionsIncludeLabels: false, // Excludes the label when calculating node bounding boxes for the layout algorithm
        roots: undefined, // the roots of the trees
        maximal: false, // whether to shift nodes down their natural BFS depths in order to avoid upwards edges (DAGS only)
        animate: false, // whether to transition the node positions
        animationDuration: 500, // duration of animation in ms if enabled
        animationEasing: undefined, // easing of animation if enabled,
        animateFilter: function (node, i) {
            return true;
        }, // a function that determines whether the node should be animated.  All nodes animated by default on animate enabled.  Non-animated nodes are positioned immediately when the layout starts
        ready: undefined, // callback on layoutready
        stop: undefined, // callback on layoutstop
        transform: function (node, position) {
            return position;
        } // transform a given node position. Useful for changing flow direction in discrete layouts
    };
    cy.layout(customBreadthfirst).run();
}


function convertToSeconds(duration) {
    return duration.days * 86400 + duration.hours * 3600 + duration.minutes * 60 + duration.seconds;
}