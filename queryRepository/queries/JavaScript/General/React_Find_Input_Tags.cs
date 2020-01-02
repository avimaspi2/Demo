CxList methods = Find_Methods();
CxList stringLiterals = Find_String_Literal();
CxList cElement = methods.FindByMemberAccess("React.createElement");
CxList cElementStrings = stringLiterals.GetByAncs(cElement);

CxList inputs = stringLiterals.FindByShortNames(new List<string> {"input", "select", "textarea", "datalist"});

result = cElementStrings * inputs;