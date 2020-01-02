CxList hardcodedPasswordInConfig = Common_Low_Visibility.Use_Of_Hardcoded_Password_In_Config();

// Remove the i18n (Internationalization) files
hardcodedPasswordInConfig -= Find_Properties_I18N();

// Remove upper case string
// Example : PASSWORD
CxList passwordLowerCase = All.NewCxList();
foreach (CxList r in hardcodedPasswordInConfig)
{
	CSharpGraph g = r.TryGetCSharpGraph<CSharpGraph>();
	if(g != null && g.ShortName != null && g.ShortName.Length > 1)
	{
		if((char.IsUpper(g.ShortName[0]) && char.IsLower(g.ShortName[1]))
		|| (char.IsLower(g.ShortName[0]) && char.IsLower(g.ShortName[1]))){
			passwordLowerCase.Add(r);
		}
	}
}

CxList smallPassword = All.NewCxList();

// Remove long string in right side
// Example : PASSWORD = <long string>
CxList longAssigner = passwordLowerCase.GetAssigner();
foreach (CxList l in longAssigner)
{
	CSharpGraph g = l.TryGetCSharpGraph<CSharpGraph>();
	if(g != null && g.ShortName != null && g.ShortName.Length < 25)
	{
		smallPassword.Add(l);
	}
}

CxList smallPasswordLowerCase = smallPassword.GetAssignee();

CxList strLiterals = Find_Strings();
strLiterals -= strLiterals.FindByShortName("");
strLiterals -= Find_Empty_Strings();
strLiterals -= Find_Null_String_Name(); 
strLiterals -= All.FindByName("true"); 
strLiterals -= All.FindByName("false"); 

result = (smallPasswordLowerCase.GetAssigner() * strLiterals).GetAssignee();