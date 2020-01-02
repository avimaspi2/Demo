CxList emptyString = Find_Empty_Strings();
CxList NULL = Find_Apex_Files().FindByName("null");
CxList psw = Find_Passwords();

// Find password in an initialization operation
CxList psw_in_lSide = psw.FindByAssignmentSide(CxList.AssignmentSide.Left);
CxList psw_in_lSide_decl = psw_in_lSide.FindByType(typeof(Declarator));

CxList strLiterals = Find_Strings() - emptyString - NULL;

CxList lit_in_rSide = strLiterals.FindByAssignmentSide(CxList.AssignmentSide.Right);

CxList initializedPassword = psw_in_lSide_decl.FindByInitialization(lit_in_rSide);

// Find password in an "equals" operation
CxList eq = All.FindByMemberAccess("String.equals");
eq *= psw.GetMembersOfTarget();
CxList equalsPassword = strLiterals.GetByAncs(eq);

eq = All.FindByMemberAccess("String.equals");
eq *= strLiterals.GetMembersOfTarget();
equalsPassword.Add(psw.GetByAncs(eq));

CxList assignPassword = psw_in_lSide.GetAncOfType(typeof(AssignExpr));
assignPassword = lit_in_rSide.GetByAncs(assignPassword);

result = initializedPassword + equalsPassword + assignPassword;

result -= Find_Test_Code();