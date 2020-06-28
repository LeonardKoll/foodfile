import React, {useEffect} from 'react';
import {getVersionLists, prepareEntities} from './TraceAtomsUtils'

function TraceAtoms ({entities, setDisplayentities})
{
    useEffect(() => {

        const selection = getVersionLists(entities);
        console.log(selection);
        var selected = [];
        for (const [atomID, versionList] of Object.entries(selection)) {
            selected.push(atomID+ "-" + versionList[0])
        }
        console.log(selected);
        const toDisplay = prepareEntities(entities, selected);
        console.log(toDisplay);
        setDisplayentities(toDisplay);

    }, [entities.toString()]);

    return (
        <div>
            <p>Hello! Here will be a table with radio buttons</p>
        </div>
    );
}

export default TraceAtoms;