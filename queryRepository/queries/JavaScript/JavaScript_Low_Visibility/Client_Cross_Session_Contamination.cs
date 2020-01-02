//cross session contamination
//trasfer data from sessionStorage to localStorage

CxList outputs = Find_Storage_Outputs();
CxList session = outputs.GetTargetOfMembers().FindByShortName("sessionStorage");
CxList storage = Find_Storage_Inputs();
CxList sessionStorage = storage.GetTargetOfMembers().FindByShortName("sessionStorage").GetMembersOfTarget();
CxList inputs = storage - sessionStorage;

result = inputs.DataInfluencedBy(session);