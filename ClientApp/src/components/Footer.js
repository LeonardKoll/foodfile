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
                            (((mode.Mode=="regular") || (mode.Mode=="combined")) && (mode.MemberID != "")) &&
                            <p className="float-right">Member {mode.MemberID}<br></br>{mode.MemberName}</p>
                        }
                    </div>
                </div>
            </div>
        </footer>
    );
}