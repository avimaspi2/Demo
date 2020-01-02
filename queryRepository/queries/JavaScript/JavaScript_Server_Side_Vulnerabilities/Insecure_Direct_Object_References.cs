/// <summary>
///  This query serches for session parameters taten from request params instead of the session paramters.
/// </summary>
CxList inputs = NodeJS_Find_Interactive_Inputs();
CxList session = Find_Methods().FindByShortName("session");

CxList target = All.NewCxList();
CxList SourcesList = All.NewCxList();
CxList inputParams = All.NewCxList();
// for each input - check if there is a session parameter assigned a value from input.params.
foreach(CxList input in inputs)
{
	target = input.GetTargetOfMembers();
	while (target.Count > 0)
	{
		inputParams = target.FindByShortName("params");
		if (inputParams.Count > 0)
		{
			SourcesList.Add(input.ConcatenateAllSources(target));
			break;	// the input is from params - add to source list and skip to the next input
		}
		target = target.GetTargetOfMembers();
	}
}
result.Add(SourcesList.DataInfluencingOn(session));