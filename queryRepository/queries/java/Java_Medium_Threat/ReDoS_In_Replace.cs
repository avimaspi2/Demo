CxList evilStringsForReplace = Find_Evil_Strings_For_Replace();
CxList replaceRegexFirsParam = Find_Replace_Regex_In_First_Param();
CxList replaceRegexSecondParam = Find_Replace_Regex_In_Second_Param();


result = Find_ReDoS(evilStringsForReplace, replaceRegexFirsParam, 0, false);

result.Add(Find_ReDoS(evilStringsForReplace, replaceRegexSecondParam, 1, false));