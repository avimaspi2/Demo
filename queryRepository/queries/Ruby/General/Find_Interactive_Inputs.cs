CxList allParams = All.FindByType(typeof(UnknownReference));
allParams.Add(All.FindByType(typeof(MemberAccess)));
allParams.Add(All.FindByType(typeof(IndexerRef)));

allParams = allParams.FindByShortNames(new List<string>{"params", "cookies"});
allParams -= All.FindAllReferences(All.FindDefinition(allParams));

CxList paramsAssign = allParams.GetAncOfType(typeof(AssignExpr));
CxList leftAssign = All.GetByAncs(paramsAssign).FindByAssignmentSide(CxList.AssignmentSide.Left);
allParams -= allParams.GetByAncs(leftAssign);
allParams.Add(All.FindByShortName("__query_parameters"));

CxList TempResult = Find_Console_Inputs();
TempResult.Add(allParams + All.FindAllReferences(All.FindDefinition(allParams)));
	
CxList iterate = TempResult.FindByShortName("params");
result = TempResult - TempResult.GetMembersOfTarget().GetTargetOfMembers();

CxList target = All.NewCxList();
for (int i = 10; i > 0; i--)
{
	if (iterate.Count <= 0)
	{
		break;
	}
	target.Add(iterate);
	iterate = iterate.GetMembersOfTarget();
}
result.Add(target);