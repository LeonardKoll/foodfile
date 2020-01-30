import React from 'react';


export function Footer({mode})
{

    return (       
        <footer className="page-footer font-small bg-white text-dark border-top mt-5">
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
                        {
                            ((mode=="member") || (mode=="combined")) &&
                            <p>You consent to the usage of cookies when using the membership services available at {window.location.origin.toString()}/membership.</p>
                        }
                    </div>
                </div>
            </div>
        </footer>
    );
}