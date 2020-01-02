CxList methods = Find_Methods();

CxList write = Find_Write();

CxList parameters = Find_Strings().GetParameters(write);
CxList writeStrings = methods.FindByParameters(parameters);
write = write - writeStrings;

result = Find_Log_Outputs();
result.Add(write);
result.Add(Set_Session_Attribute());
result.Add(methods.FindByShortName("SecurityUtils"));
result.Add(All.FindByTypes(new string [] {"HttpClient", "HttpServletResponse"}));
result.Add(methods.FindByMemberAccess("URLConnection.*"));