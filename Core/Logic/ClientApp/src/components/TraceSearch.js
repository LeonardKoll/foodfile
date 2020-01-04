import React, {useState, useEffect} from 'react';

function TraceSearch ({setSearchterm})
{
    return (
        <div class="form-group">

            <div>
            <form class="form-inline">
            <label>Your trace code:</label>
            <input type="text" name="search" class="form-control border-0" placeholder="8X55N4-6FGEMO4WX8" onChange={e => setSearchterm(e.target.value)}/>
            </form>
            </div>

            <label>Please indicate whether you are interested in</label>

            <div class="form-check ml-4">
            <input class="form-check-input" type="radio" value="option2" checked="checked"/>
            <label class="form-check-label">the origin of this item</label>
            </div>

            <div class="form-check ml-4 disabled">
            <input class="form-check-input" type="radio" value="option3" disabled />
            <label class="form-check-label">the usage of this item</label>
            </div>

        </div>
    );
}

export default TraceSearch;