CxList iterationStmt = Find_IterationStmt();
string regexPrefix = @"[\s\r\n};]?";
string balancedParentheses = @"([^()]|(?<open>\()|(?<-open>\)))+";
string testIfBalanced = @"(?(open)(?!))";
CxList.CxRegexOptions regexOptions = CxList.CxRegexOptions.DoNotSearchInStringLiterals | CxList.CxRegexOptions.AllowOverlaps;

CxList ifStmt = Find_Ifs().FindByRegex(regexPrefix + @"if(\s)*\(" + balancedParentheses + @"\)[\s\r\n]*[^{\s\r\n]" + testIfBalanced, regexOptions);
CxList elseStmt = All.FindByRegex(regexPrefix + @"else(\s)*[^{]*;", regexOptions);
CxList whileStmt = iterationStmt.FindByRegex(regexPrefix + @"while(\s)*\(" + balancedParentheses + @"\)[\s\r\n]*[^{\s\r\n;]" + testIfBalanced, regexOptions);
CxList forStmt = iterationStmt.FindByRegex(regexPrefix + @"for(\s)*\(" + balancedParentheses + @"\)[\s\r\n]*[^{\s\r\n;]" + testIfBalanced, regexOptions);
CxList doWhileStmt = iterationStmt.FindByRegex(regexPrefix + @"do[\s\r\n]+[^{\s\r\n]", regexOptions);

result.Add(ifStmt);
result.Add(elseStmt);
result.Add(whileStmt);
result.Add(forStmt);
result.Add(doWhileStmt);

result -= Find_Properties_Files();