CxList methods = Find_Methods();
CxList theParams = Find_Param();

CxList sqlit3Meth = methods.FindByShortName("sqlite3_exec");
sqlit3Meth.Add(methods.FindByShortName("sqlite3_prepare*"));
sqlit3Meth.Add(methods.FindByShortName("sqlite3_bind_*"));

CxList executeQuery = methods.FindByShortName("executeQuery*");
	
CxList inParams = All.GetParameters(sqlit3Meth, 1);
inParams.Add(All.GetParameters(executeQuery, 0));
inParams = inParams -theParams;

result = inParams;