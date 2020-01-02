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

// Find password in an "Equals" operation
CxList eq = Get_Conditions();
eq = All.FindByFathers(eq.FindByType(typeof(BinaryExpr)));
CxList eq1 = eq * psw;
CxList equalsPassword = strLiterals.GetByAncs(eq1);
eq1 = eq * strLiterals;
equalsPassword.Add(psw.GetByAncs(eq1));

// Find Password in an assign
CxList assignPassword = psw_in_lSide.GetAncOfType(typeof(AssignExpr));
assignPassword = lit_in_rSide.GetByAncs(assignPassword);

//Find password in StrComp method	==> StrComp("hello",pwd,0)
CxList methods = Find_Methods();
CxList StrComp = methods.FindByShortName("StrComp", false);
CxList StrCompParam1 = All.GetParameters(StrComp, 0);
CxList StrCompParam2 = All.GetParameters(StrComp, 1);
CxList hPassInStrComp = All.FindByParameters(StrCompParam1 * psw).FindByParameters(StrCompParam2 * strLiterals);
hPassInStrComp.Add(All.FindByParameters(StrCompParam2 * psw).FindByParameters(StrCompParam1 * strLiterals));
hPassInStrComp = psw.GetParameters(hPassInStrComp);
hPassInStrComp = hPassInStrComp.FindByType(typeof (UnknownReference));

result = equalsPassword + initializePassword + assignPassword + hPassInStrComp;