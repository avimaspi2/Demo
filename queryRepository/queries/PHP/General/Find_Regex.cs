CxList regex = Find_Methods().FindByShortNames(new List<String>(){ "*preg*", "*ereg*" }, false);
regex.Add(Find_Methods().FindByShortName("*regex*"));

CxList nonSanitizingStrings = Find_Strings().FindByShortNames(new List<String>(){ @"/^.*$/", @"*^.*$*" });
nonSanitizingStrings = nonSanitizingStrings.GetAncOfType(typeof(MethodInvokeExpr));
result = regex - nonSanitizingStrings;