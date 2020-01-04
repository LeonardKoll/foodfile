import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { Membership } from './components/Membership';
import { Developers } from './components/Developers';
import { Capture } from './components/Capture';
import Trace from './components/Trace';

import './custom.css'


function App() {
  return (
    <Layout>
        <Route exact path='/' component={Home} />
        <Route path='/developers' component={Developers} />
        <Route path='/membership' component={Membership} />
        <Route path='/trace' component={Trace} />
        <Route path='/capture' component={Capture} />
      </Layout>
  )
}

export default App;