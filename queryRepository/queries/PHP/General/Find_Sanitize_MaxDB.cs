CxList methods = Find_Methods();
CxList unkRefs = Find_UnknownReferences();

CxList maxDbSanitizer = methods.FindByShortName("maxdb_escape_string");
maxDbSanitizer.Add(methods.FindByShortName("maxdb_real_escape_string"));

CxList maxDBTarget = unkRefs.FindByType("maxdb");
CxList maxDBMembersSanitizers = maxDBTarget.GetMembersOfTarget().FindByShortName("real_escape_string");

result.Add(maxDbSanitizer);
result.Add(maxDBMembersSanitizers);