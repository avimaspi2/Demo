CxList psw = Find_Passwords();
CxList psw_in_lSide = psw.FindByAssignmentSide(CxList.AssignmentSide.Left);
CxList psw_in_lSide_decl = psw_in_lSide.FindByType(typeof(Declarator));
CxList strLiterals = Find_Strings();
CxList lit_in_rSide = strLiterals.FindByAssignmentSide(CxList.AssignmentSide.Right);
//when the hardcoded string includes a space or dot we believe 
//it is not a password string
lit_in_rSide -= lit_in_rSide.FindByName("* *");
lit_in_rSide -= lit_in_rSide.FindByName("*.*");
lit_in_rSide -= lit_in_rSide.FindByName("*/*");
lit_in_rSide -= lit_in_rSide.FindByName("*\\*");

//empty string is OK
lit_in_rSide -= Find_Empty_Strings();

// Password in declaration
CxList PasswordInDecl = psw_in_lSide_decl.FindByInitialization(lit_in_rSide);

//remove passwords with equal name and contant ==> currPassword = "currPassword";
CxList notHdPass = All.NewCxList();
char[] trimChars = new char[2] { '\'', '"'};
foreach(CxList currPass in PasswordInDecl)
{
	CxList currStrInLeft = lit_in_rSide.FindInitialization(currPass);
	string strName = currStrInLeft.GetName().Trim(trimChars);
	string passName = currPass.GetName();
	if (passName.Equals(strName))
	{
		notHdPass.Add(currPass);
	}
}
PasswordInDecl -= notHdPass;

CxList methods = Find_Methods();
CxList strcmp = methods.FindByShortName("strcmp")
	+ methods.FindByShortName("strncmp")
	+ methods.FindByShortName("bcmp")
	+ methods.FindByShortName("strcoll");
CxList strcmpParam1 = All.GetParameters(strcmp, 0);
CxList strcmpParam2 = All.GetParameters(strcmp, 1);

	//strcmp(password, "myPass")
	//strcnmp(password, "myPass", length)
	//bcmp(password, "myPass", cnt)
CxList hPassInStrcmp = All.FindByParameters(strcmpParam1 * psw).FindByParameters(strcmpParam2 * strLiterals);

	//strcmp("myPass", password)
	//strcnmp("myPass", password, length)
	//bcmp("myPass", password, cnt)
hPassInStrcmp.Add(All.FindByParameters(strcmpParam2 * psw).FindByParameters(strcmpParam1 * strLiterals));

// Find password in an "compare" operation
CxList eq = All.FindByMemberAccess("String.compare");
CxList equalsPassword = strLiterals.GetByAncs(eq * psw.GetMembersOfTarget());

eq *= strLiterals.GetMembersOfTarget();
equalsPassword.Add(psw.GetByAncs(eq));

// Find password in a '==' operator
CxList EqualBinaryExpr = psw.GetFathers().FindByType(typeof(BinaryExpr)).
	GetByBinaryOperator(Checkmarx.Dom.BinaryOperator.IdentityEquality);
CxList EqualOperatorStrings = All.NewCxList();
foreach(CxList bin in EqualBinaryExpr)
{
	CxList password = psw.FindByFathers(bin);
	CxList stringLit = strLiterals.FindByFathers(bin);
	
	if(password.Count > 0 && stringLit.Count > 0)
	{
		EqualOperatorStrings.Add(stringLit);
	}		
}
//equals of type "string".compare(password);
CxList strEQ = strLiterals.GetMembersOfTarget().FindByShortName("compare");
strEQ = psw.GetByAncs(strEQ);
equalsPassword.Add(strEQ);

// Password in simple assignment
CxList assignPassword = psw_in_lSide.GetAncOfType(typeof(AssignExpr));
assignPassword = lit_in_rSide.GetByAncs(assignPassword);

CxList macroPassword = strLiterals.GetParameters(Find_Methods().FindByShortName("CxPw"));

result = PasswordInDecl + hPassInStrcmp + equalsPassword + assignPassword + macroPassword + EqualOperatorStrings;