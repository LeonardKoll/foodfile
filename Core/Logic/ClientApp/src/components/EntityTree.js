import React, {useRef, useEffect} from 'react';
import * as d3 from "d3";
import "./EntityTree.css";

function newHierarchyItem (name, member, location, time)
{
    var displayInfos = [];
    if (member!=null)   displayInfos.push("by " + member);
    if (location!=null) displayInfos.push("at " + location);
    if (time!=null)     displayInfos.push("on " + (new Date (1000 * time)).toLocaleString());

    return {
        name: name,
        info: displayInfos,
        children: []
    }    
}

function generateHierarchy (entities, members, rootID) {

    for (var i=0; i<entities.length; i++)
    {
        if (entities[i].ID === rootID)
        {
            var atoms = entities[i].Atoms;

            // Sort Atoms inside Entity to have the latest first.
            atoms.sort(function(atomA, atomB){return atomB.Version - atomA.Version});

            // Find EntityDescription to get the Name
            var entityName = rootID;
            for (var i=0; i<atoms.length; i++)
            {
                var data = atoms[i].Information;
                if (data.Case == "EntityDescription")
                {
                    entityName = data.Fields[0].Name;
                    break;
                }
            }

            // Create hierarchy Items
            var hierarchyChain = []
            
            // (1) Find all Transfer Atoms
            var completedAtomIDs = []
            atoms.forEach(atom => { 

                if (atom.Information.Case == "Transfer" && !completedAtomIDs.includes(atom.AtomID))
                {
                    completedAtomIDs.push(atom.ID);
                    var data = atom.Information.Fields[0];
                    
                    // Member
                    var member = null;
                    if (data.Responsible != null)
                    {
                        member = data.Responsible.Fields[0];
                        var found = members.find (c => c.ID == member)
                        if (typeof found !== 'undefined')
                        {
                            if (found.Name != null) member = found.Name.Fields[0];
                        }
                    } 
                    
                    // Location & Tome
                    var location = null;
                    var time = null;
                    var destination = data.TrackPoints.length>0 ? data.TrackPoints[data.TrackPoints.length-1] : null;
                    if (destination!=null)
                    {
                        time = destination.Item2;

                        if (destination.Item1.Name != null)
                            location = destination.Item1.Name.Fields[0];
                        else
                            location = destination.Coordinates
                    }
                    
                    // We do add a Transfer if we don't have a location / timestamp.
                    if (destination!=null)
                    {
                        var transfer = newHierarchyItem (entityName, member, location, time);
                        transfer.timestamp = time;
                        hierarchyChain.push (transfer);
                    }                    
                }

            });

            // (3) Sort by timestamp: Latest first.
            hierarchyChain.sort (function(a, b){return b.timestamp - a.timestamp});

            // (4) Find Creation Atom
            var inEntities = [];
            var member = null;
            var location = null;
            var time = null;

            for (var i=0; i<atoms.length; i++)
            {
                if (atoms[i].Information.Case == "Creation")
                {
                    var data = atoms[i].Information.Fields[0];

                    inEntities = data.InEntities;
                    time = data.Timestamp;
                    if (data.Responsible != null)
                    {
                        member = data.Responsible.Fields[0];
                        var found = members.find (c => c.ID == member)
                        if (typeof found !== 'undefined')
                        {
                            if (found.Name != null) member = found.Name.Fields[0];
                        }
                    }
                    else member = null;
                    if (data.Location != null)
                    {
                        var locData = data.Location.Fields[0];
                        location = locData.Name != null ? locData.Name.Fields[0] : locData.Coordinates;
                    }
                    else location = null;
                    
                    break;
                }
            }

            // (5) Add findings (even if there arent any) and recursive call
            var creation = newHierarchyItem (entityName, member, location, time);
            creation.children = inEntities.map ( entityID => generateHierarchy(entities, members, entityID) );
            hierarchyChain.push (creation);

            // (6) Generate Return
            for (var i=0; i<hierarchyChain.length-1; i++)
            {
                hierarchyChain[i].children = [hierarchyChain[i+1]];
            }
            return hierarchyChain[0];
            // We will always have at least one entry in hierarchyChain:
            // Creation is added even if there's no actual atom.
        }
    }

    return newHierarchyItem (rootID, null, null, null);
}

function appendTreeSVG (entities, members, rootID, containerRef)
{   
    var treeData = generateHierarchy(entities, members, rootID);

    // set the dimensions and margins of the diagram
    var margin = {top: 40, right: 120, bottom: 80, left: 40};
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
    var link = g.selectAll(".link")
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
        .style("font-weight", "bold")
        .text(function(d) { return d.data.name; });
    node.append("text")
        .attr("y", 37).attr("x", 10)
        .text(function(d) { return d.data.info.length > 0 ? d.data.info[0] : "" ; });
    node.append("text")
        .attr("y", 54).attr("x", 10)
        .text(function(d) { return d.data.info.length > 1 ? d.data.info[1] : "" ; });
    node.append("text")
        .attr("y", 71).attr("x", 10)
        .text(function(d) { return d.data.info.length > 2 ? d.data.info[2] : "" ; });
}

function EntityTree({entities, members, rootID})
{
    const containerRef = useRef(null);;

    useEffect(() => {

        if (entities.length>0)
            appendTreeSVG(entities, members, rootID, containerRef.current);  
        
        }, [entities.length,members.length]);
        /*
            If you pass an object, React will store only the reference to the object and run the effect when the reference changes,
            which is usually every singe time (I don't now how though).
            => Mention primitive values here.

            https://stackoverflow.com/questions/53070970/infinite-loop-in-useeffect
        */

    return (<div ref={containerRef}></div>);
}

export default EntityTree;