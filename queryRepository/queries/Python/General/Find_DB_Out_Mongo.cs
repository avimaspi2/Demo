//This query finds the API methods to get data from a Mongo DataBase

// Find MySQL DB (the target of the paths of allDataInfluencedByConn).
CxList allDataInfluencedByConn = All.DataInfluencedBy(Find_DB_Conn_Mongo()).GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
allDataInfluencedByConn = All.FindAllReferences(allDataInfluencedByConn);

//list of all the suspicious words regard to DB_out
List < string > listExecute = new List<string> {"collection_names", "findOne", "find", "get_collection", "aggregate",
		"find_one_and_update", "find_and_modify", "find_and_delete", "distinct", "map_reduce",
		"parallel_scan", "inline_map_reduce"};	

//Find execute methods
CxList methods = Find_Methods();
CxList methodExecute = methods.FindByShortNames(listExecute);

// Find all query execution methods on MySql DB (the target of the paths of allDbIn). 
CxList pathToInMeth = allDataInfluencedByConn.DataInfluencingOn(methodExecute);
result.Add(pathToInMeth.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly));

//------------    Find collection.COllection syntax  ------------------
result.Add(methods.FindByName("*.collection.Collection"));