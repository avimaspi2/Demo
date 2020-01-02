CxList inputs = Find_Inputs();
CxList methods = Find_Methods();
CxList formatString = methods.FindByShortName("printf") + methods.FindByShortName("sprintf");
formatString = All.GetParameters(formatString, 0);

result = formatString.DataInfluencedBy(inputs);