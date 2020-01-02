CxList unknownRef = Find_UnknownReference();
CxList jQuery = unknownRef.FindByShortNames(new List<string> {"$", "jQuery"});
result = jQuery.GetMembersOfTarget().FindByShortName("sap");
CxList sap = unknownRef.FindByShortName("sap");
result.Add(sap.GetMembersOfTarget());
result.Add(All.FindAllReferences(result.GetAssignee()));
result.Add(All.FindByTypes(new string[] {"jQuery.sap*", "sap.*"}));