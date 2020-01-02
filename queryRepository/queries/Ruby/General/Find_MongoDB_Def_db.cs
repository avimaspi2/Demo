CxList dbDef = All.FindByName("*Mongo.Connection");
dbDef.Add(All.FindByName("*Driver.Mongo"));
dbDef.Add(All.FindByShortName("MongoClient"));

//dbDef -= dbDef.FindByMemberAccess("*.db");
dbDef = dbDef.FindByType(typeof(ObjectCreateExpr)) + 
	dbDef.FindByType(typeof(MemberAccess));

dbDef = All.GetByAncs(dbDef.GetAncOfType(typeof(AssignExpr))).FindByAssignmentSide(CxList.AssignmentSide.Left);

result = All.FindAllReferences(dbDef);
//result -= result.FindByMemberAccess("*.db");