result = All.FindByShortNames(new List<string>(){"res","resp","*response"}).FindByType(typeof(UnknownReference));

//NodeJS express HTTP responses
CxList callbacks = NodeJS_Find_Express_HTTP_Callbacks();

result.Add(All.FindAllReferences(All.GetParameters(callbacks, 1)));
result.Add(result.GetRightmostMember());

CxList unknownRefs = Find_UnknownReference();
CxList cxObjectParams = result.FindByShortName("CxObjectParam*");
CxList assignedRefs = unknownRefs.FindAllReferences(cxObjectParams.GetAssignee());
result.Add(assignedRefs);
result.Add(assignedRefs.GetRightmostMember());