CxList render = All.FindByShortName("render");
render -= render.FindAllReferences(All.FindDefinition(render));
render -= render.GetTargetOfMembers().GetMembersOfTarget();

CxList dyna = Find_Methods() + All.FindByType(typeof(UnknownReference));
dyna -= Find_Inputs();
dyna = dyna.GetByAncs(render);

CxList sanitize = All.GetByAncs(render).FindByType(typeof(AssignExpr));
sanitize = All.GetByAncs(sanitize);

result = dyna.InfluencingOnAndNotSanitized(render, sanitize);
result -= result.DataInfluencedBy(result);
result -= result - All.GetByAncs(result);