CxList emptyString = Find_Empty_Strings();
CxList NULL = All.FindByName("null");
CxList psw = Find_Passwords();

// Find password in an initialization operation
CxList psw_in_lSide = psw.FindByAssignmentSide(CxList.AssignmentSide.Left);

CxList psw_in_lSide_decl = psw_in_lSide.FindByType(typeof(Declarator));

CxList strLiterals = Find_Strings() - emptyString - NULL;

//when the hardcoded string includes a space or dot we believe 
//it is not a password string
strLiterals -= strLiterals.FindByName("* *");
strLiterals -= strLiterals.FindByName("*.*");
strLiterals -= strLiterals.FindByName("*/*");
strLiterals -= strLiterals.FindByName("*\\*");

CxList lit_in_rSide = strLiterals.FindByAssignmentSide(CxList.AssignmentSide.Right);
CxList initializePassword = psw_in_lSide_decl.FindByInitialization(lit_in_rSide);
initializePassword = lit_in_rSide.GetByAncs(initializePassword);

// Find password in assignment
CxList assignPassword = psw_in_lSide.GetAncOfType(typeof(AssignExpr));
assignPassword = lit_in_rSide.GetByAncs(assignPassword);

// Find pasword comparison expression
CxList compareOperator = 
	All.GetByBinaryOperator(BinaryOperator.IdentityEquality) + 
	All.GetByBinaryOperator(BinaryOperator.IdentityInequality);
compareOperator *= psw.GetAncOfType(typeof(BinaryExpr));

CxList comparePassword = strLiterals.FindByFathers(compareOperator);

result = initializePassword + assignPassword + comparePassword;