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
    ds - description
    iv - involvement
    de - deleted
*)

// IDs
let st_1 = "8EALKMS2Q7"
let sb_1 = "UGSESG22LL"
let ps_1 = "EIDDTPPDB2"
let ge_1 = "KVDBFFSAB4"
let jj_1 = "6FGEMO4WX8"
let jj_2 = "PMK9BG1YL5"
let jj_3 = "JVE8F98148"
let jj_4 = "LL9ZOFC5EH"
let jj_5 = "1I2VHCWKJL"

// Creations

let st_1_cr_1 fffId ssId yjId = {
    AtomID = "E8RW"; EntityID=st_1; Version=1;
    Information = Creation ({   InEntities = []; 
                                Location = Some({Name=Some("Farm 3"); Coordinates="50.553291, 11.019047"}); 
                                Timestamp = 1562167380L; 
                                Responsible=Some(fffId)});
    Signatures=[];
    Sharing=Enabled
    }

let sb_1_cr_1 fffId ssId yjId = {
    AtomID = "6EFZ"; EntityID=sb_1; Version=1; 
    Information = Creation ({   InEntities = []; 
                                Location = Some({Name=Some("Field A4"); Coordinates="52.029034, 17.553938"}); 
                                Timestamp = 1553698467L;
                                Responsible=Some(ssId)});
    Signatures= [];
    Sharing=Enabled}

let sb_1_cr_2 fffId ssId yjId = {
    AtomID = "6EFZ"; EntityID=sb_1; Version=2; 
    Information = Creation ({   InEntities = []; 
                                Location = Some({Name=Some("Field X8"); Coordinates="52.029034, 17.553938"}); 
                                Timestamp = 1553698467L;
                                Responsible=Some(ssId)});
    Signatures= [];
    Sharing=Enabled}

let sb_1_cr_3 fffId ssId yjId = {
    sb_1_cr_1 fffId ssId yjId with Sharing=Disabled
}

let ps_1_cr_1 fffId ssId yjId = {
    AtomID = "BCWG"; EntityID=ps_1; Version=1;
    Information = Creation ({   InEntities = [sb_1; ge_1]; 
                                Location = None
                                Responsible = Some(ssId);
                                Timestamp = 1555303285L});
    Signatures= [];
    Sharing=Enabled}

let jj_1_cr_1 fffId ssId yjId = {
    AtomID = "ZSOC"; EntityID=jj_1; Version=1; 
    Information = Creation ({   InEntities = [st_1; ps_1]; 
                                Location = Some({Name=Some("YummyJam Facilty"); Coordinates="51.590067, 8.1050100"})
                                Responsible=Some(yjId)
                                Timestamp = 1568979091L});
    Signatures= [];
    Sharing=Enabled}

let jj_1_cr_2 fffId ssId yjId = {
    AtomID = "ZSOC"; EntityID=jj_1; Version=1; 
    Information = Creation ({   InEntities = []; 
                                Location = Some({Name=Some("YummyJam Facilty"); Coordinates="51.590067, 8.1050100"})
                                Responsible=Some(yjId)
                                Timestamp = 1568979091L});
    Signatures= [];
    Sharing=Enabled}

let jj_1_cr_3 fffId ssId yjId = {
    jj_1_cr_1 fffId ssId yjId with Version=2
}

let jj_2_cr_1 fffId ssId yjId = {
    AtomID = "MFEA"; EntityID=jj_2; Version=1; 
    Information = Creation ({   InEntities = [st_1; ps_1]; 
                                Location = Some({Name=Some("YummyJam Facilty"); Coordinates="51.590067, 8.1050100"})
                                Responsible=Some(yjId)
                                Timestamp = 1568979091L});
    Signatures= [];
    Sharing=Enabled}

let jj_3_cr_1 fffId ssId yjId = {
    AtomID = "OO4Q"; EntityID=jj_3; Version=1; 
    Information = Creation ({   InEntities = [st_1; ps_1]; 
                                Location = Some({Name=Some("YummyJam Facilty"); Coordinates="51.590067, 8.1050100"})
                                Responsible=Some(yjId)
                                Timestamp = 1568979091L});
    Signatures= [];
    Sharing=Enabled}

let jj_4_cr_1 fffId ssId yjId = {
    AtomID = "JN2U"; EntityID=jj_4; Version=1; 
    Information = Creation ({   InEntities = [st_1; ps_1]; 
                                Location = Some({Name=Some("YummyJam Facilty"); Coordinates="51.590067, 8.1050100"})
                                Responsible=Some(yjId)
                                Timestamp = 1568979091L});
    Signatures= [];
    Sharing=Enabled}

let jj_5_cr_1 fffId ssId yjId = {
    AtomID = "XF78"; EntityID=jj_5; Version=1; 
    Information = Creation ({   InEntities = [st_1; ps_1]; 
                                Location = Some({Name=Some("YummyJam Facilty"); Coordinates="51.590067, 8.1050100"})
                                Responsible=Some(yjId)
                                Timestamp = 1568979091L});
    Signatures= [];
    Sharing=Enabled}

let ge_1_cr_1 fffId ssId yjId = {
    AtomID = "IFG2"; EntityID=ge_1; Version=1; 
    Information = Creation ({   InEntities = []; 
                                Location = Some({Name=Some("The Chemical Company Facilty"); Coordinates="52.512435, 13.391256"})
                                Responsible=None
                                Timestamp = 1568945091L});
    Signatures= [];
    Sharing=Enabled}

let ge_1_cr_2 fffId ssId yjId = {
    AtomID = "IFG2"; EntityID=ge_1; Version=2; 
    Information = Creation ({   InEntities = []; 
                                Location = Some({Name=Some("The Chemical Company Facilty"); Coordinates="52.512435, 13.391256"})
                                Responsible=Some("M3MB3R")
                                Timestamp = 1568945091L});
    Signatures= [];
    Sharing=Enabled}

let ge_1_cr_3 fffId ssId yjId = {
    AtomID = "IFG2"; EntityID=ge_1; Version=1; 
    Information = Creation ({   InEntities = []; 
                                Location = Some({Name=Some("The Chemical Company Facilty"); Coordinates="52.512435, 13.391256"})
                                Responsible=None
                                Timestamp = 1568946091L});
    Signatures= [];
    Sharing=Enabled}

let ge_1_cr_4 fffId ssId yjId = {
    AtomID = "IFG2"; EntityID=ge_1; Version=2; 
    Information = Creation ({   InEntities = []; 
                                Location = Some({Name=Some("The Chemical Company Facilty"); Coordinates="52.512435, 13.391256"})
                                Responsible=Some("M3MB3R")
                                Timestamp = 1568946091L});
    Signatures= [];
    Sharing=Enabled}

let ge_1_cr_5 fffId ssId yjId = {
    ge_1_cr_4 fffId ssId yjId with Version=3
}

// Transfer

let st_1_tr_1 fffId ssId yjId = {
    AtomID = "NZEU"; EntityID=st_1; Version=1;
    Information = Transfer ({  Responsible=Some(fffId);
                        TrackPoints=
                            [({Name=Some("FreshFruitFarmers Warehouse"); Coordinates="50.281290, 10.967620"}, 1562227920L)
                        ]});
    Signatures= [];
    Sharing=Enabled}

let st_1_tr_2 fffId ssId yjId = {
    AtomID = "TTI5"; EntityID=st_1; Version=1;
    Information = Transfer ({  Responsible=Some(fffId);
                        TrackPoints=
                            [({Name=Some("YummyJam Facilty"); Coordinates="51.590067, 8.1050100"}, 1562231520L)
                        ]});
    Signatures= [];
    Sharing=Enabled}

let st_1_tr_3 fffId ssId yjId = {
    st_1_tr_1 fffId ssId yjId with Sharing=ByToken
}

let st_1_tr_4 fffId ssId yjId = {
    st_1_tr_2 fffId ssId yjId with Sharing=Disabled
}

let ps_1_tr_1 fffId ssId yjId = {
    AtomID = "NCDQ"; EntityID=ps_1; Version=1;
    Information = Transfer ({  Responsible=Some(ssId);
                        TrackPoints=
                            [({Name=Some("YummyJam Facilty"); Coordinates="51.590067, 8.1050100"}, 1555598311L)
                        ]});
    Signatures= [];
    Sharing=Enabled}

// Descriptions

let st_1_ds_1 fffId ssId yjId = {
    AtomID = "24CF"; EntityID=st_1; Version=1; 
    Information = Description ({Name="Strawberry Pallet" ; Certificates=[]});
    Signatures=[];
    Sharing=Enabled}

let st_1_ds_2 fffId ssId yjId = {
    st_1_ds_1 fffId ssId yjId  with Sharing=Disabled
}

let sb_1_ds_1 fffId ssId yjId = {
    AtomID = "LTWA"; EntityID="UGSESG22LL"; Version=1; 
    Information = Description ({Name="Sugar Beet Pallet" ; Certificates=[]});
    Signatures= [];
    Sharing=Enabled}

let ge_1_ds_1 fffId ssId yjId = {
    AtomID = "I1O7"; EntityID=ge_1; Version=1; 
    Information = Description ({Name="Gellant" ; Certificates=[]});
    Signatures= [];
    Sharing=Enabled}

let ps_1_ds_1 fffId ssId yjId = {
    AtomID = "EUGA"; EntityID=ps_1; Version=1; 
    Information = Description ({Name="Preserving Sugar Pallet" ; Certificates=[]});
    Signatures= [];
    Sharing=Enabled}

let jj_1_ds_1 fffId ssId yjId = {
    AtomID = "R1AL"; EntityID=jj_1; Version=1; 
    Information = Description ({Name="Strawberry Jam Jar" ; Certificates=[]});
    Signatures= [];
    Sharing=Enabled}

let jj_2_ds_1 fffId ssId yjId = {
    AtomID = "TAEF"; EntityID=jj_2; Version=1; 
    Information = Description ({Name="Strawberry Jam Jar" ; Certificates=[]});
    Signatures= [];
    Sharing=Enabled}

let jj_3_ds_1 fffId ssId yjId = {
    AtomID = "0G3T"; EntityID=jj_3; Version=1; 
    Information = Description ({Name="Strawberry Jam Jar" ; Certificates=[]});
    Signatures= [];
    Sharing=Enabled}

let jj_4_ds_1 fffId ssId yjId = {
    AtomID = "IJ89"; EntityID=jj_4; Version=1; 
    Information = Description ({Name="Strawberry Jam Jar" ; Certificates=[]});
    Signatures= [];
    Sharing=Enabled}

let jj_5_ds_1 fffId ssId yjId = {
    AtomID = "4BZD"; EntityID=jj_5; Version=1; 
    Information = Description ({Name="Strawberry Jam Jar" ; Certificates=[]});
    Signatures= [];
    Sharing=Enabled}

// Involvement

let ff_1_iv_1 fffId ssId yjId = {
    AtomID = "9FO2"; EntityID=st_1; Version=1; 
    Information = Involvement ({Member=yjId});
    Signatures= [];
    Sharing=Enabled}

let ss_1_iv_1 fffId ssId yjId = {
    AtomID = "834ID"; EntityID=ps_1; Version=1; 
    Information = Involvement ({Member=yjId});
    Signatures= [];
    Sharing=Enabled}

let jj_1_iv_1 fffId ssId yjId = {
    AtomID = "WID9"; EntityID=ps_1; Version=1; 
    Information = Involvement ({Member=ssId});
    Signatures= [];
    Sharing=Enabled}

let jj_2_iv_1 fffId ssId yjId = {
    AtomID = "M2CK"; EntityID=st_1; Version=1; 
    Information = Involvement ({Member=fffId});
    Signatures= [];
    Sharing=Enabled}

let jj_2_iv_2 fffId ssId yjId = {
    AtomID = "NOID"; EntityID=st_1; Version=1; 
    Information = Involvement ({Member="M3MB3R"});
    Signatures= [];
    Sharing=Enabled}