CxList methods = Find_Methods();

//SPEL 
CxList parseMethods = methods.FindByMemberAccess("SpelExpressionParser.parseExpression");
parseMethods.Add(methods.FindByMemberAccess("SpelExpressionParser.doParseExpression"));
parseMethods.Add(methods.FindByMemberAccess("SpelExpressionParser.parseRow"));

//Spring eval tag
CxList evalJspOutput = methods.FindByMemberAccess("spring.eval");

result.Add(parseMethods);
result.Add(evalJspOutput);