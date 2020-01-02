CxList psw = Find_Passwords();
CxList psw_in_lSide = psw.FindByAssignmentSide(CxList.AssignmentSide.Left);
CxList psw_in_lSide_decl = psw_in_lSide.FindByType(typeof(Declarator));

CxList strLiterals = Find_Strings();
strLiterals -= strLiterals.FindByName("");

//when the hardcoded string includes a space or dot we believe 
//it is not a password string
strLiterals -= strLiterals.FindByName("* *");
strLiterals -= strLiterals.FindByName("*.*");
strLiterals -= strLiterals.FindByName("*/*");
strLiterals -= strLiterals.FindByName("*\\*");

CxList lit_in_rSide = strLiterals.FindByAssignmentSide(CxList.AssignmentSide.Right);

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

// Find password in an compare operations
List < string > compareMethodsNames = new List<string>{"Compare", "EqualFold"};
CxList compareMethods = 
	psw.GetAncOfType(typeof(MethodInvokeExpr))
	.FindByShortNames(compareMethodsNames);
compareMethods = strLiterals.GetByAncs(compareMethods);

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

// Password in simple assignment
CxList assignPassword = psw_in_lSide.GetAncOfType(typeof(AssignExpr));
assignPassword = lit_in_rSide.GetByAncs(assignPassword);

result = PasswordInDecl;
result.Add(compareMethods);
result.Add(assignPassword);
result.Add(EqualOperatorStrings);