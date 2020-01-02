// Find all relevant setSeed commands and parameters
CxList setSeedParams = Find_PRNG_SetSeed();

// Find all the "final" values that are affecting the seeds of a PRNG
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
CxList integers = Find_IntegerLiterals();
CxList bin = Find_BinaryExpr();
CxList numberAffecting = integers.InfluencingOnAndNotSanitized(setSeedParams, bin);

// Put all in the result
result.Add(numberSetSeed);
result.Add(pathsFromFinal);
result.Add(numberAffecting);

// Remove loops containing other shorter loops
result = result.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);