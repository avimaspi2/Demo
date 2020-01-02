CxList dbMethods = All.NewCxList();
CxList allMethods = Find_Methods();
CxList allParams = All.FindByType(typeof (Param));

//handle dbIn
CxList dbIns = Find_DB_In();
CxList methodsOfTargets = dbIns.GetMembersOfTarget() * allMethods;         //all methods that are members of dbIns
dbMethods.Add(methodsOfTargets);

CxList dbInsAsParam = dbIns * allParams;                             //get all dbIns that are params
dbMethods.Add(allMethods.FindByParameters(dbInsAsParam));
 
//handle dbOut
CxList dbOuts = Find_DB_Out();
CxList dbOutMeth = dbOuts * allMethods;         //get dbOut methods
dbOuts -= dbOutMeth;                                   //only dbOut that are not mehtods
dbMethods.Add(dbOutMeth);                              //add dbOut methods
dbMethods.Add(allMethods.FindByParameters(dbOuts));    //add dbOut methods from Parameters
 
result = dbMethods;