// Find regexes (influenced by input) that do not contain /g, but influence a file open

CxList methods = Find_Methods();
CxList inputs = Find_Inputs();
CxList regex = Find_Regex();
regex -= regex.FindByName(@"*/g*");
regex -= regex.FindByRegex(@"/g");

CxList regexBin = regex.GetAncOfType(typeof(BinaryExpr));

result = regexBin.DataInfluencedBy(inputs);