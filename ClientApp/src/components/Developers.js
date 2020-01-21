import React from 'react';

export function Developers()
{
  return (
    <div>
      <img className="img-fluid" src="/img/developers_cover.jpg"></img>
      <h1 className="mt-5 mb-3">Developers</h1>

      <div className="container">
        <div className="row">
          <div className="col-sm">
            <div className="text-center m-5">
              <a href="https://github.com/leonardkoll/foodfile"><img className="img-fluid w-25 h-25" src="/img/developers_git.png"></img></a>
            </div>
            <h3>GitHub</h3>
            <p>
              FoodFile is open source and <a href="https://github.com/leonardkoll/foodfile">available on GitHub</a>.
            </p>
          </div>
          <div className="col-sm">
            <div className="text-center m-5">
              <a href="https://hub.docker.com/r/leonardkoll/foodfile"><img className="img-fluid w-25 h-25" src="/img/developers_docker.png"></img></a>
            </div>
            <h3>Docker</h3>
            <p>
            The Git-Master branch is availble as container on <a href="https://hub.docker.com/r/leonardkoll/foodfile">docker hub</a>.
            Please note that you must compose it with <a href="https://hub.docker.com/_/elasticsearch">Elasticsearch</a>.
            You may also <br></br>docker pull leonardkoll/foodfile.
            </p>
          </div>
        </div>
      </div>
    </div>
  );
}