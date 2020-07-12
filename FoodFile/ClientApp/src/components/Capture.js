import React, {useState} from 'react';
import axios from 'axios';

export function Capture()
{
    const exampleAtom =[{ "AtomID": "ZSOC", "EntityID": "6FGEMO4WX8", "Version": 1,"Information": {"Case": "Creation", "Fields": [{"InEntities": [ "8EALKMS2Q7","EIDDTPPDB2"],"Responsible": { "Case": "Some","Fields": ["8X55N4"]}, "Location": {"Case": "Some","Fields": [{"Name": {"Case": "Some","Fields": ["YummyJam Facilty" ]},"Coordinates": "51.590067, 8.1050100"}] },"Timestamp": 1568979091 }] },"Signatures": [],"CompleteID": "6FGEMO4WX8-ZSOC-1"}];

    const [atoms, setAtoms] = useState("[]");
    const [members, setMembers] = useState("")
    const [fetchResult, setFetchResult] = useState("")
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

    const fireFetch = () => {
        setFetchResult({Case:"loading"});

        console.log(members)

        axios.post('/api/entities/global/fetch', members
        ).then (response =>  {
            setFetchResult("ok");
        }).catch (err => {
            console.log(err)
            setFetchResult("err")
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

            <hr/>

            <h1 className="mt-5 mb-5">Fetch</h1>
            <p>
                FoodFile can fetch the latest state from other FoodFile members.
                Enter the FoodFile member IDs you whish to fetch from below and click the fetch button to start the process.
            </p>
            <textarea   className="form-control" 
                        rows="1" placeholder="HOG6XJ, S5BHU9, 8XX99T"
                        onChange={(event) => setMembers(event.target.value.replace(/\s/g,'').split(","))}></textarea>
            <button type="submit" className="btn btn-primary mb-3 mt-3" onClick={fireFetch}>FETCH</button>

            { fetchResult === "loading" &&
                <div className="alert alert-secondary" role="alert">
                    Processing your request...
                </div>
            }
            { fetchResult === "ok" &&
                <div className="alert alert-success" role="alert">
                    Your fetch request was successful.
                </div>
            }
            { fetchResult === ("err") &&
                <div className="alert alert-warning" role="alert">
                    Something went wrong.
                </div>
            }

        </div>
    );
}

