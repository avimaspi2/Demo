if (param.Length == 1)
{
	try
	{
		string name = param[0] as string;
		string[] nameMember = name.Split('.');
		result = Find_By_Constructor_Name(nameMember[0], nameMember[1]);
	}
	catch(Exception ex)
	{
		cxLog.WriteDebugMessage(ex);
	}
}
if (param.Length == 2)
{
	try
	{
		string sourceName = param[0] as string;
		string memberName = param[1] as string;		

		result = 
			All.FindByShortName(sourceName).GetMembersOfTarget()
			.FindByShortName("alloc").GetMembersOfTarget()
			.FindByShortName(memberName);
	}
	catch (Exception ex)
	{
		cxLog.WriteDebugMessage(ex);
	}
}