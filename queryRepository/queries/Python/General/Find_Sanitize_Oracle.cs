CxList methods = Find_Methods();
CxList conn = methods.FindByName("*cx_Oracle.connect");

CxList prepare = methods.FindByShortName("prepare").DataInfluencedBy(conn);
CxList execute = methods.FindByShortName("execute").DataInfluencedBy(prepare.GetTargetOfMembers());
CxList sanitize = All.GetParameters(execute, 1);

result = sanitize;