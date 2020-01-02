if (param.Length == 1)
{
	CxList classes = param[0] as CxList;
	CxList interfaces = Find_InterfaceDecl();
	
	foreach(CxList cl in classes)
	{
		try 
		{
			ClassDecl gr = cl.TryGetCSharpGraph<ClassDecl>();
			result.Add(interfaces.FindByShortName("i" + gr.Name));
		}
		catch(Exception e)
		{
			cxLog.WriteDebugMessage(e.Message);
		}
	}
}