CxList getSetParams = All.NewCxList();
Dictionary<string,string> existUnknownRef = new Dictionary<string,string>();

if (param.Length == 2)
{
	getSetParams = param[0] as CxList;
	existUnknownRef = param[1] as Dictionary<string,string>;

	CxList paramsAsUnknownReference = getSetParams.FindByType(typeof(UnknownReference));

	foreach(CxList oneParam in paramsAsUnknownReference)
	{
		string unknownRefName = oneParam.GetName();
		string declName = string.Empty;
		if (unknownRefName != "") // exists first parameter as unknown reference 
		{
			CxList declUnknownRef = All.FindDefinition(oneParam);               
			Checkmarx.Dom.Declarator decl = declUnknownRef.TryGetCSharpGraph<Checkmarx.Dom.Declarator>(); 
			if (decl != null)
			{
				Checkmarx.Dom.Expression initExpr = decl.InitExpression; 
				if (initExpr != null)
				{
					declName = initExpr.Text;
					if (declName.Length > 2)
					{
						// remove "
						declName = declName.Substring(1, declName.Length - 2);
						existUnknownRef[unknownRefName] = declName;
					}
				}
			}
		}
	}
}
else
{
	cxLog.WriteDebugMessage("Error. Expecting 2 parameters.");
}