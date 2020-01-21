import React, {useState} from 'react';
import axios from 'axios';

export function Membership()
{

    const [operation, setOperation] = useState("create");
    const [member, setMember] = useState("");
    const [password, setPassword] = useState("");
    const [name, setName] = useState("");
    const [url, setURL] = useState("");

    const ocMember = (event) => {setMember(event.target.value)};
    const ocPassword = (event) => {setPassword(event.target.value)};
    const ocName = (event) => {setName(event.target.value)};
    const ocURL = (event) => {setURL(event.target.value)};

    const [result, setResult] = useState({Case:""})

    const fireRequest = () => {
        
        setResult({Case:"loading"});
        switch (operation)
        {
            case "create":
                axios.post('/api/members/', {
                        ID: '',
                        Name: name,
                        API: url,
                        Password: password
                    }
                    ).then (response =>  {
                        setResult(response.data);
                    }).catch (err => {
                        setResult({Case:"Error", Fields:["An error occured."]})
                    });
                break;
            case "edit":
                axios.post('/api/members/', {
                        ID: member,
                        Name: name,
                        API: url,
                        Password: password
                    }
                    ).then (response =>  {
                        setResult(response.data);
                    }).catch (err => {
                        setResult({Case:"Error", Fields:["An error occured."]})
                    });
                break;
            case "delete":
                setResult("processing");
                axios.delete('/api/members/', {data:{
                        ID: member,
                        Password: password
                    }}
                    ).then (response =>  {
                        setResult(response.data);
                    }).catch (err => {
                        setResult({Case:"Error", Fields:["An error occured."]})
                    });
                break;
        }
    }

    return (       
        <div>
            <img className="img-fluid" src="/img/membership_cover.jpg"></img>
            <h1 className="mt-5 mb-3">Membership</h1>

            <div class="alert alert-danger" role="alert">
                This feature is not production ready. Your password will not be secure. Do not use any password you care about.
            </div>

            <div>
                <div class="btn-group btn-group-toggle mb-3" data-toggle="buttons">
                    <label class={(operation==="create" ? "active" : "") + " btn btn-secondary"}>
                        <input type="radio" name="options" id="option1" autocomplete="off" onClick={() => setOperation("create")} />Create
                    </label>
                    <label class={(operation==="edit" ? "active" : "") + " btn btn-secondary"}>
                        <input type="radio" name="options" id="option2" autocomplete="off" onClick={() => setOperation("edit")} />Edit
                    </label>
                    <label class={(operation==="delete" ? "active" : "") + " btn btn-secondary"}>
                        <input type="radio" name="options" id="option3" autocomplete="off" onClick={() => setOperation("delete")} />Delete
                    </label>
                </div>

                {(operation === "edit" || operation === "delete" ) &&
                    <div class="form-group">
                        <label for="memberID">Member-ID</label>
                        <input type="text" class="form-control" id="memberID" placeholder="UC2NRQ" onChange={ocMember}/>
                        <small class="form-text text-muted">Upon creation of a new member record, we generate this ID for you.</small>
                    </div>
                }
                {(operation === "edit" || operation === "create" ) &&   
                    <div class="form-group">
                        <label for="name">Company Name</label>
                        <input type="text" class="form-control" id="name" placeholder="FreshFruitFarmers LLC" onChange={ocName} />
                    </div>
                }
                {(operation === "edit" || operation === "create" ) &&
                    <div class="form-group">
                        <label for="apiurl">API-URL</label>
                        <input type="url" class="form-control" id="apiurl" placeholder="https://freshfruitfarmers.com/api/" onChange={ocURL} />
                    </div>
                }
                <div class="form-group">
                    <label for="password">Password</label>
                    <input type="password" class="form-control" id="password" placeholder="forbidden-fruit-fiasco!" onChange={ocPassword} />
                </div>

                <button type="submit" class="btn btn-primary mb-3" onClick={fireRequest}>Submit</button>

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

        </div>
    );
}