//Find all return statements
CxList returnStmt = All.FindByFathers(All.FindByType(typeof(ReturnStmt)));
//The results are all the return sataments inside the Web Service methods 
result = returnStmt.GetByAncs(Find_WebServices_Methods());