import React, { useEffect, useState } from "react";
import { getVersionLists, prepareEntities } from "./TraceAtomsUtils";
import VerifyAtom from "./VerifyAtom";

function TraceAtoms({ entities, setDisplayentities, members }) {
  const [selected, setSelected] = useState([]);

  useEffect(() => {
    const selection = getVersionLists(entities);
    var selectedLocal = [];
    for (const [atomID, versionList] of Object.entries(selection)) {
      selectedLocal.push(atomID + "-" + versionList[0]);
    }
    setSelected(selectedLocal);
  }, [entities.toString()]);

  useEffect(() => {
    const toDisplay = prepareEntities(entities, selected);
    setDisplayentities(toDisplay);
  }, [selected.toString()])

  function handleSelection(event) {
    const idParts = event.target.value.split("-")
    const eaID = idParts[0] + "-" + idParts[1]

    const newSelected = selected.map(current => {
      if (current.startsWith(eaID)) 
        return event.target.value
      else
        return current;
    })
    setSelected(newSelected);
  }

  function createTableRow(eaID, atomVersions, members) {
    const radios = atomVersions.map((ver) => (
      <label
        className={
          (selected.includes(`${eaID}-${ver}`) ? "active" : "") +
          " btn btn-secondary"
        }
        key={`${eaID}-${ver}`}
      >
        {ver}
        <input
          onChange={(event) => handleSelection(event)}
          checked={selected.includes(`${eaID}-${ver}`)}
          type="radio"
          value={`${eaID}-${ver}`}
          autoComplete="off"
        />
      </label>
    ));

    const CompleteID = selected.find (c => c.startsWith(eaID))
    const Members = members.map (c => c.ID)
    const verifyRequestData = {
      CompleteID,
      Members
    }

    return (
      <tr key={eaID}>
        <td>{eaID}</td>
        <td>
          <div className="btn-group btn-group-toggle mb-3" data-toggle="buttons">
            {radios}
          </div>
        </td>
        <td>
          <VerifyAtom verifyRequestData={verifyRequestData}/>
        </td>
      </tr>
    );
  }

  function createTableRows(entities, members) {
    const versionLists = getVersionLists(entities);

    let toReturn = [];
    for (const [eaID, versionList] of Object.entries(versionLists)) {
      toReturn.push(createTableRow(eaID, versionList, members));
    }

    return toReturn;
  }

  return (
    <div>
      {entities.length > 0 && (
        <table className="mt-4 table">
          <thead>
            <tr>
              <th>Entity - Atom</th>
              <th>Version</th>
              <th>Verify</th>
            </tr>
          </thead>
          <tbody>{createTableRows(entities, members)}</tbody>
        </table>
      )}
    </div>
  );
}

export default TraceAtoms;
