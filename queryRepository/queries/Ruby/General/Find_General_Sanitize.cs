CxList methods = Find_Methods();
result.Add((All.FindByMemberAccess("Digest.SHA1") + All.FindByMemberAccess("Digest.MD5")).GetMembersOfTarget());
result.Add(methods.FindByShortName("sanitize_limit"));

// errors is an object, that does not reflect the input, so we want it to sanitize any query with sanitization
result.Add(All.FindByShortName("errors").GetTargetOfMembers());
result.Add(All.FindByFathers(All.FindByShortName("__errors").GetAncOfType(typeof(BinaryExpr))));


result.Add(Find_Integers());
result.Add(result.GetMembersOfTarget());
result.Add(result.GetMembersOfTarget());