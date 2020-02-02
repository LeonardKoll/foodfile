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

  const [mode, setMode] = useState({Mode:"none", MemberID:"", MemberName:""})

  axios.get('/api/mode/')
  .then (response =>  {
    setMode(response.data);
  }).catch (err => {
    setMode({Mode:"none", MemberID:"", MemberName:""});
  });

  return (
    <Layout mode={mode} >
        <Route exact path='/' component={Home} />
        <Route path='/developers' component={Developers} />
        {
          ((mode.Mode=="member") || (mode.Mode=="combined")) &&
          <Route path='/membership' component={Membership} />
        }
        {
          ((mode.Mode=="regular") || (mode.Mode=="combined"))  &&
          <Route path='/trace' component={Trace} />
        }
        {
          ((mode.Mode=="regular") || (mode.Mode=="combined")) &&
          <Route path='/capture' component={Capture} />
        }
        <Route path='/legal' component={Legal} />
      </Layout>
  )
}

export default App;