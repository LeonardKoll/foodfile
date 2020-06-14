import React, {useState} from 'react';
import axios from 'axios';

export function Capture()
{
    const exampleAtom =[{ "AtomID": "ZSOC", "EntityID": "6FGEMO4WX8", "Version": 1,"Information": {"Case": "Creation", "Fields": [{"InEntities": [ "8EALKMS2Q7","EIDDTPPDB2"],"Responsible": { "Case": "Some","Fields": ["8X55N4"]}, "Location": {"Case": "Some","Fields": [{"Name": {"Case": "Some","Fields": ["YummyJam Facilty" ]},"Coordinates": "51.590067, 8.1050100"}] },"Timestamp": 1568979091 }] },"Signatures": [],"CompleteID": "6FGEMO4WX8-ZSOC-1"}];

    const [atoms, setAtoms] = useState("[]");
    const [result, setResult] = useState({Case:""})

    const fireRequest = () => {
        setResult({Case:"loading"});
        axios.post('/api/entities/local/', JSON.parse(atoms)
        ).then (response =>  {
            setResult(response.data);
        }).catch (err => {
            setResult({Case:"Error", Fields:["An error occured."]})
        });
    }

    return (       
        <div>
            <img className="img-fluid" src="/img/capture_cover.jpg" alt=""></img>
            <h1 className="mt-5 mb-5">Capture</h1>
            <p>
                Data-Capturing is designed to be directly intergrated with inventory management or production systems. 
                You may alternatively use this interface to add data manually.
            </p>
            <textarea   className="form-control" 
                        rows="10" placeholder={JSON.stringify(exampleAtom)} 
                        onChange={(event) => setAtoms(event.target.value)}></textarea>
            <small className="form-text text-muted">
                To capture data, POST the information atoms to /api/entities/local/.
                You may paste your JSON (entity representation) here to let this fronted handle the POST for you.
            </small>

            <button type="submit" className="btn btn-primary mb-3 mt-3" onClick={fireRequest}>POST</button>

            { result.Case === "loading" &&
                <div className="alert alert-secondary" role="alert">
                    Processing your request...
                </div>
            }
            { result.Case === "Result" &&
                <div className="alert alert-success" role="alert">
                    {result.Fields[0]}
                </div>
            }
            { result.Case === "Error" &&
                <div className="alert alert-warning" role="alert">
                    {result.Fields[0]}
                </div>
            }
        </div>
    );
}

