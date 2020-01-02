CxList methods = Find_Methods();
CxList unkRefs = Find_UnknownReferences();

result.Add(methods.FindByShortName("sqlite_escape_string"));
result.Add(methods.FindByShortName("sqlite_udf_encode_binary"));

CxList sqLite3Target = unkRefs.FindByType("sqlite3",false);
CxList sqLite3MembersSanitizers = sqLite3Target.GetMembersOfTarget().FindByShortName("escapeString");

result.Add(sqLite3MembersSanitizers);