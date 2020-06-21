namespace FoodFile


module TestMembers = 
    
    let freshfruitfarmers:Member = {
        ID = "HOG6XJ";
        Name = "FreshFruitFarmers LLC";
        API = "http://localhost:5002/api/entities/local/";
        Password = "admin";
    }

    let sugarsilo:Member = {
        ID = "8XX99T";
        Name = "SugarSilo SARL";
        API = "http://localhost:5003/api/entities/local/";
        Password = "admin";
    }

    let yummyjam:Member = {
        ID = "S5BHU9";
        Name = "YummyJam";
        API = "https://localhost:5001/api/entities/local/";
        Password = "admin";
    }

    let all = [freshfruitfarmers; sugarsilo; yummyjam]