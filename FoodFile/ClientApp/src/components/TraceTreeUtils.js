
// var treeData = generateHierarchy(direction, entities, members, rootID);

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

export function emptyHierarchyItem (rootID) {
    return newHierarchyItem ("Unknown", rootID, null, null, null, null);
}

function getMember (atom, members)
{
    var data = atom.Information.Fields[0];
    var member = null;
    if (data.Responsible != null)
    {
        member = data.Responsible.Fields[0];
        var found = members.find (c => c.ID === member)
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
    atoms.forEach(atom => { 

        if (atom.Information.Case === "Transfer")
        {
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
        if (atoms[i].Information.Case === "Creation")
        {
            return atoms[i]
        }
    }

    // Add findings (even if there arent any)
    return null; 
}

function getEntityName (atoms, rootID)
{
    for (var i=0; i<atoms.length; i++)
    {
        var data = atoms[i].Information;
        if (data.Case === "Description")
        {
            return data.Fields[0].Name;
        }
    }
    return rootID;
}

function getOutEntities (entityID, entities)
{
    var outEntities = []

    for (var i=0; i<entities.length; i++)
        if (entities[i].InEntities.includes(entityID))
            outEntities.push(entities[i].ID);
    
    return outEntities;
}

function getEntityAtoms (entities, entityID)
{
    var atoms = null
    for (var i=0; i<entities.length; i++)
    {
        if (entities[i].ID === entityID)
        {
            return entities[i].Atoms;

        }
    }
    
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

export function generateHierarchy (direction, entities, members, rootID) {

    var atoms = getEntityAtoms(entities, rootID);

    if(atoms != null)
    {
        var entityName = getEntityName (atoms, rootID)
        var hierarchyChain = getUnorderedTransfers (rootID, entityName, atoms, members)
        var creationAtom = getCreationAtom (atoms);
        var creationHierarchyItem = getCreationHierarchyItem (rootID, entityName, creationAtom, members)

        var followupEntities;
        if (direction==="upchain")
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

    return emptyHierarchyItem(rootID);
}