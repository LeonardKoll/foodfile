import React, {useState, useEffect} from 'react';
import TraceSearch from './TraceSearch';
import EntityHistory from './EntityHistory';

function Trace()
{
    const [searchterm, setSearchterm] = useState("")



    return (        
        <div>
            <TraceSearch setSearchterm = {setSearchterm}/>
            <EntityHistory searchterm = {searchterm}/>
        </div>
    );
}

export default Trace;