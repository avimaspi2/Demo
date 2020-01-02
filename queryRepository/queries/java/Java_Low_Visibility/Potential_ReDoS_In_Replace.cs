CxList evilStringsForReplace = Find_Evil_Strings_For_Replace();

result = Find_ReDoS(evilStringsForReplace, Find_Replace_Regex_In_First_Param(), 0, true);

result.Add(Find_ReDoS(evilStringsForReplace, Find_Replace_Regex_In_Second_Param(), 1, true));