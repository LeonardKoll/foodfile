module ScenarioLibrary
open FoodFile
open AtomLibrary

type Scenario = {
    FreshFruitFarmers: (string -> string -> string -> Atom) List
    SugarSilo:  (string -> string -> string -> Atom) List
    YummyJam:  (string -> string -> string -> Atom) List
}

let scenario1 = {
    FreshFruitFarmers = [st_cr];
    SugarSilo = [];
    YummyJam = [jj_tr]
}

let hello = {
    FreshFruitFarmers = [];
    SugarSilo = [jj_tr];
    YummyJam = [st_cr]
}