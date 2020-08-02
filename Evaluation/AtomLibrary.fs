module AtomLibrary
open FoodFile

(*
    st - strawberry
    sb - sugar beet
    ge - gellant
    ps - preserving sugar
    jj - jam jar

    cr - creation
    dt - destruction
    tr - transfer
    dc - description
    iv - involvement
    de - deleted
*)

let st_cr fffId ssId yjId = {
    AtomID = "E8RW"; EntityID="8EALKMS2Q7"; Version=1;
    Information = Creation ({   InEntities = []; 
                                Location = Some({Name=Some("Farm 3"); Coordinates="50.553291, 11.019047"}); 
                                Timestamp = 1562167380L; 
                                Responsible=Some(fffId)});
    Signatures=[];
    Sharing=Enabled
    }

let jj_tr fffId ssId yjId = {
    AtomID = "NCDQ"; EntityID="EIDDTPPDB2"; Version=1;
    Information = Transfer ({  Responsible=Some(ssId);
                        TrackPoints=
                            [({Name=Some("YummyJam Facilty"); Coordinates="51.590067, 8.1050100"}, 1555598311L)
                        ]});
    Signatures= [];
    Sharing=Enabled}