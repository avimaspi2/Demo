// $ASP
if (param.Length == 1)
{
	string prm = param[0] as string;
	result = All.FindByMemberAccess(prm.ToLower());
}