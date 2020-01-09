import React, {useState, useEffect} from 'react';

function TraceSearch ({setSearchterm})
{
    return (
        <div className="form-group">

            <div>
            <form className="form-inline">
            <label>Your trace code:</label>
            <input type="text" name="search" className="form-control border-0 shadow-none" placeholder="8X55N4-6FGEMO4WX8" onChange={e => setSearchterm(e.target.value)}/>
            </form>
            </div>

            <label className="mb-0">Please indicate whether you are interested in</label>

            <div className="form-check ml-4">
            <input className="form-check-input" type="radio" value="option2" checked="checked"/>
            <label className="form-check-label">the origin of this item</label>
            </div>

            <div className="form-check ml-4 disabled">
            <input className="form-check-input" type="radio" value="option3" disabled />
            <label className="form-check-label">the usage of this item</label>
            </div>

        </div>
    );
}

export default TraceSearch;