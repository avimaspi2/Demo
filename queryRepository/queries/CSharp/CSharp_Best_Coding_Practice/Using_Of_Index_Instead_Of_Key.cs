// using of index as key in SortedList or Dictionary
string regexString = @"\[[^\]]+\.Count[^\]]*\]";

result.Add(All.FindByType("SortedList").FindByRegex(regexString));
result.Add(All.FindByType("Dictionary").FindByRegex(regexString));