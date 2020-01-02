// Find all relevant setSeed commands and parameters
CxList setSeed = All.FindByMemberAccess("Random.setSeed");
CxList setSeedParams = All.GetParameters(setSeed);

// Find all the "final" parameters that are affecting the seed
CxList finalParamsDef = All.FindDefinition(setSeedParams).FindByRegex(@"\Wfinal\s");
CxList pathsFromFinal = All.NewCxList();
foreach (CxList final in finalParamsDef)
{
	CxList allItsReferences = setSeedParams.FindAllReferences(final);
	pathsFromFinal.Add(final.ConcatenateAllTargets(allItsReferences));
}

// Find the numerical seeds
CxList numberSetSeed = setSeedParams.FindByType(typeof(IntegerLiteral));

// Find all the integers that affect the seed without any binary expressions in between
CxList integers = All.FindByType(typeof(IntegerLiteral));
CxList bin = All.FindByType(typeof(BinaryExpr));
CxList numberAffecting = integers.InfluencingOnAndNotSanitized(setSeedParams, bin);

// Put all in the result
result = numberSetSeed + pathsFromFinal + numberAffecting;

// Remove loops containing other shorter loops
result = result.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);