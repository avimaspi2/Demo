/* This query gets Route Class Decl DOM-Object, and returns validate object  */
if(param.Length == 2)
{
	try
	{
		CxList routeClassDecl = param[0] as CxList;
		CxList validate = param[1] as CxList;
		if(routeClassDecl != null && validate != null)
		{
			CxList configClassDecl = Hapi_Find_Config_Under_Route(routeClassDecl);
			CxList allUnderConfigClassDecl = validate.GetByAncs(configClassDecl);
			result = allUnderConfigClassDecl;
		}
	}
	catch(Exception ex)
	{
		cxLog.WriteDebugMessage(ex);
	}
}