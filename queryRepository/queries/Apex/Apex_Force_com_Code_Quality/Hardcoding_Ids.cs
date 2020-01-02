CxList emptyString = Find_Empty_Strings();
CxList NULL = Find_Apex_Files().FindByName("null");
CxList id = Find_Id() * Find_Apex_Files();

CxList id_in_lSide = id.FindByAssignmentSide(CxList.AssignmentSide.Left);
	
// Find id in an initialization operation
CxList id_in_Decl = id_in_lSide.FindByType(typeof(Declarator));

CxList strLiteralsAll = Find_Strings() - emptyString - NULL;
CxList strLiterals = All.NewCxList();
foreach (CxList strLiteral in strLiteralsAll)
{
	CSharpGraph literal = strLiteral.data.GetByIndex(0) as CSharpGraph;
	if ((literal.ShortName.Length == 22) || (literal.ShortName.Length == 19))
	{
		// The actual lengths are 15 and 18, but we add 2 per side for the quotes
		strLiterals.Add(strLiteral);
	}
}


CxList lit_in_rSide = strLiterals.FindByAssignmentSide(CxList.AssignmentSide.Right);
CxList initializedId = id_in_Decl.FindByInitialization(lit_in_rSide);

// Find Id in assignment
CxList assignment = strLiterals.GetFathers().FindByType(typeof(AssignExpr));
CxList assignId = id_in_lSide.GetByAncs(assignment);

// Find id in an "equals" operation
CxList strEquals = All.FindByMemberAccess("String.equals", false);
CxList eq = strEquals * id.GetMembersOfTarget();
CxList equalsId = strLiterals.GetByAncs(eq);

eq = strEquals * strLiterals.GetMembersOfTarget();
equalsId.Add(id.GetByAncs(eq));

CxList conditions = Get_Conditions().FindByType(typeof(BinaryExpr));
conditions = conditions.FindByShortName("==");

CxList conditionId = All.NewCxList();
strLiterals = strLiterals.GetByAncs(conditions);
id = id.GetByAncs(conditions);
foreach (CxList condition in conditions)
{
	if ((id.FindByFathers(condition).Count > 0) && (strLiterals.FindByFathers(condition).Count > 0))
	{
		conditionId.Add(strLiterals.GetByAncs(condition));
	}
}

CxList selectId = Find_Strings().GetByAncs(All.GetParameters(Find_DB_Inactive())).FindByShortName("*id = '*");

result = initializedId + assignId + equalsId + conditionId + selectId;

result -= Find_Test_Code();