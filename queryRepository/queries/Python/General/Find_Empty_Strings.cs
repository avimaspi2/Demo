CxList stringsLiterals = Find_Strings();

List<string> strNames = new List<string> {"\"\"","\'\'"};

result = stringsLiterals.FindByShortNames(strNames);