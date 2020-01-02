CxList psw = Find_Passwords() - Find_Password_Strings();
psw -= psw.FindByType(typeof(MethodDecl));

CxList methods = Find_Methods();
CxList subs_pass = methods.FindByShortName("*subscript*");

subs_pass = subs_pass.FindByParameters(psw);

CxList psw_in_lSide = psw.FindByAssignmentSide(CxList.AssignmentSide.Left);

CxList psw_in_lSide_decl = psw_in_lSide.FindByType(typeof(Declarator));

//CxList allUnknownRef = Find_UnknownReference();

CxList  strLiterals = Find_Empty_Strings();

CxList lit_in_rSide = strLiterals.FindByAssignmentSide(CxList.AssignmentSide.Right);
// Password in declaration
CxList stringsOfPass = strLiterals.GetByAncs(psw);
CxList PasswordlSide = psw_in_lSide_decl.FindByInitialization(lit_in_rSide);

CxList PasswordDecl = stringsOfPass.GetAncOfType(typeof(Declarator));
CxList PasswordRef = stringsOfPass.GetAncOfType(typeof(UnknownReference));

// Remove double results
PasswordRef -= PasswordRef.GetByAncs(PasswordDecl);

CxList PasswordInDecl = All.NewCxList();
PasswordInDecl.Add(PasswordlSide);
PasswordInDecl.Add(PasswordRef);
PasswordInDecl.Add(PasswordDecl);

// Password in simple assignment
CxList assignPassword = psw_in_lSide.GetAncOfType(typeof(AssignExpr));
assignPassword = lit_in_rSide.GetByAncs(assignPassword);

//Password in subscripts
CxList empty_password_in_subscript = strLiterals.GetParameters(subs_pass, 1);

result = PasswordInDecl;
result.Add(assignPassword);
result.Add(empty_password_in_subscript);