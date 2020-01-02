// ibatis sanitizer
CxList param1 = Find_Strings().GetParameters(All.FindByMemberAccess("*.createQuery").FindByFileName("*.xml"), 0);
SortedList pStrings = new SortedList();

foreach (CxList p in param1)
{
	StringLiteral s = (StringLiteral) p.data.GetByIndex(0);
	pStrings[s.ShortName.Trim('"')] = s.ShortName.Trim('"');
}

CxList ibatis = Ibatis();
CxList ibatisSanitizers = ibatis - ibatis.FindByShortName("execute*");
ibatisSanitizers = Find_Strings().GetParameters(ibatisSanitizers, 0);
result = ibatisSanitizers;
foreach (string s in pStrings.Values)
{
	result -= ibatisSanitizers.FindByShortName('"' + s + '"', false);		 
}

result = All.GetParameters(ibatis.FindByParameters(result));