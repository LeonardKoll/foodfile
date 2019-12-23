namespace FoodFile

open Railway
open System
open Elastic

module Testdata = 

    let erdbeeren:Entity = {
        Atoms = [

            // Entity Description
            {ShortID = "GGZA1NF4B2"; EntityID="ZW9FC617UW"; Version=1; Owners = [|{ID = "FreshFruitFarmers"}|]; 
            Data = EntityDescription ({Name="strawberry pallet" ; Certificates=None});
            Meta = None}
            
            // Harvesting
            {ShortID = "2OMJWCL972"; EntityID="ZW9FC617UW"; Version=1; Owners = [|{ID = "FreshFruitFarmers"}|];
            Data = Transformation ({InEntities = [||]; Location = Some({Name="Farm 3"; Coordinates="50.553291, 11.019047"})});
            Meta = Some({Timestamp = 1562167380L})}

            // Collecting
            {ShortID = "NV390EAM9P"; EntityID="ZW9FC617UW"; Version=1; Owners = [|{ID = "FreshFruitFarmers"}|];
            Data = Tracking ({Location = {Name="FreshFruitFarmers warehouse"; Coordinates="50.281290, 10.967620"}});
            Meta = None}

            // Shipping
            {ShortID = "0SV6M9SN3I"; EntityID="ZW9FC617UW"; Version=1; Owners = [|{ID = "FreshFruitFarmers"};{ID = "SugarSilo SARL"}|];
            Data = Tracking ({Location = {Name="YummyJam production facilty"; Coordinates="51.590067, 8.1050100"}});
            Meta = Some({Timestamp = 1562227920L})}
    ]}

    let zuckerrueben = {
        Atoms = [

            // Entity Description
            {ShortID = "XY391R4AVL"; EntityID="HC0V3H5Y1R"; Version=1; Owners = [|{ID = "SugarSilo SARL"}|]; 
            Data = EntityDescription ({Name="sugar beet pallet" ; Certificates=None});
            Meta = None}

            // Harvesting
            {ShortID = "5MEDIMBBCA"; EntityID="HC0V3H5Y1R"; Version=1; Owners = [|{ID = "SugarSilo SARL"}|];
            Data = Transformation ({InEntities = [||]; Location = Some({Name="Field A4"; Coordinates="52.029034, 17.553938"})});
            Meta = Some({Timestamp = 1553698467L})}

        ]}

    let geliermittel = {
        Atoms = [
        
            // Entity Description
            {ShortID = "7SMZ65O848"; EntityID="6TKH3NAIWX"; Version=1; Owners = [|{ID = "SugarSilo SARL"}|]; 
            Data = EntityDescription ({Name="gellant" ; Certificates=None});
            Meta = None}

            // Bought but no further information
            {ShortID = "R6A2TDZN3W"; EntityID="6TKH3NAIWX"; Version=1; Owners = [|{ID = "SugarSilo SARL"}|]; 
            Data = Registration ({Seller="ChemicalKings Corp."});
            Meta = None}
        
        ]
    }

    let gelierzucker = {
        Atoms = [

            // Entity Description
            {ShortID = "J8EP46ZI6S"; EntityID="3ORECYN05A"; Version=1; Owners = [|{ID = "SugarSilo SARL"}|]; 
            Data = EntityDescription ({Name="preserving sugar pallet" ; Certificates=None});
            Meta = None}

            // Production
            {ShortID = "GNA6NJ3B1B"; EntityID="3ORECYN05A"; Version=1; Owners = [|{ID = "SugarSilo SARL"}|];
            Data = Transformation ({InEntities = [|"6TKH3NAIWX"; "HC0V3H5Y1R"|]; Location = None});
            Meta = Some({Timestamp = 1555303285L})}

            // Shipping
            {ShortID = "J0MQJB2OPR"; EntityID="3ORECYN05A"; Version=1; Owners = [|{ID = "SugarSilo SARL"};{ID = "YummyJam"}|];
            Data = Tracking ({Location = {Name="YummyJam production facilty"; Coordinates="52.513072, 13.269270"}});
            Meta = Some({Timestamp = 1555598311L})}
        ]
    }

    let jam = {
        Atoms = [
            
            // Entity Description
            {ShortID = "JI4FY38XR1"; EntityID="L8ETBLF1AN"; Version=1; Owners = [|{ID = "YummyJam"}|]; 
            Data = EntityDescription ({Name="strawberry jam jar" ; Certificates=None});
            Meta = None}

            // Production
            {ShortID = "ZYQZ46WHOM"; EntityID="L8ETBLF1AN"; Version=1; Owners = [|{ID = "YummyJam"}|];
            Data = Transformation ({InEntities = [|"3ORECYN05A"; "ZW9FC617UW"|]; Location = Some({Name="YummyJam production facilty"; Coordinates="52.513072, 13.269270"})});
            Meta = Some({Timestamp = 1568979091L})}
            
        ]
    }

    // Just Testing
    let ExecuteRead = fun () ->
        startWith "L8ETBLF1AN"
        |> switch ReadDocument
        |> Convert.ToString
        |> printf "Result: %s"


    let WriteOne = fun entity ->
        startWith entity
        |> switch WriteDocument
        |> Convert.ToString
        |> printf "Result: %s"

    // Just Testing
    let ExecuteWrite = fun () ->
        WriteOne erdbeeren
        WriteOne zuckerrueben
        WriteOne geliermittel
        WriteOne gelierzucker
        WriteOne jam