result.Add(Find_Integers());

//regex to replace
result.Add(All.FindByMemberAccess("Matcher.replaceAll"));
result.Add(All.FindByMemberAccess("Matcher.replaceFirst"));
result.Add(All.FindByMemberAccess("Matcher.appendReplacement"));
result.Add(Find_Replace_Regex());

//regex filter
result.Add(All.FindByMemberAccess("Matcher.group"));
result.Add(All.FindByMemberAccess("Pattern.split"));

//esapi
CxList ESAPI = Get_ESAPI();
result.Add(ESAPI.FindByMemberAccess("Encoder.encodeForDN"));
result.Add(ESAPI.FindByMemberAccess("Encoder.encodeForLDAP"));