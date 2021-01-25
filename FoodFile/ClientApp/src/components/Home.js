import React from 'react';

export function Home ()
{
  return (
    <div>
      <img className="img-fluid" src="/img/home_cover.jpg" alt=""></img>
      <div className="text-center mt-5">
        <h1 ><i>Taste</i></h1>
        <h2><i>is a matter of <b>trust</b>.</i></h2>
      </div>

      <div className="container">
        <div className="row">
          <div className="col-sm p-5">
            <div className="text-center m-5"><img className="img-fluid w-50 h-50" src="/img/home_about.svg" alt=""></img></div>
            <h3>About</h3>
            <p>
            FoodFile creates insights, trust and transparency
            by storing the history of individual food products
            to visualize travel and processing along the supply chain.
            It is designed to work from farm to fork and automatically 
            connects the dots between the information provided by suppliers. 
            As a decentralized system, FoodFile stores data on-site 
            and gives full access control to the originator. It integrates easily
            in existing production and ERP-landscapes and comes with a low entry barrier
            for small businesses.
            </p>
          </div>
          <div className="col-sm p-5">
          <div className="text-center m-5"><img className="img-fluid w-50 h-50" src="/img/home_motivation.svg" alt=""></img></div>
            <h3>Motivation</h3>
            <p>
            Globalization and outsourcing have fundamentally changed the pace
            and complexity of food production. Suppliers and consumers have
            limited opportunities to control source, freshness and
            certifications of resources or pre-processed ingredients. Food
            safety is subject to an intensification of legal regulation
            worldwide and sustainability plays an increasing role for consumers.
            FoodFile is tailored around the specific needs of a heterogenous global 
            mutli-step production and designed for scaleability.
            </p>
          </div>
          <div className="col-sm p-5">
            <div className="text-center m-5"><img className="img-fluid w-50 h-50" src="/img/home_technology.svg" alt=""></img></div>
            <h3>Technology</h3>
            <p>
            FoodFile is open-source and developed with modern, reliability proven 
            technologies such as Elasticsearch, Net Core and Docker. It is easy
            to deploy and scale on-site, cloud-based or even hybrid.
            FoodFile is built on the belief that blockchain is not an appropriate basis 
            for recordkeeping related to physical goods. You are invited to <a href="thesis_whitepaper.pdf">read the whitepaper</a> for a fundamental reasoning or check 
            out <a href="https://www.mckinsey.com/business-functions/mckinsey-digital/our-insights/blockchain-beyond-the-hype-what-is-the-strategic-business-value">this article by McKinsey</a>.
            </p>
          </div>
        </div>
      </div>

    </div>
  );
}
