import React, { useState } from 'react';
import axios from 'axios';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { Membership } from './components/Membership';
import { Developers } from './components/Developers';
import { Capture } from './components/Capture';
import { Trace } from './components/Trace';
import { Legal } from './components/Legal'

import './custom.css'


function App() {

  const [mode, setMode] = useState("none")
  const [memberID, setMemberID] = useState("")
  const [memberName, setMemberName] = useState("")

  axios.get('/api/mode/')
  .then (response =>  {
    setMode(response.data.Mode);
    setMemberID(response.data.MemberID);
    setMemberName(response.data.MemberName);
  }).catch (err => {
    setMode("none");
    setMemberID("");
    setMemberName("");
  });

  return (
    <Layout mode={mode} memberID={memberID} memberName={memberName} >
        <Route exact path='/' component={Home} />
        <Route path='/developers' component={Developers} />
        {
          ((mode=="member") || (mode=="combined")) &&
          <Route path='/membership' component={Membership} />
        }
        {
          ((mode=="regular") || (mode=="combined"))  &&
          <Route path='/trace' component={Trace} />
        }
        {
          ((mode=="regular") || (mode=="combined")) &&
          <Route path='/capture' component={Capture} />
        }
        <Route path='/legal' component={Legal} />
      </Layout>
  )
}

export default App;