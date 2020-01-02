CxList sap = Find_SAP_Library().FindByShortName("sap");
CxList sapStorage = sap.GetMembersOfTarget().FindByShortName("storage");
result.Add(sapStorage);
CxList assigned = sapStorage.GetAssignee();
result.Add(All.FindAllReferences(assigned));