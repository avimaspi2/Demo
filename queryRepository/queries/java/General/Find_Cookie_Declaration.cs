CxList candidates = All.FindByType("Cookie");
CxList collections = candidates.FindByType(typeof(GenericTypeRef));
CxList collectionsVarDecls = collections.GetFathers().GetFathers().FindByType(typeof(VariableDeclStmt));
CxList httpCookies = candidates.FindByType(typeof(Declarator));
httpCookies.Add(Find_Declarators().GetByAncs(collectionsVarDecls));	
result = httpCookies;