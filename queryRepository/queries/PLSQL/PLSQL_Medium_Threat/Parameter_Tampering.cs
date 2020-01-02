CxList db = Find_Dynamic_DB_In();

CxList strings = Find_Strings();

CxList Select = strings.FindByName("*select*", false);
CxList Where = strings.FindByName("*where*", false);
CxList And = strings.FindByName("*And *", false) + 
	strings.FindByName("* And*", false);

db = db.DataInfluencedBy(Select).DataInfluencedBy(Where);
db -= db.DataInfluencedBy(And);

CxList dbMembers = db.GetMembersOfTarget();
db.Add(dbMembers.FindByShortName("USING", false) + dbMembers.GetMembersOfTarget().FindByShortName("USING", false));


CxList implicitCursor = 
	All.FindByName("CX_SQL_STATEMENT", false).
	GetMembersOfTarget().FindByShortName("SELECT", false).GetMembersOfTarget();

implicitCursor = 
	implicitCursor.FindByShortName("FROM", false) + 
	implicitCursor.FindByShortName("INTO", false).GetMembersOfTarget().FindByShortName("FROM", false);

implicitCursor = implicitCursor.GetMembersOfTarget().FindByShortName("WHERE", false);
implicitCursor -= All.GetByBinaryOperator(Checkmarx.Dom.BinaryOperator.BooleanAnd).GetAncOfType(typeof(MethodInvokeExpr));

CxList input = Find_Inputs();
result = input.DataInfluencingOn(implicitCursor) + input.DataInfluencingOn(db + dbMembers);