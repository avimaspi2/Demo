if(param.Length >= 1){
	CxList objCreation = Find_ObjectCreations();
	CxList sqlite3Verbose = param[0] as CxList;
	string member = "";
	
	if(param.Length >= 2)
	{ 
		member = param[1] as string;
	}
	
	foreach (CxList sqlite3Instance in sqlite3Verbose){
		CxList elems = objCreation.FindByFileName(sqlite3Instance.GetFirstGraph().LinePragma.FileName);
		string instanceName = sqlite3Instance.GetName();
		CxList db = All.NewCxList();
		if(!String.IsNullOrEmpty(member)){
			db = elems.FindByName(instanceName + "." + member);
		}
		else{
			db = elems.FindByShortName(instanceName);
		}
		result.Add(db);
		result.Add(db.GetAssignee());
	}
}