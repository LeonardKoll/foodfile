module ScenarioLibrary
open FoodFile
open AtomLibrary

type Scenario = {
    FreshFruitFarmers: (string -> string -> string -> Atom) List
    SugarSilo: (string -> string -> string -> Atom) List
    YummyJam: (string -> string -> string -> Atom) List
    CapturePrepare: (string -> string -> string -> Atom) List
}

let basic = {
    FreshFruitFarmers = [st_1_cr_1; st_1_tr_2; st_1_ds_1]
    SugarSilo =         [sb_1_cr_1; sb_1_ds_1]              // Sugar bett
                        @ [ps_1_cr_1; ps_1_tr_1; ps_1_ds_1] // Preserving Sugar
                        @ [ge_1_ds_1]                       // Gellant
    YummyJam =          [jj_1_cr_1; jj_1_ds_1] 
                        @ [jj_2_cr_1; jj_2_ds_1] 
                        @ [jj_3_cr_1; jj_3_ds_1] 
                        @ [jj_4_cr_1; jj_4_ds_1] 
                        @ [jj_5_cr_1; jj_5_ds_1]
    CapturePrepare=     []
}

let basic_linked = {
    FreshFruitFarmers = basic.FreshFruitFarmers
                        @ [ff_1_iv_1]   // trawberries
    SugarSilo =         basic.SugarSilo
                        @ [ss_1_iv_1] // Preserving Sugar
    YummyJam =          basic.YummyJam
                        @ [jj_1_iv_1]   // Preserving Sugar
                        @ [jj_2_iv_1]  // Strawberries
    CapturePrepare =    []
}

let added_after = {
    basic_linked with CapturePrepare = [st_1_tr_1]
}

let additional_gellant_info = {
    FreshFruitFarmers = basic_linked.FreshFruitFarmers
    SugarSilo =         basic_linked.SugarSilo
                        @ [ge_1_cr_1] // Added Producer info
    YummyJam =          basic_linked.YummyJam
    CapturePrepare =    []
}

let desired_redundancy = {
    FreshFruitFarmers = [sb_1_cr_3; st_1_tr_4; st_1_ds_2; ff_1_iv_1]
    SugarSilo =         []
    YummyJam =          basic_linked.FreshFruitFarmers
                        @ basic_linked.SugarSilo
                        @ basic_linked.YummyJam
    CapturePrepare =    []
}

let no_transport = {
    FreshFruitFarmers = [st_1_cr_1; st_1_ds_1]
                        @ [ff_1_iv_1]
    SugarSilo =         [sb_1_cr_1; sb_1_ds_1]           
                        @ [ps_1_cr_1; ps_1_ds_1]
                        @ [ge_1_ds_1]
                        @ [ss_1_iv_1] 
    YummyJam =          [jj_1_cr_1; jj_1_ds_1] 
                        @ [jj_2_cr_1; jj_2_ds_1] 
                        @ [jj_3_cr_1; jj_3_ds_1] 
                        @ [jj_4_cr_1; jj_4_ds_1] 
                        @ [jj_5_cr_1; jj_5_ds_1]
                        @ [jj_1_iv_1]  
                        @ [jj_2_iv_1]  
    CapturePrepare=     []
}

let unknown_strawberry_producer = {
    FreshFruitFarmers = basic.FreshFruitFarmers
                        @ [ff_1_iv_1]   // Strawberries
    SugarSilo =         basic.SugarSilo
                        @ [ss_1_iv_1] // Preserving Sugar
    YummyJam =          basic.YummyJam
                        @ [jj_1_iv_1]   // Preserving Sugar
                        @ [jj_2_iv_2]  // Strawberries
    CapturePrepare =    []
}

let token_protected = {
    FreshFruitFarmers = basic_linked.FreshFruitFarmers
                        @ [st_1_tr_3]
    SugarSilo =         basic_linked.SugarSilo
    YummyJam =          basic_linked.YummyJam
    CapturePrepare =    []
}

let basic_contradiction = {
    FreshFruitFarmers = basic_linked.FreshFruitFarmers
                        @ [jj_1_cr_2]   // Wrong (contradicting) atom
    SugarSilo =         basic_linked.SugarSilo
                        @ [jj_1_cr_1] // Correct atom
    YummyJam =          basic_linked.YummyJam // Yj basic already has correct info
    CapturePrepare =    []
}

let version_contradiction = {
    FreshFruitFarmers = basic_contradiction.FreshFruitFarmers
                        @ [jj_1_cr_3]   // Additional Version of ZSOC wich is aligned
    SugarSilo =         basic_contradiction.SugarSilo
                        @ [jj_1_cr_3] 
    YummyJam =          basic_contradiction.YummyJam
                        @ [jj_1_cr_3] 
    CapturePrepare =    []
}


let retrospective_change_before = {
    FreshFruitFarmers = basic_linked.FreshFruitFarmers
                        @ basic_linked.SugarSilo
                        @ basic_linked.YummyJam
                        @ [ge_1_cr_1; ge_1_cr_2]
    SugarSilo =         basic_linked.FreshFruitFarmers
                        @ basic_linked.SugarSilo
                        @ basic_linked.YummyJam
                        @ [ge_1_cr_1; ge_1_cr_2]
    YummyJam =          basic_linked.FreshFruitFarmers
                        @ basic_linked.SugarSilo
                        @ basic_linked.YummyJam
                        @ [ge_1_cr_1; ge_1_cr_2]
    CapturePrepare =    []
}

let retrospective_change_after = {
    FreshFruitFarmers = basic_linked.FreshFruitFarmers
                        @ basic_linked.SugarSilo
                        @ basic_linked.YummyJam
                        @ [ge_1_cr_1; ge_1_cr_2]
    SugarSilo =         basic_linked.FreshFruitFarmers
                        @ basic_linked.SugarSilo
                        @ basic_linked.YummyJam
                        @ [ge_1_cr_3; ge_1_cr_4] // Modified time compared to retrospective_change_before
    YummyJam =          basic_linked.FreshFruitFarmers
                        @ basic_linked.SugarSilo
                        @ basic_linked.YummyJam
                        @ [ge_1_cr_1; ge_1_cr_2]
    CapturePrepare =    []
}

let retrospective_change_version = {
    FreshFruitFarmers = basic_linked.FreshFruitFarmers
                        @ basic_linked.SugarSilo
                        @ basic_linked.YummyJam
                        @ [ge_1_cr_1; ge_1_cr_2]
    SugarSilo =         basic_linked.FreshFruitFarmers
                        @ basic_linked.SugarSilo
                        @ basic_linked.YummyJam
                        @ [ge_1_cr_5] // Modified time now as new version
    YummyJam =          basic_linked.FreshFruitFarmers
                        @ basic_linked.SugarSilo
                        @ basic_linked.YummyJam
                        @ [ge_1_cr_1; ge_1_cr_2]
    CapturePrepare =    []
}