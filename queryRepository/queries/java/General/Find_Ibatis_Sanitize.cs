// ibatis sanitizer
CxList strings = Find_Strings();

CxList param1 = strings.GetParameters(All.FindByMemberAccess("*.createQuery").FindByFileName("*.xml"), 0);
SortedList pStrings = new SortedList();

foreach (CxList p in param1)
{
	StringLiteral s = p.TryGetCSharpGraph<StringLiteral>();
	pStrings[s.ShortName.Trim('"')] = s.ShortName.Trim('"');
}

CxList ibatis = Ibatis();
CxList ibatisSanitizers = ibatis - ibatis.FindByShortName("execute*");
ibatisSanitizers = strings.GetParameters(ibatisSanitizers, 0);
result = ibatisSanitizers;
foreach (string s in pStrings.Values)
{
	result -= ibatisSanitizers.FindByShortName('"' + s + '"', false);		 
}

result = All.GetParameters(ibatis.FindByParameters(result));