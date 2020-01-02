/* This query gets Routes AssociativeArrayExpr and returns it's handlers */
if(param.Length > 0)
{
	try
	{
		CxList routes = param[0] as CxList;
	
		if(routes != null)
		{
			CxList handlers = All.FindByShortName("handler");
			result = handlers.GetByAncs(routes);
		}
	}
	catch(Exception ex)
	{
		cxLog.WriteDebugMessage(ex);
	}
}