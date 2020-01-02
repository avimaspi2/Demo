CxList headersMethods = Find_Set_Headers();

CxList cspLiteral = Find_Strings().FindByShortName("Content-Security-Policy");
CxList cspAsVariable = cspLiteral.GetParameters(headersMethods, 0);
CxList cspAsParameter = All.GetParameters(headersMethods, 0).DataInfluencedBy(cspLiteral);

CxList influenced = All.NewCxList();
influenced.Add(cspAsVariable);
influenced.Add(cspAsParameter);

result = headersMethods.DataInfluencedBy(influenced);