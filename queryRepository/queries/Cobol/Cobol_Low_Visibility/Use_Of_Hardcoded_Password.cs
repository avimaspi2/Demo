CxList psw = Find_Passwords();
CxList pswInLSide = psw.FindByAssignmentSide(CxList.AssignmentSide.Left);
CxList pswInLSideDecl = pswInLSide.FindByType(typeof(Declarator));
CxList strLiterals = Find_Strings();
CxList litInRSide = strLiterals.FindByAssignmentSide(CxList.AssignmentSide.Right);
//when the hardcoded string includes a space or dot we believe 
//it is not a password string
litInRSide -= litInRSide.FindByName("* *");
litInRSide -= litInRSide.FindByName("*.*");
litInRSide -= litInRSide.FindByName("*/*");
litInRSide -= litInRSide.FindByName("*\\*");

//empty string is OK
litInRSide -= Find_Empty_Strings();

// Password in declaration
CxList PasswordInDecl = pswInLSideDecl.FindByInitialization(litInRSide);

//remove passwords with equal name and contant ==> currPassword = "currPassword";
CxList notHdPass = All.NewCxList();
char[] trimChars = new char[2] { '\'', '"'};
foreach(CxList currPass in PasswordInDecl)
{
	CxList currStrInLeft = litInRSide.FindInitialization(currPass);
	string strName = currStrInLeft.GetName().Trim(trimChars);
	string passName = currPass.GetName();
	if (passName.Equals(strName))
	{
		notHdPass.Add(currPass);
	}
}
PasswordInDecl -= notHdPass;


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
CxList assignPassword = pswInLSide.GetAncOfType(typeof(AssignExpr));
assignPassword = litInRSide.GetByAncs(assignPassword);

result = PasswordInDecl;
result.Add(assignPassword);
result.Add(EqualOperatorStrings);