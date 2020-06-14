namespace FoodFile

module TestData =

    let freshfruitfarmers = Some("IK7TEO")
    let sugarsilo = Some("UC2NRQ")
    let yummyjam = Some("8X55N4")

    let erdbeeren:Entity = {
        ID = "8EALKMS2Q7";
        Atoms = [

            // Entity Description
            {AtomID = "24CF"; EntityID="8EALKMS2Q7"; Version=1; 
            Information = Description ({Name="Strawberry Pallet" ; Certificates=[]});
            Signatures=[]}
            
            // Harvesting
            {AtomID = "E8RW"; EntityID="8EALKMS2Q7"; Version=1;
            Information = Creation ({   InEntities = []; 
                                        Location = Some({Name=Some("Farm 3"); Coordinates="50.553291, 11.019047"}); 
                                        Timestamp = 1562167380L; 
                                        Responsible=freshfruitfarmers});
            Signatures=[]}

            // Collecting
            {AtomID = "NZEU"; EntityID="8EALKMS2Q7"; Version=1;
            Information = Transfer ({  Responsible=freshfruitfarmers;
                                TrackPoints=
                                    [({Name=Some("FreshFruitFarmers Warehouse"); Coordinates="50.281290, 10.967620"}, 1562227920L)
                                ]});
            Signatures= []}

            // Shipping
            {AtomID = "TTI5"; EntityID="8EALKMS2Q7"; Version=1;
            Information = Transfer ({  Responsible=freshfruitfarmers;
                                TrackPoints=
                                    [({Name=Some("YummyJam Facilty"); Coordinates="51.590067, 8.1050100"}, 1562231520L)
                                ]});
            Signatures= []}
        ]
    }

    let zuckerrueben = {
        ID = "UGSESG22LL";
        Atoms = [

            // Entity Description
            {AtomID = "LTWA"; EntityID="UGSESG22LL"; Version=1; 
            Information = Description ({Name="Sugar Beet Pallet" ; Certificates=[]});
            Signatures= []}

            // Harvesting
            {AtomID = "6EFZ"; EntityID="UGSESG22LL"; Version=1; 
            Information = Creation ({   InEntities = []; 
                                        Location = Some({Name=Some("Field A4"); Coordinates="52.029034, 17.553938"}); 
                                        Timestamp = 1553698467L;
                                        Responsible=sugarsilo});
            Signatures= []}

        ]
    }
 

    let geliermittel = {
        ID="KVDBFFSAB4";
        Atoms = [
        
            // Entity Description
            {AtomID = "I1O7"; EntityID="KVDBFFSAB4"; Version=1; 
            Information = Description ({Name="Gellant" ; Certificates=[]});
            Signatures= []}

            // Bought but no further information
        
        ]
    }

    let gelierzucker = {
        ID = "EIDDTPPDB2";
        Atoms = [

            // Entity Description
            {AtomID = "EUGA"; EntityID="EIDDTPPDB2"; Version=1; 
            Information = Description ({Name="Preserving Sugar Pallet" ; Certificates=[]});
            Signatures= []}

            // Production
            {AtomID = "BCWG"; EntityID="EIDDTPPDB2"; Version=1;
            Information = Creation ({   InEntities = [zuckerrueben.ID; geliermittel.ID]; 
                                        Location = None
                                        Responsible = sugarsilo;
                                        Timestamp = 1555303285L});
            Signatures= []}

            // Shipping
            {AtomID = "NCDQ"; EntityID="EIDDTPPDB2"; Version=1;
            Information = Transfer ({  Responsible=sugarsilo;
                                TrackPoints=
                                    [({Name=Some("YummyJam Facilty"); Coordinates="51.590067, 8.1050100"}, 1555598311L)
                                ]});
            Signatures= []}
        ]
    }

    let jam1 = {
        ID = "6FGEMO4WX8";
        Atoms = [
            
            // Entity Description
            {AtomID = "R1AL"; EntityID="6FGEMO4WX8"; Version=1; 
            Information = Description ({Name="Strawberry Jam Jar" ; Certificates=[]});
            Signatures= []}

            // Production
            {AtomID = "ZSOC"; EntityID="6FGEMO4WX8"; Version=1; 
            Information = Creation ({   InEntities = [erdbeeren.ID; gelierzucker.ID]; 
                                        Location = Some({Name=Some("YummyJam Facilty"); Coordinates="51.590067, 8.1050100"})
                                        Responsible=yummyjam
                                        Timestamp = 1568979091L});
            Signatures= []}
            
        ]
    }

    let jam2 = {
        ID = "PMK9BG1YL5";
        Atoms = [
            
            // Entity Description
            {AtomID = "TAEF"; EntityID="PMK9BG1YL5"; Version=1; 
            Information = Description ({Name="Strawberry Jam Jar" ; Certificates=[]});
            Signatures= []}

            // Production
            {AtomID = "MFEA"; EntityID="PMK9BG1YL5"; Version=1; 
            Information = Creation ({   InEntities = [erdbeeren.ID; gelierzucker.ID]; 
                                        Location = Some({Name=Some("YummyJam Facilty"); Coordinates="51.590067, 8.1050100"})
                                        Responsible=yummyjam
                                        Timestamp = 1568979091L});
            Signatures= []}
            
        ]
    }

    let jam3 = {
        ID = "JVE8F98148";
        Atoms = [
            
            // Entity Description
            {AtomID = "0G3T"; EntityID="JVE8F98148"; Version=1; 
            Information = Description ({Name="Strawberry Jam Jar" ; Certificates=[]});
            Signatures= []}

            // Production
            {AtomID = "OO4Q"; EntityID="JVE8F98148"; Version=1; 
            Information = Creation ({   InEntities = [erdbeeren.ID; gelierzucker.ID]; 
                                        Location = Some({Name=Some("YummyJam Facilty"); Coordinates="51.590067, 8.1050100"})
                                        Responsible=yummyjam
                                        Timestamp = 1568979091L});
            Signatures= []}
            
        ]
    }

    let jam4 = {
        ID = "LL9ZOFC5EH";
        Atoms = [
            
            // Entity Description
            {AtomID = "JN2U"; EntityID="LL9ZOFC5EH"; Version=1; 
            Information = Description ({Name="Strawberry Jam Jar" ; Certificates=[]});
            Signatures= []}

            // Production
            {AtomID = "XF78"; EntityID="LL9ZOFC5EH"; Version=1; 
            Information = Creation ({   InEntities = [erdbeeren.ID; gelierzucker.ID]; 
                                        Location = Some({Name=Some("YummyJam Facilty"); Coordinates="51.590067, 8.1050100"})
                                        Responsible=yummyjam
                                        Timestamp = 1568979091L});
            Signatures= []}
            
        ]
    }

    let jam5 = {
        ID = "1I2VHCWKJL";
        Atoms = [
            
            // Entity Description
            {AtomID = "4BZD"; EntityID="1I2VHCWKJL"; Version=1; 
            Information = Description ({Name="Strawberry Jam Jar" ; Certificates=[]});
            Signatures= []}

            // Production
            {AtomID = "VS2Y"; EntityID="1I2VHCWKJL"; Version=1; 
            Information = Creation ({   InEntities = [erdbeeren.ID; gelierzucker.ID]; 
                                        Location = Some({Name=Some("YummyJam Facilty"); Coordinates="51.590067, 8.1050100"})
                                        Responsible=yummyjam
                                        Timestamp = 1568979091L});
            Signatures= []}
            
        ]
    }

    let all = [erdbeeren; zuckerrueben; geliermittel; gelierzucker; jam1; jam2; jam3; jam4; jam5]