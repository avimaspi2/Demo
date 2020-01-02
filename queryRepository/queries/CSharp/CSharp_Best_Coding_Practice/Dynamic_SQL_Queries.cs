CxList db = Find_DB_Base();
CxList dbMethods = All.GetMethod(db); // All methods that contain a DB (others are usually irrelevant)
CxList csDB = All.GetByAncs(dbMethods);
CxList methods = Find_Methods();

// Add all methods called by the DB methods - up to 3 levels of calls
for (int i = 0; i < 3; i++)
{
	CxList dbDelta = All.FindAllReferences(methods * csDB);
	dbMethods.Add(dbDelta);
	csDB.Add(All.GetByAncs(dbDelta));
}

CxList binary = csDB.FindByType(typeof(BinaryExpr));

CxList stringMethods = (csDB * methods).FindAllReferences(All.FindByReturnType("String"));
CxList append = csDB.FindByShortName("append", false);

CxList formats = methods.FindByMemberAccess("String.Format", false);
CxList formatParameters = formats.FindByParameters(All.GetParameters(formats, 1));

CxList dbVariables = csDB.FindByType(typeof(UnknownReference));
dbVariables.Add(csDB.FindByType(typeof(Declarator)));
CxList str = dbVariables.FindByTypes(new String[] {"String", "System.String"});
str.Add(stringMethods);

CxList integers = Find_Integers() - db;

CxList concat = binary.InfluencingOnAndNotSanitized(str, integers);
concat.Add(str.GetByAncs(binary.GetByAncs(db)));
concat.Add(append);
concat.Add(formatParameters);

CxList sanitize = Find_Parameters();
CxList substring = methods.FindByMemberAccess("String.substring", false);
sanitize.Add(All.GetByAncs(All.GetParameters(substring)));
sanitize.Add(integers);

result = db.InfluencedByAndNotSanitized(concat, sanitize);

result = result.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);