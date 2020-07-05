

function removeDeleted  (entities) { // No Return, in Place

    for(var iA=0; iA<entities.length; iA++)
    {
        var iB=0
        while (iB<entities[iA].Atoms.length)
        {
            if (entities[iA].Atoms[iB].Information.Case === "Deleted")
            {
                entities[iA].Atoms = entities[iA].Atoms.filter( atom => atom.AtomID!==entities[iA].Atoms[iB].AtomID );
            }
            else iB++;
        }
    }

}

function sortAtoms (entities) { // No Return, in Place

    for(var iA=0; iA<entities.length; iA++)
    {
        // Sort Atoms inside Entity to have the latest first.
        entities[iA].Atoms.sort(function(atomA, atomB){return atomB.Version - atomA.Version});
    }

}

// Returns { entityID-AtomID: [Versions] }
export function getVersionLists (entities) {

    removeDeleted(entities);
    sortAtoms(entities);

    var toReturn = {};

    entities.forEach(entitiy => {
        entitiy.Atoms.forEach(atom => {
            
            const key = atom.EntityID+"-"+atom.AtomID
            if (toReturn.hasOwnProperty(key))
                toReturn[key].push(atom.Version)
            else
                toReturn[key] = [atom.Version]
        });
    });

    return toReturn;
}

// atomVersions = List of Atom-CompleteID (eg PMK9BG1YL5-TAEF-1)
// Takes the original Entities and a List of Complite Atom IDs and returns Entities but only containing the specified Atoms.
// This function is used to connect the user selection with what needs to be written into displayentities.
export function prepareEntities (entities, atomVersions) {

    var toReturn = [];

    entities.forEach(entitiy => {
        var atoms = []

        entitiy.Atoms.forEach(atom => {
            
            if (atomVersions.includes(atom.CompleteID))
                atoms.push(atom);
        });

        toReturn.push( {ID:entitiy.ID, Atoms:atoms} )
    });

    return toReturn;
}