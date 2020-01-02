CxList methods = Find_Methods();

CxList ibatis = methods.FindByMemberAccess("SqlMapper.QueryForObject", false);
ibatis.Add(methods.FindByMemberAccess("SqlMapper.QueryForList", false)); 
ibatis.Add(methods.FindByMemberAccess("SqlMapper.QueryWithRowHandler", false)); 
ibatis.Add(methods.FindByMemberAccess("SqlMapper.QueryForPaginatedList", false)); 
ibatis.Add(methods.FindByMemberAccess("SqlMapper.QueryForMap", false)); 
ibatis.Add(methods.FindByMemberAccess("SqlMapper.Insert", false)); 
ibatis.Add(methods.FindByMemberAccess("SqlMapper.Update", false)); 
ibatis.Add(methods.FindByMemberAccess("SqlMapper.Delete", false));

CxList qSQL1 = methods.FindByMemberAccess("QsqlQuery.BindValue", false);
CxList qSQL2 = methods.FindByMemberAccess("QsqlQuery.AddBindValue", false);

CxList repl = Find_Methods().FindByMemberAccess("String.Replace", false); 
repl.Add(Find_Methods().FindByMemberAccess("StringBuilder.Replace", false));

repl = All.GetByAncs(All.GetParameters(repl, 0)); 

CxList guid = All.FindByType(typeof(ObjectCreateExpr)).FindByShortName("Guid", false); 

result = Find_Replace();
result.Add(Find_Parameters()); 
result.Add(Find_Integers()); 
result.Add(All.FindByType("bool", false));
result.Add(All.GetParameters(ibatis, 1));
result.Add(All.GetParameters(qSQL1, 1));
result.Add(All.GetParameters(qSQL2, 0));
result.Add(repl);
result.Add(Find_DB_Linq());
result.Add(Find_DB_EF()); 
result.Add(guid);

result.Add(Find_Encode() - Find_HTML_Encode());