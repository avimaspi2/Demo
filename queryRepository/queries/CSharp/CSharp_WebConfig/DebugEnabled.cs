CxList webConfig = Find_Web_Config();
CxList value_true = webConfig.FindByName("true").FindByType(typeof(StringLiteral));
CxList system_web = webConfig.FindByMemberAccess("system.web", false);
CxList compilation_Debug = system_web.GetMembersOfTarget().GetMembersOfTarget().FindByShortName("debug", false);

result = value_true * value_true.DataInfluencingOn(compilation_Debug);