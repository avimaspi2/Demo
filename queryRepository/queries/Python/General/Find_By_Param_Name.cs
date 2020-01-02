if (param.Length >= 1)
{
	string name = param[0] as string;
	string value_name = param.Length >= 2 ? param[1] as string : null;
	string method_name = param.Length == 3 ? param[2] as string : null;
      
	CxList args = Find_Param();
	CxList final = All.NewCxList();
      
	if (method_name != null)
	{
		CxList methods = All.FindByShortName(method_name);
		args = args.GetByAncs(methods);
	}
	if (value_name != null)
	{
		args = args.FindByShortName(value_name);
	}
      
	foreach(CxList p in args)
	{
		try 
		{
			Param p1 = p.TryGetCSharpGraph<Param>();                 
			if (p1 != null && p1.Name != null && p1.Name.Equals(name))
			{
				final.Add(p);
			}
		}
		catch(Exception e)
		{
			cxLog.WriteDebugMessage(e);
		}
	}
      
	result = All.GetByAncs(final) - final;
}