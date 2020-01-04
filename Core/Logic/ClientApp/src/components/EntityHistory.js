import React, {useState, useEffect} from 'react';
import EntityTree from "./EntityTree"
const axios = require('axios');
 

function EntityHistory({searchterm}) {

  const [state, setState] = useState({
    Entities:[],
    Members:[]
  });

  useEffect(() => {
    if (searchterm.length == 10)
    {
      axios.get('/api/global/' + searchterm).then (response =>  {
        setState (response.data);
      });
    }
  }, [searchterm]);

  return (
    <div>
      <EntityTree entities={state.Entities} members={state.Members} rootID={searchterm} />
    </div>
  )
}

export default EntityHistory;