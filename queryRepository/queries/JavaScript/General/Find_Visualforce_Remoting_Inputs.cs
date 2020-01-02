//for APEX
/*
The query looks for all result of visualforce invokeAction's call back function and
maps it as an input

*/
CxList methods = Find_Methods();
CxList remotingManager = All.FindByName("Visualforce.remoting");
CxList remoting = remotingManager.GetFathers();
CxList unknownReferences = Find_UnknownReference();
CxList invokeAction = unknownReferences.FindAllReferences(remoting).GetMembersOfTarget().GetMembersOfTarget();
invokeAction = invokeAction.FindByName("*Manager.invokeAction");
invokeAction.Add(methods.FindByName("Visualforce.remoting.Manager.invokeAction"));

if(invokeAction.Count > 0)
{
	CxList callbackFunction = All.FindDefinition(All.GetParameters(invokeAction, 2));
	CxList constructor = Find_LambdaExpr();
	CxList relevantConstructors = All.NewCxList();
	
	foreach(CxList cbf in callbackFunction)
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
			if(cbf.FindByType(typeof(MethodDecl)).Count > 0)
			{
				relevantConstructors.Add(cbf);
			}
			
			
		}catch(Exception e)
		{
			cxLog.WriteDebugMessage(e);
		}
	}
	result.Add(All.GetParameters(relevantConstructors, 0));
}