//this query Finds All JQuery methods, even if they are not marked by $.
//receives three parameters- first is the recursion starting point for each stage (Scope aliases and Original JQuery Methods)
//second is the new updated JQuery Methods list
//third is safe recursion counter

if(param.Length == 3)
{			
	CxList JQMethodsElements = Find_JQuery_Methods_Return_JQuery();
	CxList JQMethods = param[0] as CxList;//current recursion list
	CxList newJQMethodList = param[1] as CxList; //this is the  JQUERY  list that will be sent back
	int iterationsCount = 0;
	try 
	{
		iterationsCount = (int) param[2];
	}
	catch (Exception exc)
	{
		cxLog.WriteDebugMessage(exc);
	}
	iterationsCount++;
	if (newJQMethodList == null)
	{
		result = Find_JQuery_Methods();
	}
	else{
		if(JQMethods == null || JQMethods.Count == 0 || iterationsCount > 5)
		{
			result = newJQMethodList;
		}
		else{
			CxList nextStageJQueryMethods = All.NewCxList(); //this creates the next step for recursion
		
			//x=$() ||  x= $().attr()
			CxList invokes = Find_Methods();
			CxList MethodDecls = Find_MethodDecls();
		
			CxList paramDecl = Find_ParamDecl();
			CxList returnStmt = Find_ReturnStmt();
			
					
			CxList invocations = invokes.FindByParameters(JQMethods);
			CxList passedAsParamDecl = MethodDecls.FindByParameters(JQMethods);
			passedAsParamDecl = JQMethods.GetParameters(passedAsParamDecl);
			
			//look for left side of current marker
			CxList LeftHandSide = Find_Assign_Lefts().GetByAncs(JQMethods.GetFathers());	
			LeftHandSide.Add(passedAsParamDecl);
		    
			CxList passedJQueryParam = JQMethods.GetParameters(invocations).GetAncOfType(typeof(Param));
			if(passedJQueryParam.Count > 0)
			{
				foreach(CxList prm in passedJQueryParam)
				{
					int index = prm.GetIndexOfParameter();
					
					CxList curInvoke = invocations.FindByParameters(prm);
					LeftHandSide.Add(paramDecl.GetParameters(MethodDecls.FindAllReferences(curInvoke), index));
				}
			}
			//if reference is returned
			CxList locatedUnderReturn = JQMethods.FindByFathers(returnStmt);
			nextStageJQueryMethods.Add(invokes.FindAllReferences(locatedUnderReturn.GetAncOfType(typeof(MethodDecl))));
			
			if(LeftHandSide.Count > 0)
			{		
				CxList allLeftRefs = Find_References_After_Me_In_Method(LeftHandSide);	
				//first member will always be a JQuery method so we can add it to the result		
				//look for last member that is not first member
				CxList rightMost = allLeftRefs.GetRightmostMember();		
				//if last returns JQuery means all before return JQuery so we will add those. 
				CxList elements = rightMost * JQMethodsElements;
				newJQMethodList.Add(elements);
				CxList rm = elements.Clone();
				for(int i = 0; i < 5; i++)
				{
					CxList target = rm.GetTargetOfMembers();
					target = target.GetMembersWithTargets();
					
					newJQMethodList.Add(target);
					rm = target;
			
				}
				nextStageJQueryMethods.Add(elements);		
				
				//if reference is aliased
				CxList assignRights = Find_Assign_Rights();
				CxList imInRight = (allLeftRefs * assignRights).GetByAncs(allLeftRefs.GetFathers());
				nextStageJQueryMethods.Add(imInRight);	
				//if reference is Passed As Parameter		
				CxList methodOfParam = invokes.FindByParameters(allLeftRefs);
				CxList imAParam = allLeftRefs.GetParameters(methodOfParam).GetAncOfType(typeof(Param));	
				foreach(CxList prm in imAParam)
				{
					int index = prm.GetIndexOfParameter();	
					CxList curInvoke = methodOfParam.FindByParameters(prm);
					CxList pDecl = paramDecl.GetParameters(MethodDecls.FindAllReferences(curInvoke), index);
					nextStageJQueryMethods.Add(pDecl);
				}
				//if reference is returned
				CxList underReturn = allLeftRefs.FindByFathers(returnStmt);
				nextStageJQueryMethods.Add(invokes.FindAllReferences(underReturn.GetAncOfType(typeof(MethodDecl))));
			}	
			result = Find_All_JQuery_Methods_Including_Aliases(nextStageJQueryMethods, newJQMethodList, iterationsCount);
		}
	}
}