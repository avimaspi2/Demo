CxList psw = Find_Passwords() - Find_Password_Strings();
psw -= psw.FindByType(typeof(MethodDecl));

CxList psw_in_lSide = psw.FindByAssignmentSide(CxList.AssignmentSide.Left);
CxList psw_in_lSide_decl = psw_in_lSide.FindByType(typeof(Declarator));

CxList allParams = Find_Param();
CxList nullLiterals = Find_Null_Literals();

CxList lit_in_rSide = nullLiterals.FindByAssignmentSide(CxList.AssignmentSide.Right);

CxList stringsOfPass = nullLiterals.GetByAncs(psw) - nullLiterals.GetByAncs(allParams);

CxList PasswordlSide = psw_in_lSide_decl.FindByInitialization(lit_in_rSide);
CxList PasswordDecl = stringsOfPass.GetAncOfType(typeof(Declarator));
CxList PasswordRef = stringsOfPass.GetAncOfType(typeof(UnknownReference));
// Remove double results
PasswordRef -= PasswordRef.GetByAncs(PasswordDecl);

CxList PasswordInDecl =All.NewCxList();
PasswordInDecl.Add(PasswordlSide);
PasswordInDecl.Add(PasswordRef);
PasswordInDecl.Add(PasswordDecl);

// Password in simple assignment
CxList assignPassword = psw_in_lSide.GetAncOfType(typeof(AssignExpr));
assignPassword = lit_in_rSide.GetByAncs(assignPassword);

result.Add(PasswordInDecl);
result.Add(assignPassword);

// Handle swift subscripts, e.g. dict[password] = nil
{
	CxList subscriptMethods = Find_Methods().FindByShortName("subscript").FindByFileName("*.swift");
	
	CxList subscriptMethodsWithNullPasswords = All.NewCxList();
	foreach (CxList m in subscriptMethods)
		if (psw.GetParameters(m, 0).Count > 0 && nullLiterals.GetParameters(m, 1).Count > 0)
			subscriptMethodsWithNullPasswords.Add(m);
	
	result.Add(nullLiterals.GetParameters(subscriptMethodsWithNullPasswords, 1));
}