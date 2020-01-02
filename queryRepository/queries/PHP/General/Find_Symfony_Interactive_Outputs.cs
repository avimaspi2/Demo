if(Find_Twig().Count > 0)
{	
	CxList allParam = Find_Params();
	CxList tempResult = All.NewCxList();
	CxList mie = Find_Methods();
	CxList sl = Find_Strings();
	CxList ur = Find_UnknownReferences();
	
	CxList twigForeachDeclarator = All.FindByType(typeof(ForEachStmt)).FindByFileName("*html.twig");
	CxList newForeachRun = ur.FindByFathers(twigForeachDeclarator);
	CxList require = sl.GetParameters(mie.FindByShortName("require"));
	require = require.FindByShortName("*.twig");

	CxList returnRender = mie.FindByShortName("render*");
	CxList array = mie.FindByShortName("array").GetParameters(returnRender);
	array.Add(mie.FindByShortName("array").GetByAncs(All.FindByType(typeof(ReturnStmt))));

	CxList toPassToView = sl.GetParameters(array);

/*
go over every controller left side of array param variable of render statement
*/

	CxList cxTWIG = mie.FindByShortName("CX_twig_out");
	foreach(CxList val in toPassToView)
	{
		CxList method = val.GetAncOfType(typeof(MethodDecl));
		CxList req = require.GetByAncs(method);	
		//go over all required twig file for current method.
		foreach(CxList viewFile in req)
		{
			StringLiteral curFile = viewFile.data.GetByIndex(0) as StringLiteral;
			if(curFile != null)
			{
				string name = curFile.ShortName.Replace("\"", "");				
				CxList output = cxTWIG.FindByFileName(name);						
				CxList curParam = allParam.FindByFileName(name).GetByAncs(output);
				CSharpGraph passedVal = val.data.GetByIndex(0) as CSharpGraph;
				string CallName = passedVal.ShortName.Replace("\"", "");			
				if(curParam.FindByShortName(CallName).Count > 0)
				{
					tempResult.Add(val);
				}			
				CxList baseRef = newForeachRun.DataInfluencingOn(output);				
				tempResult.Add(val.FindByShortName(baseRef));
			}
		}
	}
	//if method call is from sanitize then return am intermediate result
	if(param.Length > 0)
	{
		result = tempResult;
	}
	else
	{
		CxList arrayTop = (tempResult).GetAncOfType(typeof(MethodInvokeExpr)).FindByShortName("array");
		CxList arrayParams = All.GetParameters(arrayTop).FindByType(typeof(Param));
		for(int i = 0; i < 20; i += 2)
		{
			CxList a = sl.GetParameters(arrayTop, i);	
			if((tempResult * a).Count > 0)
			{		
				CxList b = arrayTop.FindByParameters(a);	
				CxList c = All.GetParameters(b, i + 1) - allParam;
				result.Add(c);
			}
		}
		
		//Only the outputs that are passed to the view in {val | raw} are vulnerables
		CxList taintedInTwig = ur.GetParameters(mie.FindByShortName("raw"));
		CxList tainted = All.NewCxList();
		foreach(CxList sani in taintedInTwig){
			UnknownReference urInRaw = sani.data.GetByIndex(0) as UnknownReference;
			tainted.Add(ur.FindByShortName(urInRaw.VariableName));
		}
		tainted = tainted - taintedInTwig;
	
		//now handle output of form return new Response('wellcome'. $someName);
		CxList response = Find_ObjectCreation().FindByShortName("Response");
		CxList response_with_renders = returnRender.GetAncOfType(typeof(ObjectCreateExpr));
		response = response - response_with_renders;
		response_with_renders = ur.GetByAncs(response_with_renders);
		result.Add(ur.GetByAncs(response));
		result.Add(response_with_renders * tainted);
	}
}