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

  axios.get('/api/mode/')
  .then (response =>  {
    setMode(response.data);
  }).catch (err => {
    setMode("none");
  });

  return (
    <Layout mode={mode} >
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