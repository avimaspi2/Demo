if (param.Length == 1)
{
	CxList member = param[0] as CxList;
	member.Add(member.GetTargetOfMembers());
	member.Add(member.GetTargetOfMembers());
	member.Add(member.GetTargetOfMembers());
	member.Add(member.GetTargetOfMembers());
	member.Add(member.GetTargetOfMembers());
	member.Add(member.GetTargetOfMembers());
	result = member;
}