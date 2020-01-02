// JdbcTemplate methods
CxList methods = Find_Methods();
	
List <string> JdbcTemplateMethodsNames = new List<string>() {"query*", "update*", "execute*", "batchUpdate*"};
CxList JdbcTemplateMethods = methods.FindByShortNames(JdbcTemplateMethodsNames);

CxList JdbcClasses = All.InheritsFrom("JdbcTemplate");
CxList relevantMethods = JdbcTemplateMethods.GetByAncs(JdbcClasses);

CxList getJdbcTemplateMembers = methods.FindByShortName("getJdbcTemplate").GetMembersOfTarget();
relevantMethods.Add(getJdbcTemplateMembers * JdbcTemplateMethods);

result = All.GetParameters(relevantMethods);