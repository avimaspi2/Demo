CxList methods = Find_Methods();
CxList parameters = Find_Param();
CxList outputs = NodeJS_Find_Interactive_Outputs();
outputs.Add(Find_Outputs_XSS());

// Only the first parameter from res.Render is vulnerable. The second can be ignored, so we add it to sanitizers below
CxList unknRefs = Find_UnknownReference();
CxList res = unknRefs.FindByShortName("res");
CxList renders = res.GetMembersOfTarget().FindByShortName("render");
CxList rendersSanitizedParams = parameters.GetParameters(renders, 1);

CxList inputs = NodeJS_Find_Interactive_Inputs();

CxList sanitize = NodeJS_Find_XSS_Sanitize();
// Angular IO default sanitizers
CxList angularDefaultSanitizers = methods.FindByShortName("CxDefaultSanitizer");
sanitize.Add(angularDefaultSanitizers - angularDefaultSanitizers.DataInfluencedBy(Find_Angular_Sanitizers_Bypass()));

sanitize.Add(rendersSanitizedParams);
sanitize.Add(All.GetByAncs(rendersSanitizedParams));

CxList lambdas = Find_LambdaExpr();
CxList paramDecls = Find_ParamDecl();
CxList memberAccess = Find_MemberAccesses();
CxList activatedRoutes = All.FindByType("ActivatedRoute");
CxList potencialInputs = memberAccess.GetByAncs(activatedRoutes).FindByShortNames(new List<string>(){"queryParams", "params"}).GetMembersOfTarget();

CxList potencialServiceSubscriptions = (methods.DataInfluencedBy(potencialInputs)).GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
CxList callbackParams = paramDecls.GetParameters(lambdas.GetParameters(potencialServiceSubscriptions));

CxList callbackParamsRefs = All.FindAllReferences(callbackParams).GetRightmostMember();
// In case of IndexerRef
callbackParamsRefs.Add(callbackParamsRefs.GetFathers().GetMembersOfTarget());

CxList influencedResults = All.DataInfluencedBy(callbackParamsRefs).ReduceFlow(Checkmarx.DataCollections.CxQueryProvidersInterface.CxList.ReduceFlowType.ReduceSmallFlow).GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly);

inputs.Add(influencedResults);

// If SWIG is not prone to XSS (auto escaped) then remove SWIG outputs.
if(NodeJS_Find_Swig_Autoescape_False().Count == 0)
{
	outputs -= NodeJS_Find_Swig_Interactive_Outputs();
}

result = outputs.InfluencedByAndNotSanitized(inputs, sanitize);
result.Add(inputs * outputs - sanitize);

result = result.ReduceFlow(Checkmarx.DataCollections.CxQueryProvidersInterface.CxList.ReduceFlowType.ReduceBigFlow);