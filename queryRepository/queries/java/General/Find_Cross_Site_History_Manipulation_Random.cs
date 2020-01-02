//Finds randoms for Cross_Site_History_Manipulation

CxList methods = Find_Methods();
CxList rand = All.FindByMemberAccess("Random.Next*", false); // next(), nextInt...
rand.Add(All.FindByMemberAccess("Math.random", false));
rand.Add(Get_ESAPI().FindByMemberAccess("Randomizer.*")); // ESAPI
rand.Add(All.GetParameters(methods.FindByMemberAccess("*Random.nextBytes", false), 0)); 

result = rand;