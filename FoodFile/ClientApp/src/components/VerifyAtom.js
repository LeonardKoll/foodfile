import React, { useState, useEffect } from "react";
import axios from "axios";

export default function VerifyAtom({verifyRequestData}) {
  const [result, setResult] = useState("");

  function verify() {
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
  
  function renderVerification(resp) {
      
      const stringified = resp.map (c => `${c.atomHash.substr(c.atomHash.length - 7)} (${c.supportCount})`)
      return stringified.join(", ")
  
  }

  return (
    <div>
      <button
        type="button"
        className="btn btn-primary"
        onClick={() => verify()}
      >
        Verify
      </button>

      <span className="ml-3">{result.toString()}</span>
    </div>
  );
}
