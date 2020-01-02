// This query looks for castings of a specific type

if (param.Length == 2)
{
	CxList base_list = param[0] as CxList;
	string type = param[1] as string;
	CxList allCastExpr = base_list.FindByType(typeof(CastExpr));
	CxList castings = All.NewCxList();

	foreach(CxList ace in allCastExpr)
	{
		try
		{
			CastExpr ce = ace.TryGetCSharpGraph<CastExpr>();

			if(ce != null)
			{
				string typeName = ce.TargetType.TypeName; 
				if(typeName.Equals(type))
				{
					castings.Add(ace);
				}
			}
		}
		catch(Exception ex)
		{
			cxLog.WriteDebugMessage(ex);
		}
	}
	result = castings;
}