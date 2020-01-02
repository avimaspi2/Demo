CxList db = Find_SQL_DB_In();
CxList java = All - Find_Jsp_Code();
CxList javaDBMethods = java.GetMethod(db); // All methods that contain a DB (others are usually irrelevant)
CxList javaDB = java.GetByAncs(javaDBMethods);
CxList methods = Find_Methods();

// Add all methods called by the DB methods - up to 3 levels of calls
for (int i = 0; i < 3; i++)
{
	javaDBMethods.Add(java.FindAllReferences(methods * javaDB));
	javaDB.Add(All.GetByAncs(javaDBMethods));
}

CxList binary = javaDB.FindByType(typeof(BinaryExpr));

CxList stringMethods = (javaDB * methods).FindAllReferences(All.FindByReturnType("String"));
CxList append = javaDB.FindByShortName("append");

CxList str = (javaDB.FindByType(typeof(UnknownReference)) + 
	javaDB.FindByType(typeof(Declarator))).FindByType("String") +
	stringMethods;

CxList concat = 
	binary.DataInfluencingOn(str) +
	str.GetByAncs(binary.GetByAncs(db)) +
	append;

CxList sanitize = Find_Parameters() + Find_Dead_Code_Contents();
CxList substring = All.FindByMemberAccess("String.substring");
sanitize.Add(All.GetByAncs(All.GetParameters(substring)));

result = db.InfluencedByAndNotSanitized(concat, sanitize).ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);