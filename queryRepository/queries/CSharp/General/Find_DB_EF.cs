// The purpose of the query is to found 
// All database access in Entity Framework (EF)

// The access to the database may be performed in different way

// The line below applys Where method
// Car car = db.Cars.Where(p => p.Model == "323").First();
//
// The line below applys Select method
// String Model = db.Cars.Select(c => c.Model).Where(c1 => c1 == "999").First();
//
// The line below allows get the first car in the car list
// Car car3 = db.Cars.ElementAt(0);
//
// The code below go over all cars in DB
// IEnumerable<Car> res = db.Cars;
// foreach (Car c in res)
// {
//      Console.WriteLine(c.Model);         
// }

// All access to DB - read and write will be detected by the query
// ---------------------------------------------------------------
// In EF model.designer object auto-generated and includes ObjectQuery property 
// as listed below

// public global::System.Data.Objects.ObjectQuery<Car> Cars {
//    get {            
//          this._Cars = base.CreateQuery<Car>("[Cars]");               
//          return this._Cars;
//      }
// }

result = Find_DB_EF_In() + Find_DB_EF_Out();