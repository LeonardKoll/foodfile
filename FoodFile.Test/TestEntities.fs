namespace FoodFile

open TestMembers

module TestEntities =

    let erdbeeren:Entity = {
        ID = "8EALKMS2Q7";
        Atoms = [

            // Entity Description
            {AtomID = "24CF"; EntityID="8EALKMS2Q7"; Version=1; 
            Information = Description ({Name="Strawberry Pallet" ; Certificates=[]});
            Signatures=[];
            Sharing=Enabled}
            
            // Harvesting
            {AtomID = "E8RW"; EntityID="8EALKMS2Q7"; Version=1;
            Information = Creation ({   InEntities = []; 
                                        Location = Some({Name=Some("Farm 3"); Coordinates="50.553291, 11.019047"}); 
                                        Timestamp = 1562167380L; 
                                        Responsible=Some(freshfruitfarmers.ID)});
            Signatures=[];
            Sharing=Enabled}

            // Collecting
            {AtomID = "NZEU"; EntityID="8EALKMS2Q7"; Version=1;
            Information = Transfer ({  Responsible=Some(freshfruitfarmers.ID);
                                TrackPoints=
                                    [({Name=Some("FreshFruitFarmers Warehouse"); Coordinates="50.281290, 10.967620"}, 1562227920L)
                                ]});
            Signatures= [];
            Sharing=ByToken}

            // Shipping
            {AtomID = "TTI5"; EntityID="8EALKMS2Q7"; Version=1;
            Information = Transfer ({  Responsible=Some(freshfruitfarmers.ID);
                                TrackPoints=
                                    [({Name=Some("YummyJam Facilty"); Coordinates="51.590067, 8.1050100"}, 1562231520L)
                                ]});
            Signatures= [];
            Sharing=Enabled}
        ]
    }

    let zuckerrueben = {
        ID = "UGSESG22LL";
        Atoms = [

            // Entity Description
            {AtomID = "LTWA"; EntityID="UGSESG22LL"; Version=1; 
            Information = Description ({Name="Sugar Beet Pallet" ; Certificates=[]});
            Signatures= [];
            Sharing=Enabled}

            // Harvesting
            {AtomID = "6EFZ"; EntityID="UGSESG22LL"; Version=1; 
            Information = Creation ({   InEntities = []; 
                                        Location = Some({Name=Some("Field A4"); Coordinates="52.029034, 17.553938"}); 
                                        Timestamp = 1553698467L;
                                        Responsible=Some(sugarsilo.ID)});
            Signatures= [];
            Sharing=Enabled}

        ]
    }
 

    let geliermittel = {
        ID="KVDBFFSAB4";
        Atoms = [
        
            // Entity Description
            {AtomID = "I1O7"; EntityID="KVDBFFSAB4"; Version=1; 
            Information = Description ({Name="Gellant" ; Certificates=[]});
            Signatures= [];
            Sharing=Enabled}

            // Bought but no further information
        
        ]
    }

    let gelierzucker = {
        ID = "EIDDTPPDB2";
        Atoms = [

            // Entity Description
            {AtomID = "EUGA"; EntityID="EIDDTPPDB2"; Version=1; 
            Information = Description ({Name="Preserving Sugar Pallet" ; Certificates=[]});
            Signatures= [];
            Sharing=Enabled}

            // Production
            {AtomID = "BCWG"; EntityID="EIDDTPPDB2"; Version=1;
            Information = Creation ({   InEntities = [zuckerrueben.ID; geliermittel.ID]; 
                                        Location = None
                                        Responsible = Some(sugarsilo.ID);
                                        Timestamp = 1555303285L});
            Signatures= [];
            Sharing=Enabled}

            // Shipping
            {AtomID = "NCDQ"; EntityID="EIDDTPPDB2"; Version=1;
            Information = Transfer ({  Responsible=Some(sugarsilo.ID);
                                TrackPoints=
                                    [({Name=Some("YummyJam Facilty"); Coordinates="51.590067, 8.1050100"}, 1555598311L)
                                ]});
            Signatures= [];
            Sharing=Enabled}
        ]
    }

    let jam1 = {
        ID = "6FGEMO4WX8";
        Atoms = [
            
            // Entity Description
            {AtomID = "R1AL"; EntityID="6FGEMO4WX8"; Version=1; 
            Information = Description ({Name="Strawberry Jam Jar" ; Certificates=[]});
            Signatures= [];
            Sharing=Enabled}

            // Production
            {AtomID = "ZSOC"; EntityID="6FGEMO4WX8"; Version=1; 
            Information = Creation ({   InEntities = [erdbeeren.ID; gelierzucker.ID]; 
                                        Location = Some({Name=Some("YummyJam Facilty"); Coordinates="51.590067, 8.1050100"})
                                        Responsible=Some(yummyjam.ID)
                                        Timestamp = 1568979091L});
            Signatures= [];
            Sharing=Enabled}
            
        ]
    }

    let jam2 = {
        ID = "PMK9BG1YL5";
        Atoms = [
            
            // Entity Description
            {AtomID = "TAEF"; EntityID="PMK9BG1YL5"; Version=1; 
            Information = Description ({Name="Strawberry Jam Jar" ; Certificates=[]});
            Signatures= [];
            Sharing=Enabled}

            // Production
            {AtomID = "MFEA"; EntityID="PMK9BG1YL5"; Version=1; 
            Information = Creation ({   InEntities = [erdbeeren.ID; gelierzucker.ID]; 
                                        Location = Some({Name=Some("YummyJam Facilty"); Coordinates="51.590067, 8.1050100"})
                                        Responsible=Some(yummyjam.ID)
                                        Timestamp = 1568979091L});
            Signatures= [];
            Sharing=Enabled}
            
        ]
    }

    let jam3 = {
        ID = "JVE8F98148";
        Atoms = [
            
            // Entity Description
            {AtomID = "0G3T"; EntityID="JVE8F98148"; Version=1; 
            Information = Description ({Name="Strawberry Jam Jar" ; Certificates=[]});
            Signatures= [];
            Sharing=ByTokenOrChain}

            // Production
            {AtomID = "OO4Q"; EntityID="JVE8F98148"; Version=1; 
            Information = Creation ({   InEntities = [erdbeeren.ID; gelierzucker.ID]; 
                                        Location = Some({Name=Some("YummyJam Facilty"); Coordinates="51.590067, 8.1050100"})
                                        Responsible=Some(yummyjam.ID)
                                        Timestamp = 1568979091L});
            Signatures= [];
            Sharing=Enabled}
            
        ]
    }

    let jam4 = {
        ID = "LL9ZOFC5EH";
        Atoms = [
            
            // Entity Description
            {AtomID = "JN2U"; EntityID="LL9ZOFC5EH"; Version=1; 
            Information = Description ({Name="Strawberry Jam Jar" ; Certificates=[]});
            Signatures= [];
            Sharing=ByToken}

            // Production
            {AtomID = "XF78"; EntityID="LL9ZOFC5EH"; Version=1; 
            Information = Creation ({   InEntities = [erdbeeren.ID; gelierzucker.ID]; 
                                        Location = Some({Name=Some("YummyJam Facilty"); Coordinates="51.590067, 8.1050100"})
                                        Responsible=Some(yummyjam.ID)
                                        Timestamp = 1568979091L});
            Signatures= [];
            Sharing=Enabled}
            
        ]
    }

    let jam5 = {
        ID = "1I2VHCWKJL";
        Atoms = [
            
            // Entity Description
            {AtomID = "4BZD"; EntityID="1I2VHCWKJL"; Version=1; 
            Information = Description ({Name="Strawberry Jam Jar" ; Certificates=[]});
            Signatures= [];
            Sharing=Disabled}

            // Production
            {AtomID = "VS2Y"; EntityID="1I2VHCWKJL"; Version=1; 
            Information = Creation ({   InEntities = [erdbeeren.ID; gelierzucker.ID]; 
                                        Location = Some({Name=Some("YummyJam Facilty"); Coordinates="51.590067, 8.1050100"})
                                        Responsible=Some(yummyjam.ID)
                                        Timestamp = 1568979091L});
            Signatures= [];
            Sharing=Enabled}

            // Involvement
            {AtomID = "9FO2"; EntityID="1I2VHCWKJL"; Version=1; 
            Information = Involvement ({Member=freshfruitfarmers.ID});
            Signatures= [];
            Sharing=Enabled}

            // Involvement
            {AtomID = "9FO2"; EntityID="1I2VHCWKJL"; Version=1; 
            Information = Involvement ({Member=sugarsilo.ID});
            Signatures= [];
            Sharing=Enabled}
            
        ]
    }

    let all = [erdbeeren; zuckerrueben; geliermittel; gelierzucker; jam1; jam2; jam3; jam4; jam5]
    let yummyjamEntities = [jam1; jam2; jam3; jam4; jam5]
    let sugarsiloEntities = [geliermittel; gelierzucker]
    let freshfruitfarmerEntities = [erdbeeren; zuckerrueben]