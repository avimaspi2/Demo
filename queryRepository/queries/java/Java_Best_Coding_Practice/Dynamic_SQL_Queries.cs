CxList db = Find_SQL_DB_In() - Find_Hibernate_DB();
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

CxList javaDBUnkonRef = All.NewCxList();
javaDBUnkonRef.Add(javaDB.FindByType(typeof(UnknownReference)));
javaDBUnkonRef.Add(javaDB.FindByType(typeof(Declarator)));

CxList str = javaDBUnkonRef.FindByType("String");
str.Add(stringMethods);

// Find strings of type member access (e.g. a.str)
str.Add(All.FindByType("String").FindByType(typeof(MemberAccess)));

// Find all string variables that are all uppercase - assume these are constants
CxList constants = All.NewCxList();
foreach(CxList s in str)
{
	try
	{
		CSharpGraph graph = s.TryGetCSharpGraph<CSharpGraph>();
		if(graph != null)
		{
			String shortname = graph.ShortName;
			if (shortname == shortname.ToUpper())
			{
				constants.Add(s);
			}
		}
	}
	catch(Exception ex)
	{
		cxLog.WriteDebugMessage(ex);
	}
}
str -= constants;

// Ignore constant declarations
CxList constDecl = Find_Constants();
str -= All.FindAllReferences(constDecl);

// Refine the append to include only appends that are ancestors of strings
append = str.GetByAncs(append).GetAncOfType(typeof(MethodInvokeExpr));

// binary operations whose descendants are strins
CxList binaryAncOfStr = str.GetAncOfType(typeof(BinaryExpr));

CxList concat = All.NewCxList();
concat.Add(binaryAncOfStr);
concat.Add(append);
concat.Add(str.GetByAncs(binary.GetByAncs(db)));


// Find the '+=' operators
CxList assignments = Find_AssignExpr();
CxList assignAdd = All.NewCxList();
foreach(CxList assignment in assignments)
{
	try
	{
		AssignExpr graph = assignment.TryGetCSharpGraph<AssignExpr>();
		if(graph != null && graph.Operator == AssignOperator.AdditionAssign)
			assignAdd.Add(assignment);	
	}
	catch(Exception ex)
	{
		cxLog.WriteDebugMessage(ex);
	}
}
// Find all strings on the right of '+=' operator
CxList rightAssignments = All.FindByAssignmentSide(CxList.AssignmentSide.Right).GetByAncs(assignAdd);
concat.Add(str.GetByAncs(rightAssignments));

CxList sanitize = All.NewCxList();
sanitize.Add(Find_Parameters());
sanitize.Add(Find_Dead_Code_Contents());
sanitize.Add(All.FindByMemberAccess("DriverManager.getConnection"));

CxList substring = All.FindByMemberAccess("String.substring");
sanitize.Add(All.GetByAncs(All.GetParameters(substring)));

result = db.InfluencedByAndNotSanitized(concat, sanitize).ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);