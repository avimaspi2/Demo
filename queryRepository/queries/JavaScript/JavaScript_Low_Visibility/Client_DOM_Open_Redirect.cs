CxList inputs = Find_Inputs();
CxList redirect = Find_Outputs_Redirection();
CxList redirectMethods = Find_Outputs_Redirection_Methods();

List <string> membersOutput = new List<string> (new string[]{"href","URLUnencoded"});
List <string> membersInput = new List<string>(new string[]{"href","search","referrer","URL","documentURI",
	"baseURI","URLUnencoded"});

CxList thisReference = Find_ThisRef();
CxList documentThisRef = thisReference.FindByShortName("CxJSNS*");
CxList href = documentThisRef.GetMembersOfTarget();

inputs.Add(href.FindByShortNames(membersInput));
redirect.Add(href.FindByShortNames(membersOutput));


//This code remove redirect to the same URL, like: 'document.location.href = document.location.href;'
CxList assignExpr = Find_Assignments();
assignExpr = assignExpr * (redirect.GetAncOfType(typeof(AssignExpr)) * inputs.GetAncOfType(typeof(AssignExpr)));

CxList removeFromInput = All.NewCxList();
foreach (CxList curAssign in assignExpr)
{
	try
	{
		AssignExpr assgn = curAssign.TryGetCSharpGraph<AssignExpr>();

		if (assgn != null && assgn.Left != null && assgn.Right != null)
		{ 
			CSharpGraph t = assgn.Right;
			while( t != null && t is BinaryExpr)
			{
				t = ((BinaryExpr) t).Left;
			}
			if(t != null )
			{
				if (assgn.Left.FullName == t.FullName)
				{
					removeFromInput.Add(redirect.GetByAncs(curAssign));
				}
			}
		}
	}
	catch(Exception e)
	{
		cxLog.WriteDebugMessage(e);
	}
}

redirect -= removeFromInput;

CxList sanitizers = Find_Integers();
CxList redirectWithMethods = All.FindByParameters(redirectMethods);
redirectWithMethods.Add(redirect);

result.Add(redirectWithMethods.InfluencedByAndNotSanitized(inputs, sanitizers, CxList.InfluenceAlgorithmCalculation.NewAlgorithm));
result = result.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);