CxList render = All.FindByShortName("render");
render -= render.FindAllReferences(All.FindDefinition(render));
render -= render.GetTargetOfMembers().GetMembersOfTarget();
CxList inputs = Find_Interactive_Inputs() + Find_Read();
CxList db = Find_DB();

result = inputs.InfluencingOnAndNotSanitized(render, db);
result.Add(db.DataInfluencingOn(render));