import React, {useState, useEffect} from 'react';
import TraceSearch from './TraceSearch';
import TraceTree from "./TraceTree"
import TraceAtoms from "./TraceAtoms"
import axios from 'axios';
import {generateHierarchy, emptyHierarchyItem} from './TraceTreeUtils'

function extractEntityID (searchterm)
{
    var splitted = searchterm.split("-");
    if (splitted.length < 3 )
        return splitted[splitted.length-1];
    return splitted[1]
}

export function Trace()
{
    const [direction, setDirection] = useState("downchain");
    const [searchterm, setSearchterm] = useState("")
    const [requeststate, setRequeststate] = useState("")
    const [searchresult, setSearchresult] = useState({
        Entities:[],
        Members:[]
      });
    const [displayentities, setDisplayentities] = useState([])
    const [treedata, setTreedata] = useState(undefined)

    useEffect(() => {
        var displayentities=[]
        if (searchterm.length === 10 || searchterm.length=== 17 || searchterm.length === 82)
        {
            setRequeststate("loading");
            axios.get('/api/entities/global/' + direction + "/" + searchterm)
            .then (response =>  {
                setRequeststate("result");
                setSearchresult (response.data);
            }).catch (err => {
                setRequeststate("error");
            });
        }
        else
        {
            setSearchresult({
                Entities: [],
                Members: []
            });
            setRequeststate("");
        }
    }, [searchterm, direction]);

    useEffect(() => {

        const rootID = extractEntityID(searchterm)
        const tD = generateHierarchy(
            direction,
            displayentities,
            searchresult.Members,
            rootID
        );
        
        if (tD.children.length === 0)
            setTreedata(undefined)
        else 
            setTreedata(tD)
            
    }, [displayentities.toString()]);

    return (       
        <div>
            <img className="img-fluid" src="/img/trace_cover.jpg" alt=""></img>
            <h1 className="mt-5 mb-5">Trace</h1>
            <p>
                FoodFile starts your retrival automatically as soon as you enter a valid trace code and opt for a search type.
                The trace code should have the form indicated below, with the first six digits being optional.
                Please note that collecting data from other members of the FoodFile network takes a few moments.
            </p>

            <TraceSearch setSearchterm={setSearchterm} direction={direction} setDirection={setDirection}/>

            { requeststate === "loading" &&
                <div className="alert alert-secondary" role="alert">
                    Processing your request...
                </div>
            }
            { requeststate === "result" &&
                <div className="alert alert-success" role="alert">
                    Retreival completed.
                </div>
            }
            { requeststate === "error" &&
                <div className="alert alert-warning" role="alert">
                    Retreival failed.
                </div>
            }

            <TraceTree treeData={treedata} />
            <TraceAtoms entities={searchresult.Entities} setDisplayentities={setDisplayentities} />
        </div>
    );
}