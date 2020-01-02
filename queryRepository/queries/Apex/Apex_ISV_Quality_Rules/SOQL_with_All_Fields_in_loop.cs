CxList methods = Find_Methods();
CxList fieldsGetMap = methods.FindByMemberAccess("fields.getMap", false);
CxList foreachstmt = All.FindByType(typeof(ForEachStmt));
CxList mapRefs = All.FindByType(typeof(UnknownReference)).FindByType("Map");

CxList fields = mapRefs.FindByFathers(foreachstmt).DataInfluencedBy(fieldsGetMap);

CxList tempField = All.FindByShortName("getDescribe", false).GetTargetOfMembers().FindByType("*.SObjectField").GetByAncs(foreachstmt);

CxList describe = tempField.GetMembersOfTarget().DataInfluencedBy(fields);

result += All.FindByMemberAccess("Database.query", false).DataInfluencedBy(describe).DataInfluencedBy(fieldsGetMap);