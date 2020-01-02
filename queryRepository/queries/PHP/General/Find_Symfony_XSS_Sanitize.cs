if(Find_Twig().Count > 0)
{	
	CxList foundVulnerableOutput = All.NewCxList();
	CxList ur = All.FindByType(typeof(UnknownReference));
	CxList mie = Find_Methods();
	CxList sl = All.FindByType(typeof(StringLiteral));
	CxList ifStmt = All.FindByType(typeof(IfStmt));
	CxList paramD = All.FindByType(typeof(Param));

	CxList twigRef = ur.FindByShortName("twig");
	CxList removeExt = All.FindByShortName("escaper").GetParameters(twigRef.GetMembersOfTarget().FindByShortName("removeExtenstion"));
	CxList twigExtEscaper = All.FindByShortName("false").GetParameters(All.FindByType(typeof(ObjectCreateExpr)).FindByShortName("Twig_Extension_Escaper")) + removeExt;



	CxList autoEscape = mie.FindByShortName("Equals").GetByAncs(ifStmt.FindByFileName("*.twig"));
	CxList autoEscapeType = sl.GetParameters(autoEscape);
	CxList notOkEscape = autoEscapeType.FindByShortName("false").GetAncOfType(typeof(IfStmt));
	autoEscapeType -= autoEscapeType.FindByShortName("false");
	CxList okAutoEscape = autoEscapeType.GetAncOfType(typeof(IfStmt));

	CxList outputCXCall = mie.FindByShortName("CX_twig_out");


	CxList notSByIf = outputCXCall - outputCXCall.GetByAncs(outputCXCall.GetAncOfType(typeof(IfStmt)));
	CxList temp = ur.FindByShortName("raw").GetByAncs(ifStmt.FindByFileName("*.twig"));
	notOkEscape.Add(temp.GetAncOfType(typeof(IfStmt)));


	CxList twigForeachDeclarator = All.FindByType(typeof(ForEachStmt)).FindByFileName("*html.twig");
	CxList newForeachRun = ur.FindByFathers(twigForeachDeclarator);
	CxList require = sl.GetParameters(mie.FindByShortName("require"));
	require = require.FindByShortName("*.twig");

	CxList returnRender = mie.FindByShortName("render*");
	CxList array = mie.FindByShortName("array").GetParameters(returnRender);

	array.Add(mie.FindByShortName("array").GetByAncs(All.FindByType(typeof(ReturnStmt))));


	CxList toPassToView = sl.GetParameters(array);
	CxList mieRaw = mie.FindByShortName("raw");
	CxList mieEsc = mie.FindByShortName("escape");
	
	foreach(CxList val in toPassToView)
	{
		CxList method = val.GetAncOfType(typeof(MethodDecl));
		CxList req = require.GetByAncs(method);	
		foreach(CxList viewFile in req)
		{		
			StringLiteral curFile = viewFile.data.GetByIndex(0) as StringLiteral;		
			if(curFile != null)
			{			
				CxList curOut = All.NewCxList();
				string name = curFile.ShortName.Replace("\"", "");				
				CxList output = outputCXCall.FindByFileName(name);										
				if(twigExtEscaper.Count > 0)
				{								
					curOut.Add(notSByIf * output);				
				}else{
					foreach(CxList tempOut in notSByIf)
					{
						if((mieRaw.GetByAncs(tempOut)).Count > 0){
							curOut.Add(tempOut);
						}
					}
				
				}
				CxList outputOK = output.GetByAncs(output.GetAncOfType(typeof(IfStmt)) * okAutoEscape);
			
				foreach(CxList echo in outputOK){				
					if(mieRaw.GetByAncs(echo).Count > 0)
					{								
						curOut.Add(echo);
					}
				}			
				outputOK = output.GetByAncs(output.GetAncOfType(typeof(IfStmt)) * notOkEscape);			
				foreach(CxList echo in outputOK){
					if(mieEsc.GetByAncs(echo).Count == 0)
					{					
						curOut.Add(echo);
					}
				}									
				CxList curParam = paramD.FindByFileName(name).GetByAncs(curOut);										
				CSharpGraph passedVal = val.data.GetByIndex(0) as CSharpGraph;			
				string CallName = passedVal.ShortName.Replace("\"", "");											
				if(curParam.FindByShortName(CallName).Count > 0)
				{	
				
					foundVulnerableOutput.Add(val);
				}			
				CxList baseRef = newForeachRun.DataInfluencingOn(curOut);						
				foundVulnerableOutput.Add(val.FindByShortName(baseRef));
			}
		}	
	}
	CxList io = Find_Symfony_Interactive_Outputs(All.NewCxList());
	CxList tempResult = io - foundVulnerableOutput;
	CxList arrayTop = tempResult.GetAncOfType(typeof(MethodInvokeExpr)).FindByShortName("array");
	CxList arrayParams = All.GetParameters(arrayTop).FindByType(typeof(Param));

	for(int i = 0; i < 20; i += 2)
	{
		CxList a = sl.GetParameters(arrayTop, i);	
		if((tempResult * a).Count > 0)
		{
			CxList b = arrayTop.FindByParameters(a);	
			CxList c = All.GetParameters(b, i + 1) - paramD;
		/*CxList c=All.GetByAncs(All.GetParameters(b, i + 1));	
		c -= c.DataInfluencingOn(c);
		c =c.DataInfluencingOn(arrayTop);*/
			result.Add(c);
		}
	}
}