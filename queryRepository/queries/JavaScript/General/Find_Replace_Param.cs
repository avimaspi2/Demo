CxList replace = All.FindByMemberAccess("*.replace", false);
replace.Add(All.FindByMemberAccess("*.split", false));
result = All.GetParameters(replace, 0);