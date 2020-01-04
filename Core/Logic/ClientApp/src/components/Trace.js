import React, {useState, useEffect} from 'react';
import TraceSearch from './TraceSearch';
import EntityTree from "./EntityTree"
import axios from 'axios';

export function Trace()
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
            <h1>Trace</h1>
            <TraceSearch setSearchterm = {setSearchterm}/>
            <EntityTree entities={searchresult.Entities} members={searchresult.Members} rootID={searchterm} />
            <p>
                FoodFile starts your retrival automatically as soon as you entered a valid trace code and opted for a search type.
                The trace code should have the form indicated above with the first six digits being optional.
                Please note that collecting data from other members of the FoodFile network takes a few moments.
            </p>
        </div>
    );
}