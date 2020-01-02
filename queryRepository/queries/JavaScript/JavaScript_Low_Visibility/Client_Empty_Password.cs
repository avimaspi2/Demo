// Query Client_Empty_Password
CxList open = Find_XmlHttp_Open();
CxList passwords = Find_String_Literal().GetParameters(open, 4);
CxList relevantPwd = passwords.FindByShortName("");

result = open.DataInfluencedBy(relevantPwd);