import React from 'react';

function TraceSearch ({setSearchterm, direction, setDirection})
{
    return (
        <div className="form-group">

            <div>
            <form className="form-inline">
            <label>Your trace code:</label>
            <input type="text" name="search" className="form-control border-0 shadow-none" placeholder="8X55N4-6FGEMO4WX8" onChange={e => setSearchterm(e.target.value)}/>
            </form>
            </div>

            <div className="btn-group btn-group-toggle mb-3" data-toggle="buttons">
                <label className={(direction==="downchain" ? "active" : "") + " btn btn-secondary"}>
                    <input type="radio" name="options" id="option1" autoComplete="off" onClick={() => setDirection("downchain")} />Trace to origin
                </label>
                <label className={(direction==="upchain" ? "active" : "") + " btn btn-secondary"}>
                    <input type="radio" name="options" id="option2" autoComplete="off" onClick={() => setDirection("upchain")} />Trace to shelf
                </label>
            </div>
        </div>
    );
}

export default TraceSearch;