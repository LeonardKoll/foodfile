module ScenarioLibrary
open FoodFile
open AtomLibrary

type Scenario = {
    FreshFruitFarmers: (string -> string -> string -> Atom) List
    SugarSilo:  (string -> string -> string -> Atom) List
    YummyJam:  (string -> string -> string -> Atom) List
}

let basic = {
    FreshFruitFarmers = [st_1_cr_1; st_1_tr_1; st_1_tr_2; st_1_ds_1]
    SugarSilo =         [sb_1_cr_1; sb_1_ds_1]              // Sugar bett
                        @ [ps_1_cr_1; ps_1_tr_1; ps_1_ds_1] // Preserving Sugar
                        @ [ge_1_ds_1]                       // Gellant
    YummyJam =          [jj_1_cr_1; jj_1_ds_1] 
                        @ [jj_2_cr_1; jj_2_ds_1] 
                        @ [jj_3_cr_1; jj_3_ds_1] 
                        @ [jj_4_cr_1; jj_4_ds_1] 
                        @ [jj_5_cr_1; jj_5_ds_1]
}