import React, {useRef, useEffect} from 'react';
import * as d3 from "d3";
import "./EntityTree.css";

function newHierarchyItem (type, id, name, member, location, time)
{
    var displayInfos = [];
    if (type!=null)     displayInfos.push("[" + type + "] " + id);
    if (name!=null)     displayInfos.push(name);
    if (member!=null)   displayInfos.push("by " + member);
    if (location!=null) displayInfos.push("at " + location);
    if (time!=null)     displayInfos.push("on " + (new Date (1000 * time)).toLocaleString());

    return {
        info: displayInfos,
        children: []
    }    
}

function getPreparedAtoms (entities, rootID)
{
    var atoms = null
    for (var i=0; i<entities.length; i++)
        if (entities[i].ID === rootID)
        {
            atoms = entities[i].Atoms;
            break;
        }
    
    if (atoms==null)
        return null;

    // Sort Atoms inside Entity to have the latest first.
    atoms.sort(function(atomA, atomB){return atomB.Version - atomA.Version});

    // Remove Deleted
    var i=0
    while (i<atoms.length)
    {
        if (atoms[i].Information.Case == "Deleted")
        {
            atoms = atoms.filter( atom => atom.AtomID!=atoms[i].AtomID )
        }
        else i++;
    }

    return atoms;
}

function getEntityName (atoms, rootID)
{
    for (var i=0; i<atoms.length; i++)
    {
        var data = atoms[i].Information;
        if (data.Case == "Description")
        {
            return data.Fields[0].Name;
        }
    }
    return rootID;
}

function getMember (atom, members)
{
    var data = atom.Information.Fields[0];
    var member = null;
    if (data.Responsible != null)
    {
        member = data.Responsible.Fields[0];
        var found = members.find (c => c.ID == member)
        if (typeof found !== 'undefined')
        {
            member = found.Name;
        }
    } 
    return member
}

function getUnorderedTransfers (entityID, entityName, atoms, members)
{
    var transfers = []
        
    // Find all Transfer Atoms
    var completedAtomIDs = []
    atoms.forEach(atom => { 

        if (atom.Information.Case == "Transfer" && !completedAtomIDs.includes(atom.AtomID))
        {
            completedAtomIDs.push(atom.ID);
            var data = atom.Information.Fields[0];
            
            // Member
            var member = getMember(atom, members);
            
            // Location & Time
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
                var transfer = newHierarchyItem ("Transfer", entityID, entityName, member, location, time);
                transfer.timestamp = time;
                transfers.push (transfer);
            }                    
        }
    });

    return transfers;
}

function getCreationAtom (atoms)
{
    for (var i=0; i<atoms.length; i++)
    {
        if (atoms[i].Information.Case == "Creation")
        {
            return atoms[i]
        }
    }

    // Add findings (even if there arent any)
    return null; 
}

function getCreationHierarchyItem (entityID, entityName, creationAtom, members)
{
    if (creationAtom==null)
        return newHierarchyItem ("Creation", entityID, entityName, null, null, null);

    var member = null;
    var location = null;
    var time = null;
    var data = creationAtom.Information.Fields[0];

    time = data.Timestamp;
    member = getMember(creationAtom, members)
    if (data.Location != null)
    {
        var locData = data.Location.Fields[0];
        location = locData.Name != null ? locData.Name.Fields[0] : locData.Coordinates;
    }
    else location = null;
    
    return newHierarchyItem ("Creation", entityID, entityName, member, location, time);
}

function getOutEntities (entityID, entities)
{
    var outEntities = []

    for (var i=0; i<entities.length; i++)
        if (entities[i].InEntities.includes(entityID))
            outEntities.push(entities[i].ID);
    
    return outEntities;
}

function generateHierarchy (direction, entities, members, rootID) {

    var atoms = getPreparedAtoms(entities, rootID);
    if(atoms != null)
    {
        var entityName = getEntityName (atoms, rootID)
        var hierarchyChain = getUnorderedTransfers (rootID, entityName, atoms, members)
        var creationAtom = getCreationAtom (atoms);
        var creationHierarchyItem = getCreationHierarchyItem (rootID, entityName, creationAtom, members)

        var followupEntities;
        if (direction=="upchain")
        {
            // Sort by timestamp: Oldest first.
            hierarchyChain.sort (function(a, b){return a.timestamp - b.timestamp});
            hierarchyChain.unshift(creationHierarchyItem);
            followupEntities = getOutEntities (rootID, entities);
        }
        else
        {
            // Sort by timestamp: Latest first.
            hierarchyChain.sort (function(a, b){return b.timestamp - a.timestamp});
            hierarchyChain.push(creationHierarchyItem);
            followupEntities = (creationAtom==null) ? [] : creationAtom.Information.Fields[0].InEntities;
        }

        hierarchyChain[hierarchyChain.length-1].children = followupEntities.map ( entityID => generateHierarchy(direction, entities, members, entityID) );
        
        // Generate Return
        for (var i=0; i<hierarchyChain.length-1; i++)
        {
            hierarchyChain[i].children = [hierarchyChain[i+1]];
        }
        return hierarchyChain[0];
    }
    return newHierarchyItem ("Unknown", rootID, null, null, null, null);
}

function placeTreeSVG (direction, entities, members, rootID, containerRef)
{   
    var treeData = generateHierarchy(direction, entities, members, rootID);

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
    d3.select(containerRef).select("svg").remove();
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

function EntityTree({direction, entities, members, rootID})
{
    const containerRef = useRef(null);

    useEffect(() => {

        if (entities.length>0)
            placeTreeSVG(direction, entities, members, rootID, containerRef.current);  
        
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