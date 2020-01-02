CxList methods = Find_Methods();

CxList dbDef = methods.FindByMemberAccess("PGconn.connect");

dbDef = dbDef.GetTargetOfMembers();
CxList dbAll = All * All.DataInfluencedBy(dbDef);

result = dbAll.FindByShortName("exec");

result.Add(Add_Second_Order_DB(result, new string[] {"exec"}));