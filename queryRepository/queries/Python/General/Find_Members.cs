//Finds the members of the first parameter string. 
//The optional second parameter is the group from which to search for the results.
//In addition, the query searches for handles of the first object, and finds their member access.
if (param.Length >= 1)
{	
	string template = param[0] as string;
	CxList source = All.NewCxList(); 
	if (param.Length >= 2) 
	{
		source = param[1] as CxList;
	}else
	{
		source = All.Clone();
	}
	string[] splitTemplate = template.Split('.');
	if (splitTemplate[0].Equals("*"))
	{
		result = source.FindByShortName(splitTemplate[1]).GetMembersWithTargets();
	}
	else
	{
		result = source.FindByName("*" + template.TrimStart('*'));
		if (splitTemplate.Length >= 2 && !template.Contains("*"))
		{
			//Find relevant members and targets according to the parameter.
			CxList members = source.FindByShortName(splitTemplate[1]);
			CxList targets = All.FindByShortName(splitTemplate[0]);
			targets -= (targets.GetMembersOfTarget().GetTargetOfMembers());
			//Find relevant references linked to the members and the targets.
			CxList refs = All.FindAllReferences(members.GetTargetOfMembers());
			CxList newRefs = refs.DataInfluencedBy(targets);
			refs = refs.FindAllReferences(newRefs);
			result.Add(refs.GetMembersOfTarget() * members);
		}
	}
	result.Add(source.FindByMemberAccess(template));
}