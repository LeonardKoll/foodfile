import React, { useState, useEffect } from "react";
import axios from "axios";

function verify(verifyRequestData, setResult) {
  setResult("loading");
  axios
    .post("/api/entities/global/atom/verify", verifyRequestData)
    .then((response) => {
      const rendered = renderVerification(response.data)
      setResult(rendered);
    })
    .catch((err) => {
      setResult(err.toString());
    });
}

function renderVerification(result) {
    
    const stringified = result.map (c => `${c.atomHash.substr(c.atomHash.length - 7)} (${c.supportCount})`)
    return stringified.join(", ")

}

export default function VerifyAtom({verifyRequestData}) {
  const [result, setResult] = useState("");

  return (
    <div>
      <button
        type="button"
        className="btn btn-primary"
        onClick={() => verify(verifyRequestData, setResult)}
      >
        Verify
      </button>

      <span className="ml-3">{result.toString()}</span>
    </div>
  );
}
