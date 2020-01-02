CxList methods = Find_Methods();

CxList connections = methods.FindByName("*sdb.sql.connect");
CxList prepared = methods.FindByShortName("prepare").DataInfluencedBy(connections);
CxList execute = methods.FindByShortName("execute").DataInfluencedBy(prepared.GetTargetOfMembers());

result = All.GetParameters(execute);