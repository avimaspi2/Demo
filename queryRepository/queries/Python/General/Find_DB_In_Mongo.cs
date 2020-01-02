//This query finds the API methods to insert data into a Mongo DataBase

// Find MySQL DB (the target of the paths of allDataInfluencedByConn).
CxList allDataInfluencedByConn = All.DataInfluencedBy(Find_DB_Conn_Mongo());
allDataInfluencedByConn = All.FindAllReferences(allDataInfluencedByConn);

//list of all the suspicious words regard to DB_In
List < string > listExecute = new List<string> {"bulk_write", "insert_one", "insert_many", "insert", "save",
		"update","remove", "replace_one", "update_one", "update_many", "delete_one", 
		"delete_many", "find_one*", "drop","drop_collection", "create_collection", "find_one_and*", 
		"drop_index", "drop_indexes", "find", "find_and_modify", "distinct", "group", "map_reduce"};



//Find execute methods
CxList methods = Find_Methods();
CxList methodExecute = methods.FindByShortNames(listExecute);
result.Add(methodExecute.InfluencedBy(allDataInfluencedByConn));

//Find system_js 
CxList memberAccess = All.FindByShortName("system_js").GetMembersOfTarget().FindByType(typeof(MemberAccess));
result.Add(memberAccess);

// Find all query execution methods on MySql DB. 
CxList tempTargets = All.NewCxList();
//Get 5 nodes for the right of our position
for(int i = 0; i < 5; i++){
	if(tempTargets.Count == 0){
		tempTargets.Add(allDataInfluencedByConn.GetMembersOfTarget());	
	}
	else{
		tempTargets.Add(tempTargets.GetMembersOfTarget());	
	}
}

CxList strings = Find_Strings();
List < string > mongoDBOperators = new List<string> {"$where", "$regex"};
CxList pathToInMeth = tempTargets.FindByShortName(methodExecute).InfluencedBy(strings.FindByShortNames(mongoDBOperators));
result.Add(pathToInMeth.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly));

//////////////    Find collection.COllection syntax  //////////////////
result.Add(methods.FindByName("*.collection.Collection"));