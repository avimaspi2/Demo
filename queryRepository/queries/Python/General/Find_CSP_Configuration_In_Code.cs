List<string> csp = new List<string>{"X-Content-Security-Policy", "Content-Security-Policy", "*Content-Security-Policy\""};
CxList cspString = Find_MemberAccesses().FindByShortNames(csp);
result.Add(cspString);

CxList unknownRefs = Find_UnknownReference();
CxList httpResponse = Find_Methods().FindByShortName("HttpResponse");
CxList httpReferences = unknownRefs.InfluencedBy(httpResponse);
CxList references = unknownRefs.FindAllReferences(httpReferences).GetMembersOfTarget();
result.Add(references.FindByShortNames(csp));

CxList referencesSetitem = references.FindByShortName("setitem");
CxList cspLiteral = Find_Strings().FindByShortNames(csp);
CxList cspAsVariable = cspLiteral.GetParameters(referencesSetitem, 0);
CxList cspAsParameter = All.GetParameters(referencesSetitem, 0).DataInfluencedBy(cspLiteral);

CxList influenced = All.NewCxList();
influenced.Add(cspAsVariable);
influenced.Add(cspAsParameter);

result.Add(references.DataInfluencedBy(influenced));