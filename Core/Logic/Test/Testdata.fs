namespace FoodFile

open System
open Elastic

module Testdata = 

    let freshfruitfarmers = {ID="IK7TEO"; Name=Some("FreshFruitFarmers"); API=Some("...")}
    let sugarsilo = {ID="UC2NRQ"; Name=Some("SugarSilo SARL"); API=Some("...")}
    let yummyjam = {ID="8X55N4"; Name=Some("YummyJam"); API=Some("...")}
    
    // ToDo: In Entities


    let erdbeeren:Entity = {
        Atoms = [

            // Entity Description
            {AtomID = "24CF"; EntityID="8EALKMS2Q7"; ProducerID=""; Version=1; AdditionalOwnerIDs = []; 
            Data = EntityDescription ({Name="strawberry pallet" ; Certificates=None});
            Meta = None}
            
            // Harvesting
            {AtomID = "E8RW"; EntityID="8EALKMS2Q7"; ProducerID=""; Version=1; AdditionalOwnerIDs = [];
            Data = Transformation ({InEntities = []; Location = Some({Name="Farm 3"; Coordinates="50.553291, 11.019047"})});
            Meta = Some({Timestamp = 1562167380L})}

            // Collecting
            {AtomID = "NZEU"; EntityID="8EALKMS2Q7"; ProducerID=""; Version=1; AdditionalOwnerIDs = [];
            Data = Tracking ({Location = {Name="FreshFruitFarmers warehouse"; Coordinates="50.281290, 10.967620"}});
            Meta = None}

            // Shipping
            {AtomID = "TTI5"; EntityID="8EALKMS2Q7"; ProducerID=""; Version=1; AdditionalOwnerIDs = [sugarsilo.ID];
            Data = Tracking ({Location = {Name="YummyJam production facilty"; Coordinates="51.590067, 8.1050100"}});
            Meta = Some({Timestamp = 1562227920L})}
    ]}

    let zuckerrueben = {
        Atoms = [

            // Entity Description
            {AtomID = "LTWA"; EntityID="UGSESG22LL"; ProducerID=sugarsilo.ID; Version=1; AdditionalOwnerIDs = []; 
            Data = EntityDescription ({Name="sugar beet pallet" ; Certificates=None});
            Meta = None}

            // Harvesting
            {AtomID = "6EFZ"; EntityID="UGSESG22LL"; ProducerID=sugarsilo.ID; Version=1; AdditionalOwnerIDs = []; 
            Data = Transformation ({InEntities = []; Location = Some({Name="Field A4"; Coordinates="52.029034, 17.553938"})});
            Meta = Some({Timestamp = 1553698467L})}

        ]}
 

    let geliermittel = {
        Atoms = [
        
            // Entity Description
            {AtomID = "I1O7"; EntityID="KVDBFFSAB4"; ProducerID=sugarsilo.ID; Version=1; AdditionalOwnerIDs = []; 
            Data = EntityDescription ({Name="gellant" ; Certificates=None});
            Meta = None}

            // Bought but no further information
            {AtomID = "4LPO"; EntityID="KVDBFFSAB4"; ProducerID=sugarsilo.ID; Version=1; AdditionalOwnerIDs = []; 
            Data = Registration ({Seller="ChemicalKings Corp."});
            Meta = None}
        
        ]
    }

    let gelierzucker = {
        Atoms = [

            // Entity Description
            {AtomID = "EUGA"; EntityID="EIDDTPPDB2"; ProducerID=sugarsilo.ID; Version=1; AdditionalOwnerIDs = []; 
            Data = EntityDescription ({Name="preserving sugar pallet" ; Certificates=None});
            Meta = None}

            // Production
            {AtomID = "BCWG"; EntityID="EIDDTPPDB2"; ProducerID=sugarsilo.ID; Version=1; AdditionalOwnerIDs = []; 
            Data = Transformation ({InEntities = [zuckerrueben.CompleteID.Value; geliermittel.CompleteID.Value]; Location = None});
            Meta = Some({Timestamp = 1555303285L})}

            // Shipping
            {AtomID = "NCDQ"; EntityID="EIDDTPPDB2"; ProducerID=sugarsilo.ID; Version=1; AdditionalOwnerIDs = [yummyjam.ID]; 
            Data = Tracking ({Location = {Name="YummyJam production facilty"; Coordinates="52.513072, 13.269270"}});
            Meta = Some({Timestamp = 1555598311L})}
        ]
    }

    let jam = {
        Atoms = [
            
            // Entity Description
            {AtomID = "R1AL"; EntityID="6FGEMO4WX8"; ProducerID=yummyjam.ID; Version=1; AdditionalOwnerIDs = []; 
            Data = EntityDescription ({Name="strawberry jam jar" ; Certificates=None});
            Meta = None}

            // Production
            {AtomID = "ZSOC"; EntityID="6FGEMO4WX8"; ProducerID=yummyjam.ID; Version=1; AdditionalOwnerIDs = []; 
            Data = Transformation ({InEntities = [erdbeeren.CompleteID.Value; gelierzucker.CompleteID.Value]; Location = Some({Name="YummyJam production facilty"; Coordinates="52.513072, 13.269270"})});
            Meta = Some({Timestamp = 1568979091L})}
            
        ];
        CompleteID=yummyjam.ID + "-" + "6FGEMO4WX8"
    }

    
    // Just Testing
    let ExecuteRead = fun () ->
        "6FGEMO4WX8"
        |> GetEntityLocal
        |> Convert.ToString
        |> printf "Result: %s"

    // Just Testing
    let ExecuteWrite = fun () ->
        WriteEntity erdbeeren               |> printf "Write Result: %s"
        WriteEntity zuckerrueben            |> printf "Write Result: %s"
        WriteEntity geliermittel            |> printf "Write Result: %s"
        WriteEntity gelierzucker            |> printf "Write Result: %s"
        WriteEntity jam                     |> printf "Write Result: %s"
        WriteParticipant freshfruitfarmers  |> printf "Write Result: %s"
        WriteParticipant sugarsilo          |> printf "Write Result: %s"
        WriteParticipant yummyjam           |> printf "Write Result: %s"

    