CxList methods = Find_Methods();
CxList validate = methods.FindByShortName("validate*");

CxList regexValidation = validate.FindByRegex(@"/[^/\n]*/");

CxList toRemove = regexValidation.FindByRegex(@"/\^[^$|\n]*\$(\|(\^[^|$\n]*\$))*/");
toRemove.Add(regexValidation.FindByRegex(@"/\\A.*?\\z(\|\\A.*?\\z)*/"));

result = regexValidation - toRemove;