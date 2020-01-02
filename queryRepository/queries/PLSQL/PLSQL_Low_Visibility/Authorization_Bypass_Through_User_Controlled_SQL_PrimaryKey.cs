// database
CxList db = Find_Dynamic_DB_In();

// strings
CxList strings = Find_Strings();
// strings that end with "id" (there might be also "pid" and others, but then I'm starting with many
// potential false positives
CxList id = 
	strings.FindByShortName("*id",false) + 
	strings.FindByShortName("*id=",false) + 
	strings.FindByShortName("*id =",false) + 
	strings.FindByShortName("*id = ",false);

/// DB influenced by potentially problematic input
db = db.DataInfluencedBy(id);
CxList dbMembers = db.GetMembersOfTarget();
db.Add(dbMembers.FindByShortName("USING", false) + dbMembers.GetMembersOfTarget().FindByShortName("USING", false));


CxList implicitCursor = 
	All.FindByName("CX_SQL_STATEMENT", false).
	GetMembersOfTarget().FindByShortName("SELECT",false).GetMembersOfTarget();

implicitCursor = 
	implicitCursor.FindByShortName("FROM", false) + 
	implicitCursor.FindByShortName("INTO", false).GetMembersOfTarget().FindByShortName("FROM", false);

implicitCursor = implicitCursor.GetMembersOfTarget().FindByShortName("WHERE", false);
implicitCursor -= All.GetByBinaryOperator(Checkmarx.Dom.BinaryOperator.BooleanAnd).GetAncOfType(typeof(MethodInvokeExpr));

CxList implicitCursorParams = All.GetParameters(implicitCursor);
implicitCursorParams  = All.GetByAncs(implicitCursorParams).FindByShortName("*id", false);
implicitCursorParams = implicitCursorParams.GetAncOfType(typeof(BinaryExpr)).GetAncOfType(typeof(MethodInvokeExpr));

// Interative inputs, that are influenced by this id
CxList input = Find_Inputs();
result = input.DataInfluencingOn(implicitCursorParams);
input = input.DataInfluencingOn(id + dbMembers);
result.Add(input.DataInfluencingOn(db));