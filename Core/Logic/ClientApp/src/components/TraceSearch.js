import React, {useState, useEffect} from 'react';

function TraceSearch ({setSearchterm})
{
    return (
        <form>
            <input type="text" name="search" onChange={e => setSearchterm(e.target.value)}/>
        </form>
    );
}

export default TraceSearch;