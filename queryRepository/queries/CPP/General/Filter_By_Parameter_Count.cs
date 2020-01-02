/*
	Filter_By_Parameter_Count
		elementList - CxList containg the MethodInvokeExprs or ObjectCreateExpr to filter
        paramNumber - the number of parameters (int)
*/
if (param.Length == 2)
{
	CxList elementList = param[0] as CxList;
	int? paramNumber = (int) param[1];
        
	foreach(CxList element in elementList)
	{
		MethodInvokeExpr method = element.TryGetCSharpGraph<MethodInvokeExpr>();
		if(method != null)
		{ 
			if(method.Parameters.Count == paramNumber)
			{
				result.Add(element);
			}
		}
		else
		{
			ObjectCreateExpr objCreate = element.TryGetCSharpGraph<ObjectCreateExpr>();
			if(objCreate != null)
			{ 
				if(objCreate.Parameters.Count == paramNumber)
				{
					result.Add(element);
				}
			}
		}
		
	}
}
else
{
	cxLog.WriteDebugMessage("Number of parameters should be 2");          
}