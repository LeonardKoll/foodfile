import React, {useRef, useEffect} from 'react';
import * as d3 from "d3";
import "./TraceTree.css";
import {emptyHierarchyItem} from './TraceTreeUtils'

function placeTreeSVG (treeData, containerRef)
{   
    d3.select(containerRef).select("svg").remove();
    if (treeData === undefined)
        return

    // set the dimensions and margins of the diagram
    var margin = {top: 40, right: 120, bottom: 100, left: 40};
    var width = containerRef.offsetWidth - margin.left - margin.right;
    var height = 700 - margin.top - margin.bottom;

    // declares a tree layout and assigns the size
    var treemap = d3.tree().size([width, height]);

    //  assigns the data to a hierarchy using parent-child relationships
    var nodes = d3.hierarchy(treeData);
    // maps the node data to the tree layout
    nodes = treemap(nodes);

    // append the svg obgect to the body of the page
    // appends a 'group' element to 'svg'
    // moves the 'group' element to the top left margin
    var svg = d3.select(containerRef).append("svg")
        .attr("width", width + margin.left + margin.right)
        .attr("height", height + margin.top + margin.bottom);
    var g = svg.append("g")
        .attr("transform", "translate(" + margin.left + "," + margin.top + ")");

    // adds the links between the nodes
    g.selectAll(".link")
        .data( nodes.descendants().slice(1))
        .enter().append("path")
        .attr("class", "link")
        .attr("d", function(d) {
            return "M" + d.x + "," + d.y
                + "C" + d.x + "," + (d.y + d.parent.y) / 2
                + " " + d.parent.x + "," +  (d.y + d.parent.y) / 2
                + " " + d.parent.x + "," + d.parent.y;
            });

    // adds each node as a group
    var node = g.selectAll(".node")
        .data(nodes.descendants())
        .enter().append("g")
        .attr("class", function(d) { 
            return "node" + (d.children ? " node--internal" : " node--leaf");
            })
        .attr("transform", function(d) { 
            return "translate(" + d.x + "," + d.y + ")";
            });
    
    // adds the circle to the node
    node.append("circle").attr("r", 5);

    // adds the text to the node
    node.append("text")
        .attr("y", 20 ).attr("x", 10)
        .text(function(d) { return d.data.info.length > 0 ? d.data.info[0] : "" ; });
    node.append("text")
        .attr("y", 37).attr("x", 10)
        .style("font-weight", "bold")
        .text(function(d) { return d.data.info.length > 1 ? d.data.info[1] : "" ; });
    node.append("text")
        .attr("y", 54).attr("x", 10)
        .text(function(d) { return d.data.info.length > 2 ? d.data.info[2] : "" ; });
    node.append("text")
        .attr("y", 71).attr("x", 10)
        .text(function(d) { return d.data.info.length > 3 ? d.data.info[3] : "" ; });
    node.append("text")
        .attr("y", 88).attr("x", 10)
        .text(function(d) { return d.data.info.length > 4 ? d.data.info[4] : "" ; });

}

function TraceTree({treeData})
{
    const containerRef = useRef(null);

    useEffect(() => {

        placeTreeSVG(treeData, containerRef.current);  
        
        }, [treeData]); //entities.length,members.length
        /*
            If you pass an object, React will store only the reference to the object and run the effect when the reference changes,
            which is usually every singe time (I don't now how though).
            => Mention primitive values here.

            https://stackoverflow.com/questions/53070970/infinite-loop-in-useeffect
        */

    return (<div ref={containerRef}></div>);
}

export default TraceTree;