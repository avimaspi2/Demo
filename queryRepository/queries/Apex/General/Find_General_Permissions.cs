/*
(1) Account.fields.Name.getDescribe().isUpdateable()
(2) Account.fields.getMap().get('Name').getDescribe().isUpdateable()

(3) Schema.getGlobalDescribe.get('Account').getDescribe().fields.Name.isUpdateable()
(4) Schema.getGlobalDescribe.get('Account').getDescribe().fields.getMap().get('Name').getDescribe().isUpdateable()

(5) Schema.sObjectType.Account.Name.isUpdateable()
(6) Schema.sObjectType.Account.getMap().get('Name').getDescribe().isUpdateable()
(7) Schema.sObjectType.Account.fields.Name.isUpdateable()
(8) Schema.sObjectType.Account.fields.getMap().get('Name').getDescribe().isUpdateable()
*/

CxList apexFiles = Find_Apex_Files() -Find_Triggers_Code() - Find_Test_Code();
CxList upd = apexFiles.FindByShortName("isUpdateable", false);
CxList crt = apexFiles.FindByShortName("isCreateable", false);
CxList acc = apexFiles.FindByShortName("isAccessible", false);
CxList methods = apexFiles.GetMethod(crt + upd + acc);
CxList apexData = apexFiles.GetByAncs(methods);
apexData = apexData.FindByType(typeof(MethodInvokeExpr)) + apexData.FindByType(typeof(UnknownReference));

CxList schema = apexData.FindByShortName("Schema", false);
CxList get1 = schema
	.GetMembersOfTarget().FindByShortName("getGlobalDescribe", false)
	.GetMembersOfTarget().FindByShortName("get", false);
CxList schemaFields = get1
	.GetMembersOfTarget().FindByShortName("getDescribe", false)
	.GetMembersOfTarget().FindByShortName("fields", false)
	;
CxList schemaObj = schema
	.GetMembersOfTarget().FindByShortName("SObjectType", false)
	.GetMembersOfTarget();
	
CxList one = apexData
	.GetMembersOfTarget().FindByShortName("fields", false)
	.GetMembersOfTarget()
	.GetMembersOfTarget().FindByShortName("getDescribe", false)
	.GetMembersOfTarget()
	;
CxList get2 = apexData
	.GetMembersOfTarget().FindByShortName("fields", false)
	.GetMembersOfTarget().FindByShortName("getMap", false)
	.GetMembersOfTarget().FindByShortName("get", false);
CxList two = get2
	.GetMembersOfTarget().FindByShortName("getDescribe", false)
	.GetMembersOfTarget()
	;
CxList three = schemaFields
	.GetMembersOfTarget()
	.GetMembersOfTarget()
	;
CxList get3 = schemaFields
	.GetMembersOfTarget().FindByShortName("getMap", false)
	.GetMembersOfTarget().FindByShortName("get", false);
CxList four = get3
	.GetMembersOfTarget().FindByShortName("getDescribe", false)
	.GetMembersOfTarget()
	;
CxList five = schemaObj
	.GetMembersOfTarget()
	.GetMembersOfTarget()
	;
CxList get4 = schemaObj
	.GetMembersOfTarget().FindByShortName("getMap", false)
	.GetMembersOfTarget().FindByShortName("get", false);
CxList six = get4
	.GetMembersOfTarget().FindByShortName("getDescribe", false)
	.GetMembersOfTarget()
	;
CxList seven = schemaObj
	.GetMembersOfTarget().FindByShortName("fields", false)
	.GetMembersOfTarget()
	.GetMembersOfTarget()
	;
CxList eight = schemaObj
	.GetMembersOfTarget().FindByShortName("fields", false)
	.GetMembersOfTarget().FindByShortName("getMap", false)
	.GetMembersOfTarget().FindByShortName("get", false)
	.GetMembersOfTarget().FindByShortName("getDescribe", false)
	.GetMembersOfTarget()
	;

result = one + two;
result.Add(three + four);
result.Add(five + six);
result.Add(seven + eight);
result = result.FindByShortName("isUpdateable", false)
	+ result.FindByShortName("isCreateable", false)
	+ result.FindByShortName("isAccessible", false);