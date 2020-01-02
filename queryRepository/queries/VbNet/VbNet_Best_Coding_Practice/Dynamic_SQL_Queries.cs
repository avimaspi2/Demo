CxList db = Find_DB_Base();
CxList dbMethods = All.GetMethod(db); // All methods that contain a DB (others are usually irrelevant)
CxList vbDB = All.GetByAncs(dbMethods);
CxList methods = Find_Methods();
CxList methDeclInvoke = All.FindByType(typeof(MethodDecl));
methDeclInvoke.Add(methods);

// Add all methods called by the DB methods - up to 3 levels of calls
for (int i = 0; i < 3; i++)
{
	CxList dbDelta = methDeclInvoke.FindAllReferences(methods * vbDB);
	dbMethods.Add(dbDelta);
	vbDB.Add(All.GetByAncs(dbDelta));
}

CxList binary = vbDB.FindByType(typeof(BinaryExpr));

CxList stringMethods = (vbDB * methods).FindAllReferences(All.FindByReturnType("String", false));
CxList append = vbDB.FindByShortName("Append", false);

CxList str = (vbDB.FindByType(typeof(UnknownReference)) + 
	vbDB.FindByType(typeof(Declarator))).FindByType("String", false);
str.Add(stringMethods);

CxList concat = binary.DataInfluencingOn(str);
concat.Add(str.GetByAncs(binary.GetByAncs(db)));
concat.Add(append);

CxList sanitize = Find_Parameters();
CxList substring = methods.FindByMemberAccess("String.Substring", false);
sanitize.Add(All.GetByAncs(All.GetParameters(substring)));

result = db.InfluencedByAndNotSanitized(concat, sanitize);