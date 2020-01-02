// Find all strings that are use by a "match" command

CxList match = Find_Match();
CxList allMatchString = All.GetByAncs(All.GetParameters(match, 0));

result.Add(allMatchString.FindByType(typeof(UnknownReference)));
result.Add(allMatchString.FindByType(typeof(StringLiteral)));
result.Add(allMatchString.FindByType(typeof(MemberAccess)));