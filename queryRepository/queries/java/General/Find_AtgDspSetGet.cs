CxList AllDsp = Get_AllDSP();  
CxList methodInvokes = Find_Methods();

// calculate "getAttribute" and "getParameter"
CxList getAttrib = methodInvokes.FindByShortNames(new List<string> {"getAttribute","getParameter"});

CxList getAttribFirstParam = All.GetParameters(getAttrib, 0);

// calculate "setAttribute" and "setParameter"
CxList setAttrib = methodInvokes.FindByShortNames(new List<string> {"setAttribute","setParameter"});


CxList setAttribFirstParam = All.GetParameters(setAttrib, 0);
CxList setAttribSecondParam = All.GetParameters(setAttrib, 1);


CxList classDecls = Find_Class_Decl();
CxList MethodDecls = Find_MethodDeclaration();

// Handle inputs parameters to droplet
CxList dropletDecls = classDecls.InheritsFrom("DynamoServlet");

CxList dropletServiceMethods = MethodDecls.GetByAncs(dropletDecls);
dropletServiceMethods = dropletServiceMethods.FindByShortName("service");
// Get side

CxList getInServiceMethods = getAttribFirstParam.GetByAncs(dropletServiceMethods);

Dictionary<string,string> existUnknownRef = new Dictionary<string,string>();

CxList allAttrib = All.NewCxList();
allAttrib.Add(setAttribFirstParam);
allAttrib.Add(getAttribFirstParam);

Find_VarConstStrMapping(allAttrib , existUnknownRef);

//======================================================
// let assume that set exists in param of droplet      |
//======================================================
CxList setAttribDsp = AllDsp * setAttrib;

// for each setAttribute
foreach (CxList setAttr in setAttribDsp)
{
	// search first If
	CxList beanInvoke = setAttr.GetAncOfType(typeof(IfStmt));
	CxList objectCreateExpr = All.NewCxList();
	try
	{
		Checkmarx.Dom.IfStmt ifStmt = beanInvoke.TryGetCSharpGraph<IfStmt>(); 
		if (ifStmt == null)
		{
			continue;
		}
                
		// search create droplet statement
		objectCreateExpr = AllDsp.GetByAncs(beanInvoke).FindByType(typeof(ObjectCreateExpr));
                
		if (objectCreateExpr.Count == 0)
		{
			continue;
		}
	}
	catch(Exception ex)
	{
		cxLog.WriteDebugMessage(ex);
		continue;
	}
	
	string className = objectCreateExpr.GetName();
                                
	CxList setAttrFirstParam = setAttribFirstParam.GetParameters(setAttr, 0);
	string firstSetParamName = setAttrFirstParam.GetName();

	// for each getAttribute parameter
	foreach (CxList getAttr in getInServiceMethods)
	{
		CxList classDef = getAttr.GetAncOfType(typeof(ClassDecl));
		if (classDef == null || classDef.Count == 0)
			continue;
                                
		// same class
		if (classDef.GetName() != className)
			continue;
                                
		string firstGetParamName = getAttr.GetName();
                                
		CxList getParameterInvokeFirstParamUnknowRef = getAttr.FindByType(typeof(UnknownReference));

		string unknownRefName = getParameterInvokeFirstParamUnknowRef.GetName();
		string declName = string.Empty;
		if (unknownRefName != "") // exists first parameter as unknown reference 
		{
			if (!existUnknownRef.TryGetValue(unknownRefName, out declName))
				declName = string.Empty;
		}
                
		// same name
		if ((firstSetParamName != firstGetParamName) &&
			(firstSetParamName != declName))
			continue;
                                
		CxList secParam = setAttribSecondParam.GetParameters(setAttr, 1);
		secParam -= secParam.FindByType(typeof(Param));

		CxList tmpResult;                        
		// Build the flow 
		// secondParam(setAttribute) -> firstParam(setAttribute) -> getParameter Invoke 
		if (secParam.data.GetByIndex(0).GetType() == typeof(Checkmarx.Dom.MethodInvokeExpr))
		{
			tmpResult = secParam.GetTargetOfMembers().ConcatenateAllPaths(secParam, false);
			tmpResult = tmpResult.ConcatenateAllPaths(setAttrFirstParam, false);
		}
		else
		{
			tmpResult = secParam.ConcatenateAllPaths(setAttrFirstParam, false);
		}
		tmpResult = tmpResult.ConcatenateAllPaths(getAttr, false);
		tmpResult = tmpResult.ConcatenateAllPaths(getAttr.GetFathers().GetAncOfType(typeof(MethodInvokeExpr)), false);
		result.Add(tmpResult);
	}
}

CxList getAttribDsp = AllDsp * getAttrib;

foreach (CxList attr in getAttribDsp)
{
	CxList beanInvoke = attr.GetAncOfType(typeof(IfStmt));
	Checkmarx.Dom.UnknownReference unkownRef = null;
	try
	{
		Checkmarx.Dom.IfStmt ifStmt = beanInvoke.TryGetCSharpGraph<IfStmt>(); 
		if (ifStmt == null)
		{
			continue;
		}
                
		Checkmarx.Dom.BinaryExpr binExpr = ifStmt.Condition as Checkmarx.Dom.BinaryExpr;
		if (binExpr == null)
		{
 		
			beanInvoke = beanInvoke.GetFathers().GetAncOfType(typeof(IfStmt));
			ifStmt = beanInvoke.TryGetCSharpGraph<IfStmt>(); 
			if (ifStmt == null)
			{
				continue;
			}
			binExpr = ifStmt.Condition as Checkmarx.Dom.BinaryExpr;
			if (binExpr == null)
			{
				continue;
			}
		}
	
		unkownRef = binExpr.Left as Checkmarx.Dom.UnknownReference;
		if (unkownRef == null)
		{
			continue;
		}
	}
	catch(Exception ex)
	{
		cxLog.WriteDebugMessage(ex);
		continue;
	}       

	CxList variable = AllDsp.FindByShortName(unkownRef.VariableName).FindByType(typeof(Declarator));
                
	CxList tmp = variable.GetAncOfType(typeof(VariableDeclStmt));

	CxList typeRef = AllDsp.GetByAncs(tmp).FindByType(typeof(TypeRef));

	// find class definition of requested droplet
	CxList dropletDef = classDecls.FindDefinition(typeRef);
                                
	// find "setParameter" MethodInvokeExpr that exists in droplet class implementation
	CxList classSpecificSetParam = setAttrib.GetByAncs(dropletDef);
                
	// find "getAttribute param name in jsp file
	CxList getAttribParam = AllDsp.GetParameters(attr);
	string attribName = getAttribParam.GetName();
                
	// foreach setParameter invoke
	foreach (CxList setParamInvoke in classSpecificSetParam)
	{
		// get first parameter of setParameter invoke
		CxList setParameterInvokeFirstParam = setAttribFirstParam.GetParameters(setParamInvoke, 0).FindByType(typeof(Param));
		CxList setParameterInvokeFirstParamUnknowRef = setAttribFirstParam.GetParameters(setParamInvoke, 0).FindByType(typeof(UnknownReference));
		string unknownRefName = setParameterInvokeFirstParamUnknowRef.GetName();
		string declName = string.Empty;
                                
		if (unknownRefName != "") // exists first parameter as unknown reference 
		{
			if (!existUnknownRef.TryGetValue(unknownRefName, out declName))
				declName = string.Empty;
		}
                                
		if ((attribName == setParameterInvokeFirstParam.GetName()) ||
			(attribName == declName))
		{
			// find second parameter of setParameter invoke
			CxList secParam = setAttribSecondParam.GetParameters(setParamInvoke, 1);
			secParam -= secParam.FindByType(typeof(Param));
                                                
			// Build the flow 
			// secondParam -> firstParam -> setParameter Invoke -> output

			CxList tmpResult;
			bool handleObjectWithMethod = false;
			CxList targetObj = All.NewCxList();
			targetObj.Add(secParam);
			// Build the flow 
			// secondParam(setAttribute) -> firstParam(setAttribute) -> getParameter Invoke
			if (secParam.data.GetByIndex(0).GetType() == typeof(Checkmarx.Dom.MethodInvokeExpr))
			{
				targetObj = secParam.GetTargetOfMembers();
				if (targetObj.Count > 0)
				{
					handleObjectWithMethod = true;
				}
			}
			if (handleObjectWithMethod)
			{
				tmpResult = targetObj.ConcatenateAllPaths(secParam, false);
				tmpResult = tmpResult.ConcatenateAllPaths(setParameterInvokeFirstParam, false);
			}
			else
			{
				tmpResult = secParam.ConcatenateAllPaths(setParameterInvokeFirstParam, false);
			}

			tmpResult = tmpResult.ConcatenateAllPaths(getAttribParam, false);
			tmpResult = tmpResult.ConcatenateAllPaths(getAttribParam.GetAncOfType(typeof(MethodInvokeExpr)), false);
			result.Add(tmpResult);
		}
	}
}