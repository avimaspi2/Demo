if (param.Length == 1)
{
	CxList member = param[0] as CxList;
	member.Add(member.GetMembersOfTarget());
	member.Add(member.GetMembersOfTarget());
	member.Add(member.GetMembersOfTarget());
	member.Add(member.GetMembersOfTarget());
	member.Add(member.GetMembersOfTarget());
	member.Add(member.GetMembersOfTarget());
	result = member;
}