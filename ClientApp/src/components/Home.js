import React, { useState } from 'react';
import { UncontrolledCollapse, Button, CardBody, Card } from 'reactstrap';

export function Home ()
{
  const [isOpen, setIsOpen] = useState(false);

  const toggle = () => setIsOpen(!isOpen);

  return (
    <div>
      <img className="img-fluid mb-5" src="/img/home_cover.jpg"></img>
      <h1><i>Taste</i></h1>
      <h2 className="ml-4 mb-3"><i>is a matter of <b>trust</b>.</i></h2>

      <div className="container">
        <div className="row">
          <div className="col-sm p-5">
            <div className="text-center m-5"><img className="img-fluid w-50 h-50" src="/img/home_about.svg"></img></div>
            <h3>About</h3>
            <p>
            FoodFile creates insights, trust and transparency
            by storing the history of individual food products
            to visualize travel and processing along the supply chain.
            It is designed to work from farm to fork and automatically 
            connects the dots between the information provided by the suppliers. 
            As a decentralized system, FoodFile stores data on-site 
            and gives full access control to the originator. It integrates easely
            in existing production and ERP-landscapes and comes with a low entry barrier
            for small businesses.
            </p>
          </div>
          <div className="col-sm p-5">
          <div className="text-center m-5"><img className="img-fluid w-50 h-50" src="/img/home_motivation.svg"></img></div>
            <h3>Motivation</h3>
            <p>
            Globaliztion and outsourcing have fundamentlly changed the pace
            and complexity of food production. Suppliers and consumers have
            limited opportunities to control source, freshness and
            certifications of resources or pre-processed ingredients. Food
            safety is subject to an intensification of legal regulation
            worldwide and sustainability palys an increasing role for consumers.
            FoodFile is tailored around the specific needs of a heterogenous global 
            mutli-step production and designed for scalaibility.
            </p>
          </div>
          <div className="col-sm p-5">
            <div className="text-center m-5"><img className="img-fluid w-50 h-50" src="/img/home_technology.svg"></img></div>
            <h3>Technology</h3>
            <p>
            FoodFile is open-source and developed with modern, reliability proven 
            technologies such as Docker, Elasticsearch and Net Core. It is easy
            to deploy and scale on-site, cloud-based or even hybrid.
            FoodFile is built on the belief that blockchain is not an appropriate basis 
            for recordkeeping related to physical goods. You are invited to
            read the whitepaper (following soon) for a fundamental reasoning or check
            out <a href="https://www.mckinsey.com/business-functions/mckinsey-digital/our-insights/blockchain-beyond-the-hype-what-is-the-strategic-business-value">this McKinsey article</a>.
            </p>
          </div>
        </div>
      </div>

    </div>
  );
}
