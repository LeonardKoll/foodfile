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
            default: //"create"
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
        }
    }

    return (       
        <div>
            <img className="img-fluid" src="/img/membership_cover.jpg" alt=""></img>
            <h1 className="mt-5 mb-5">Membership</h1>

            

            <div className="row">
                <div className="col-sm">

                    <div className="text-center m-5"><img className="img-fluid w-25 h-25" src="/img/membership_edit.svg" alt=""></img></div>
                    <h3>Manage</h3>

                    <div class="btn-group btn-group-toggle mt-3 mb-3" data-toggle="buttons">
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
                    <small class="alert alert-danger form-text text-muted">This feature is not production ready. Your password will not be secure. Do not use any password you care about.</small>

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

                <div className="col-sm">

                    <div className="text-center m-5"><img className="img-fluid w-25 h-25" src="/img/membership_about.svg" alt=""></img></div>
                    <h3>About</h3>
                    <p>
                    In order to link the decentralized instances, FoodFile needs a common lookup table called membership service.
                    Request and manage your member-ID here in order to connect your FoodFile instance to the global network.
                    Your member-ID, company name and API-URL will be publicly available.
                    A membership service does not store your food records.
                    </p>
                    <p>
                    In upcoming versions, you will be able to sign data captured by you or other members.
                    The membership service will then serve as a trusted repository for your signature key.
                    </p>
                </div>

            </div>

        </div>
    );
}