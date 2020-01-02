//NodeJS express HTTP requests
CxList unknownRefs = Find_UnknownReference();
CxList callbacks = NodeJS_Find_Express_HTTP_Callbacks();
result = unknownRefs.FindAllReferences(All.GetParameters(callbacks, 0));
result.Add(result.GetRightmostMember());

CxList cxObjectParams = result.FindByShortName("CxObjectParam*");
CxList assignedRefs = unknownRefs.FindAllReferences(cxObjectParams).GetRightmostMember().GetAssignee();
assignedRefs.Add(unknownRefs.FindAllReferences(assignedRefs));
assignedRefs.Add(assignedRefs.GetRightmostMember());
result.Add(assignedRefs);