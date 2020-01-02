CxList booleanDeclarators = All.FindByType(typeof(BooleanLiteral)).GetAncOfType(typeof(Declarator));
CxList booleanReferences = All.FindAllReferences(booleanDeclarators);

CxList toRemove = Find_Strings();
toRemove.Add(booleanDeclarators);
toRemove.Add(booleanReferences);

CxList decode = (All - toRemove).FindByShortName("*decode*", false);
CxList sanitize = Find_XSS_Sanitize();
CxList output = Find_Interactive_Outputs();

result = output.InfluencedByAndNotSanitized(decode, sanitize);