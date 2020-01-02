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

CxList initializedPassword = psw_in_lSide_decl.FindByInitialization(lit_in_rSide);

//remove passwords with equal name and contant ==> currPassword = "currPassword";
CxList notHdPass = All.NewCxList();
char[] trimChars = new char[2] { '\'', '"'};
foreach(CxList currPass in initializedPassword)
{
	CxList currStrInLeft = lit_in_rSide.FindInitialization(currPass);
	string strName = currStrInLeft.GetName().Trim(trimChars);
	string passName = currPass.GetName();
	if (passName.Equals(strName))
	{
		notHdPass.Add(currPass);
	}
}
initializedPassword -= notHdPass;

// Find password in an "equals" operation
CxList eq = All.FindByMemberAccess("String.equals");
eq *= psw.GetMembersOfTarget();
CxList equalsPassword = strLiterals.GetByAncs(eq);

eq = All.FindByMemberAccess("String.equals");
eq *= strLiterals.GetMembersOfTarget();
equalsPassword.Add(psw.GetByAncs(eq));

//equals of type "string".equals(password);
CxList strEQ = strLiterals.GetMembersOfTarget().FindByShortName("equals");
strEQ = psw.GetByAncs(strEQ);
equalsPassword.Add(strEQ);

// Find password in assignments
CxList assignPassword = psw_in_lSide.GetAncOfType(typeof(AssignExpr));
assignPassword = lit_in_rSide.GetByAncs(assignPassword);
result = initializedPassword + equalsPassword + assignPassword;