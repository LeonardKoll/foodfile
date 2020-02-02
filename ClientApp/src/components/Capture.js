import React, {useState} from 'react';
import axios from 'axios';

export function Capture()
{
    const exampleAtom =[{ "AtomID": "ZSOC", "EntityID": "6FGEMO4WX8", "Version": 1,"Information": {"Case": "Creation", "Fields": [{"InEntities": [ "8EALKMS2Q7","EIDDTPPDB2"],"Responsible": { "Case": "Some","Fields": ["8X55N4"]}, "Location": {"Case": "Some","Fields": [{"Name": {"Case": "Some","Fields": ["YummyJam Facilty" ]},"Coordinates": "51.590067, 8.1050100"}] },"Timestamp": 1568979091 }] },"Signatures": [],"CompleteID": "6FGEMO4WX8-ZSOC-1"}];

    const [atoms, setAtoms] = useState("");
    const [result, setResult] = useState({Case:""})

    const fireRequest = () => {
        setResult({Case:"loading"});
        axios.post('/api/entities/local/', {data: atoms}
        ).then (response =>  {
            setResult(response.data);
        }).catch (err => {
            setResult({Case:"Error", Fields:["An error occured."]})
        });
    }

    return (       
        <div>
            <img className="img-fluid" src="/img/capture_cover.jpg"></img>
            <h1 className="mt-5 mb-5">Capture</h1>
            <p>
                Data-Capturing is designed to be directly intergrated with inventory management or production systems. 
                You may alternatively use this interface to add data manually.
            </p>
            <textarea   class="form-control" 
                        rows="10" placeholder={JSON.stringify(exampleAtom)} 
                        onCahnge={(event) => setAtoms(event.target.value)}></textarea>
            <small class="form-text text-muted">
                To capture data, POST the information atoms to /api/entities/local/. 
                The atoms need to be provided as a list. All atoms of one call must belong to the same entity.
                You may paste your JSON (list of atoms) here to let this fronted handle the POST for you.
            </small>

            <button type="submit" class="btn btn-primary mb-3 mt-3" onClick={fireRequest}>POST</button>

            { result.Case === "loading" &&
                <div class="alert alert-secondary" role="alert">
                    Processing your request...
                </div>
            }
            { result.Case === "Result" &&
                <div class="alert alert-success" role="alert">
                    {result.Fields[0]}
                </div>
            }
            { result.Case === "Error" &&
                <div class="alert alert-warning" role="alert">
                    {result.Fields[0]}
                </div>
            }
        </div>
    );
}

