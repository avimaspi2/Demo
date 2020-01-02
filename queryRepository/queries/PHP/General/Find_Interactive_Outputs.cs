CxList methods = Find_Methods();
result = methods.FindByShortNames(new List<string> {"echo", "print", "header", "printf","print_r","vprintf","var_dump", "readfile"});

// Method export is not an output if the second parameter is true or 1
CxList var_exports = methods.FindByShortName("var_export");
CxList second_param = All.GetParameters(var_exports, 1).FindByType(typeof(Param));
result.Add(var_exports - second_param.GetAncOfType(typeof(MethodInvokeExpr)));
second_param = second_param.FindByShortNames(new List<string>(){"null","false","0","\"\"","''","\"0\""}, false);
result.Add(second_param.GetAncOfType(typeof(MethodInvokeExpr)));

//print_r method is not an output if the second parameter is true
CxList print_r = methods.FindByShortName("print_r");
CxList booleans = All.FindByType(typeof(BooleanLiteral)).FindByShortName("true");
booleans.Add(Find_UnknownReferences().FindByShortName("TRUE"));
CxList leftSide = booleans.GetAssignee(All);
booleans.Add(All.FindAllReferences(leftSide));
second_param = All.GetParameters(print_r, 1) * booleans;
result -= second_param.GetAncOfType(typeof(MethodInvokeExpr));

result.Add(Find_Zend_Interactive_Outputs());
result.Add(Find_Kohana_Interactive_Outputs());
result.Add(Find_Smarty_Interactive_Outputs());
result.Add(Find_Cake_Interactive_Outputs());
result.Add(Find_Symfony_Interactive_Outputs());