// In this query we find uses of external code without verification at all, which can lead to buffer overflow.

CxList inputs = Find_Interactive_Inputs();
CxList methods = Find_Methods();

// Find "length" methods
CxList len = methods.FindByShortName("length");

// Find all methodswith "native" attribute - calls to external methods
CxList externalMethodCalls = All.FindByFieldAttributes(Modifiers.Extern).FindByType(typeof(MethodDecl));
externalMethodCalls = methods.FindAllReferences(externalMethodCalls);
// Find their parameters, and remove length
CxList externalMethodParams = All.GetParameters(externalMethodCalls);
externalMethodParams = All.GetByAncs(externalMethodParams);
CxList paramsLen = externalMethodParams * len;
paramsLen.Add(paramsLen.GetTargetOfMembers());
paramsLen.Add(paramsLen.GetTargetOfMembers());
externalMethodParams -= paramsLen;

// Look only at the parameters that do not have an "if" check which checks their content
CxList potentialProblems = externalMethodParams;
// Loop on all parameters
foreach (CxList methodParam in externalMethodParams)
{
	try
	{
		// Look at the relevant "if" statement
		CxList ifStmt = methodParam.GetAncOfType(typeof(IfStmt));
		if (ifStmt.Count > 0)
		{	// If there is such an "if" statement
			IfStmt stmt = ifStmt.data.GetByIndex(0) as IfStmt;
			
			//See if the condition includes a reference to the parameter we are checking
			CxList ifCondition = All.FindById(stmt.Condition.NodeId);
			ifCondition = All.GetByAncs(ifCondition);
			if (ifCondition.FindAllReferences(methodParam).Count > 0)
			{
				// If so remove it from the potential list
				potentialProblems -= methodParam;
			}
		}
	}
	catch(Exception ex)
	{
		// Just in case we get an unexpected exception
	}
}

// Parameters that are affected by the input
result = potentialProblems.DataInfluencedBy(inputs);