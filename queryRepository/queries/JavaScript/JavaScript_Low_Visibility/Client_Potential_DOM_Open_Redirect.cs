CxList inputs = Find_Potential_Inputs();

CxList redirect = Find_Outputs_Redirection();
CxList redirectMethods = Find_Outputs_Redirection_Methods();

//This code remove redirect to the same URL, like: 'document.location.href = document.location.href;'
CxList assignExpr = Find_Assignments();
assignExpr = assignExpr * (redirect.GetFathers() * inputs.GetFathers());

CxList removeFromInput = All.NewCxList();
foreach (CxList curAssign in assignExpr)
{
	AssignExpr assgn = curAssign.TryGetCSharpGraph<AssignExpr>();

	if (assgn != null && assgn.Left != null && assgn.Right != null)
	{ 
		if (assgn.Left.FullName == assgn.Right.FullName)
		{
			removeFromInput.Add(redirect.GetByAncs(curAssign));
		}
	}
}

redirect -= removeFromInput;

CxList redirectMethodsAll = All.NewCxList();
redirectMethodsAll.Add(redirect);
redirectMethodsAll.Add(redirectMethods);

result = redirectMethodsAll.DataInfluencedBy(inputs, CxList.InfluenceAlgorithmCalculation.NewAlgorithm);
result.Add(redirectMethods.FindByType(typeof(Param)) * inputs);
result = result.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);