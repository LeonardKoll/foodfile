import React from 'react';


export function Footer()
{

    return (       
        <footer className="page-footer font-small bg-white text-dark border-top">
            <div className="container">
                <div className="row py-5">
                    <div className="col-sm">
                        <p>
                            <a href="/legal">References &amp; Legal</a>
                            <br></br>  
                            © 2020 by <a href="https://linkedin.com/in/leonardkoll">Leonard Koll</a>            
                        </p>
                    </div>
                    <div className="col-sm">
                        <p>You consent to the usage of cookies when using the membership services available at {window.location.origin.toString()}/membership.</p>
                    </div>
                </div>
            </div>
        </footer>
    );
}