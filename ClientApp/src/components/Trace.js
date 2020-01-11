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
            axios.get('/api/entities/global/' + searchterm).then (response =>  {
            setSearchresult (response.data);
            });
        }
    }, [searchterm]);

    return (       
        <div>
            <img className="img-fluid" src="/img/trace_cover.jpg"></img>
            <h1 className="mt-5 mb-3">Trace</h1>
            <p>
                FoodFile starts your retrival automatically as soon as you enter a valid trace code and opt for a search type.
                The trace code should have the form indicated below, with the first six digits being optional.
                Please note that collecting data from other members of the FoodFile network takes a few moments.
            </p>
            <TraceSearch setSearchterm = {setSearchterm}/>
            <EntityTree entities={searchresult.Entities} members={searchresult.Members} rootID={searchterm} />
        </div>
    );
}

export default Trace;