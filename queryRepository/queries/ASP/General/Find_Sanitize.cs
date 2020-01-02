CxList methods = Find_Methods();

CxList ibatis = methods.FindByMemberAccess("sqlmapper.queryforobject") + 
				methods.FindByMemberAccess("sqlmapper.queryforlist") + 
				methods.FindByMemberAccess("sqlmapper.querywithrowhandler") + 
				methods.FindByMemberAccess("sqlmapper.queryforpaginatedlist") + 
				methods.FindByMemberAccess("sqlmapper.queryformap") + 
				methods.FindByMemberAccess("sqlmapper.insert") + 
				methods.FindByMemberAccess("sqlmapper.update") + 
				methods.FindByMemberAccess("sqlmapper.delete");

CxList qSQL1 = methods.FindByMemberAccess("qsqlquery.bindvalue");
CxList qSQL2 = methods.FindByMemberAccess("qsqlquery.addbindvalue");

CxList repl1 = methods.FindByShortName("replace");
repl1 = All.GetByAncs(All.GetParameters(repl1, 1));
CxList repl2 = methods.FindByShortName("replaceall");
repl2 = All.GetByAncs(All.GetParameters(repl2, 0));

result = Find_Replace() + 
	Find_Parameters() + 
	Find_Integers() + 
	All.FindByType("bool") +
	All.GetParameters(ibatis, 1) +
	All.GetParameters(qSQL1, 1) +
	All.GetParameters(qSQL2, 0) +
	Find_DB().FindByShortName("update",false) +
	Find_Connection_DB() + 
	repl1 + repl2;