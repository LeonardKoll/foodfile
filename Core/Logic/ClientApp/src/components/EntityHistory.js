import React, {useState, useEffect} from 'react';
import Tree from 'react-d3-tree';
const axios = require('axios');
 
const myTreeData = [
  {
    name: 'Top Level',
    attributes: {
      keyA: 'val A',
      keyB: 'val B',
      keyC: 'val C',
    },
    children: [
      {
        name: 'Level 2: A',
        attributes: {
          keyA: 'val A',
          keyB: 'val B',
          keyC: 'val C',
        },
      },
      {
        name: 'Level 2: B',
      },
    ],
  },
];

function EntityHistory({searchterm}) {

  const [state, setState] = useState(
    <p>Waiting.</p>
  );

  if (searchterm.length == 10)
  {
    axios.get('/api/trace/' + searchterm).then (response =>  {
      setState (JSON.stringify(response.data));
    });
  }

  return (
    <div>
      {state}
      <Tree data={myTreeData} />
    </div>
  )
}

export default EntityHistory;