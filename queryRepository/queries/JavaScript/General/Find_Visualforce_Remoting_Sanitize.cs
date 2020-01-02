//for APEX
/*
 All outputs that appear under visualforce invoke action callback function 
 in case escape is mapped to true, those outputs will be added to the sanitizer of an XSS

*/
CxList methods = Find_Methods();
CxList remotingManager = All.FindByName("Visualforce.remoting");
CxList remoting = remotingManager.GetFathers();
CxList unknownReferences = Find_UnknownReference();
CxList invokeAction = unknownReferences.FindAllReferences(remoting).GetMembersOfTarget().GetMembersOfTarget();
invokeAction = invokeAction.FindByName("*Manager.invokeAction");
invokeAction.Add(methods.FindByName("Visualforce.remoting.Manager.invokeAction"));
CxList anony = Find_LambdaExpr();
CxList sanitizedInvokeAction = All.NewCxList();
CxList constructor = Find_MethodDecls() * anony;
CxList escp = All.FindByShortName("escape");
CxList escapeBool = Find_BooleanLiteral().FindByShortName("true");
CxList escapeUnderConstructor = escp.GetByAncs(constructor);

foreach(CxList invA in invokeAction){
	CxList escape = anony.GetParameters(invA, 3);
	try{
		if(escape.Count == 0)
		{
			sanitizedInvokeAction.Add(invA);
			continue;
		}
		CxList relevantConstructors = All.NewCxList();
		foreach(CxList cbf in escape)
		{
			try{
				CSharpGraph g = cbf.GetFirstGraph();
				if(g != null && g.ShortName != null)
				{
					string anonymousName = g.ShortName;
					if(anonymousName.EndsWith("var")){			
						anonymousName = anonymousName.Remove(anonymousName.IndexOf("var"));
						int fileId = g.LinePragma.GetFileId();
			
						relevantConstructors.Add(constructor.FindByFileId(fileId).FindByShortName(anonymousName));
					}
				}
			}catch(Exception e)
			{
				cxLog.WriteDebugMessage(e);
			}
		}
		CxList escapeAssign = escapeUnderConstructor.GetByAncs(relevantConstructors);
		escapeAssign = escapeAssign.DataInfluencedBy(escapeBool.GetByAncs(relevantConstructors));
		if(escapeAssign.Count > 0)
		{
			sanitizedInvokeAction.Add(invA);
		}
	}catch(Exception e)
	{
		cxLog.WriteDebugMessage(e);
	}
}

CxList callbackFunction = anony.GetParameters(sanitizedInvokeAction, 2);


CxList relevantCtrs = All.NewCxList();
foreach(CxList cbf in callbackFunction)
{
	try{
		CSharpGraph g = cbf.GetFirstGraph();
		if(g != null && g.ShortName != null && g.LinePragma != null)
		{
			string anonymousName = g.ShortName;
			if(anonymousName.EndsWith("var")){
				anonymousName = anonymousName.Remove(anonymousName.IndexOf("var"));
				int fileId = g.LinePragma.GetFileId();
				relevantCtrs.Add(constructor.FindByFileId(fileId).FindByShortName(anonymousName));
			}
		}
	}catch(Exception e)
	{
		cxLog.WriteDebugMessage(e);
	}
}
CxList vulnerable = Find_Vulnerable_SetAttribute();
CxList back = All.FindByAssignmentSide(CxList.AssignmentSide.Right).GetByAncs(
	vulnerable.GetFathers());

CxList xssOutputs = Find_Outputs_XSS();
CxList RemoveFromXss = vulnerable;
RemoveFromXss.Add(xssOutputs.DataInfluencedBy(back));
xssOutputs -= RemoveFromXss;

/*- Find_XSS_Vulnerable_SetAttribute();*/
result = xssOutputs.GetByAncs(relevantCtrs);