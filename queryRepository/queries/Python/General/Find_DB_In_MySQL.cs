//This query finds the API methods to insert data into a MySQL DataBase

// Find MySQL DB (the target of the paths of allDataInfluencedByConn).
CxList allDataInfluencedByConn = All.DataInfluencedBy(Find_DB_Conn_MySQL()).GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);

allDataInfluencedByConn = All.FindAllReferences(allDataInfluencedByConn);
CxList allExecuts = All.FindByShortNames(new List<string> {"execute", "executemany"});

// Get all the members which their targets are execution methods
CxList methodExecute = allExecuts.GetTargetOfMembers();

// Find all query execution methods on MySql DB (the target of the paths of allDbIn). 
allDataInfluencedByConn = allDataInfluencedByConn * methodExecute;
result = allDataInfluencedByConn.GetMembersOfTarget() * allExecuts;