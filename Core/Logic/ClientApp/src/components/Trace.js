import React, {useState, useEffect} from 'react';
import TraceSearch from './TraceSearch';
import EntityTree from "./EntityTree"
import axios from 'axios';

function Trace()
{
    const [searchterm, setSearchterm] = useState("")
    const [searchresult, setSearchresult] = useState({
        Entities:[],
        Members:[]
      });

    useEffect(() => {
        if (searchterm.length == 10)
        {
            axios.get('/api/global/' + searchterm).then (response =>  {
            setSearchresult (response.data);
            });
        }
    }, [searchterm]);

    return (        
        <div>
            <TraceSearch setSearchterm = {setSearchterm}/>
            <EntityTree entities={searchresult.Entities} members={searchresult.Members} rootID={searchterm} />
        </div>
    );
}

export default Trace;