if(cxScan.IsFrameworkActive("Jelly"))
{
	CxList ifEvaluate = All.FindByShortName("evaluate").GetFathers().FindByType(typeof(IfStmt));
	CxList evaluated = All.GetByAncs(ifEvaluate);
	CxList sanitize = Sanitize();

	CxList cxInput = All.FindByShortName("CxInput");
	CxList input = (All - cxInput).GetByAncs(cxInput);


	result = input.InfluencingOnAndNotSanitized(evaluated, sanitize);
	result = result.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);

	CxList evaluatedInput = input * evaluated;
	result.Add(evaluatedInput * Find_Methods());
	result.Add(evaluatedInput * Find_UnknownReference());
}