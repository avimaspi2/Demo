if(param.Length > 0){
	CxList methods = Find_Methods();
	CxList sqliteLibRefs = param[0] as CxList;
	
	CxList sqliteLibRefsInParam = sqliteLibRefs.FindByFathers(Find_Param());
	result.Add(sqliteLibRefs - sqliteLibRefsInParam);
	CxList sqliteLibParams = sqliteLibRefsInParam.GetFathers();
	
	CxList tempResult = All.NewCxList();
	foreach(CxList elem in sqliteLibParams){
		int index = elem.GetIndexOfParameter();
		CxList content = All.FindDefinition(methods.FindByParameters(sqliteLibParams));
		tempResult.Add(All.GetParameters(content, index));
	}
	result.Add((All.FindAllReferences(tempResult)));
}