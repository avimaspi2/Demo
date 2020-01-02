bool validatedParameters = false;
CxList interfaceMethods = null;
CxList interfaceParameters = null;
if (param.Length > 0)
{
	try
	{
		interfaceMethods = param[0] as CxList;
		validatedParameters = interfaceMethods != null;
		//cxLog.WriteDebugMessage("interfaceMethods is not null: " + interfaceMethods.Count);	
		if (validatedParameters && param.Length > 1)
		{
			interfaceParameters = param[1] as CxList;
			//cxLog.WriteDebugMessage("interfaceParameters is not null: " + interfaceParameters.Count);
			validatedParameters = interfaceParameters != null;
		}
		else
		{
			interfaceParameters = All.NewCxList();
		}
	}
	catch (Exception e)
	{
		cxLog.WriteDebugMessage(e);
	}
}

if (validatedParameters)
{
	cxLog.WriteDebugMessage("validatedParameters is true");
	CxList classes = Find_Class_Decl();
	CxList paramDecl = Find_ParamDeclaration();	
	CxList interfaces = Find_Interfaces();
	CxList typeRef = Find_TypeRef();
	CxList customAttribue = Find_CustomAttribute();
	CxList overrideAttribute = customAttribue.FindByCustomAttribute("Override");
	CxList methodDecl = Find_MethodDeclaration().FindByFieldAttributes(Modifiers.Public | Modifiers.Protected);
	CxList overrideMethods = methodDecl.GetMethod(overrideAttribute);
	
	CxList relevantInterfaces = interfaces.GetClass(interfaceMethods);
	relevantInterfaces.Add(interfaces.GetClass(interfaceParameters));
	CxList interfaceParameterMethods = methodDecl.GetMethod(interfaceParameters) - interfaceMethods;

	//result.Add(relevantInterfaces);
	foreach(CxList singleInterface in relevantInterfaces)
	{
		CxList singleInterfaceMethods = methodDecl.GetByAncs(singleInterface);
		CxList relevantMethods = interfaceMethods * singleInterfaceMethods;
		CxList relevantParameterMethods = interfaceParameterMethods * singleInterfaceMethods;
		CxList relevantParameters = paramDecl.GetParameters(relevantMethods);
		relevantParameters.Add(interfaceParameters.GetParameters(relevantParameterMethods));

		CxList relevantClasses = classes.InheritsFrom(singleInterface);
		CxList classMethods = overrideMethods.GetByAncs(relevantClasses);
		
		foreach (CxList singleParam in relevantParameters)
		{
			CxList method = singleInterfaceMethods.FindByParameters(singleParam);
			CxList potentialOverride = classMethods.FindByShortName(method);
			CxList type = typeRef.GetByAncs(singleParam);
			int index = singleParam.GetIndexOfParameter();
			if(potentialOverride.Count == 0 || index < 0)
			{
				continue;
			}
			foreach(CxList overrideMethod in potentialOverride)
			{
				if(paramDecl.GetParameters(overrideMethod).Count != paramDecl.GetParameters(method).Count){
					continue;
				}
				CxList relevantParameter = paramDecl.GetParameters(overrideMethod, index);
				if(relevantParameter.Count > 0)
				{
					if(typeRef.GetByAncs(relevantParameter).GetName().Equals(type.GetName()))
					{
						result.Add(relevantParameter);
					}
				}             
			}                 
		}
	}
}