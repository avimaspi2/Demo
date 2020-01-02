result = Find_General_Sanitize();

//regex to replace
result.Add(All.FindByMemberAccess("Matcher.replaceAll"));
result.Add(All.FindByMemberAccess("String.replaceAll"));
result.Add(All.FindByMemberAccess("Matcher.replaceFirst"));
result.Add(All.FindByMemberAccess("Matcher.appendReplacement"));

//regex filter
result.Add(All.FindByMemberAccess("Matcher.group"));
result.Add(All.FindByMemberAccess("String.split"));
result.Add(All.FindByMemberAccess("Pattern.split"));