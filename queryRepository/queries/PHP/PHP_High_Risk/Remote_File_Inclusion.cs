CxList inputs = Find_Interactive_Inputs();

CxList methods = Find_Methods();
CxList include = methods.FindByShortNames(new List<string>(){ "include", "include_once", "require", "require_once" }, false);

CxList sanitizer = All.NewCxList();
sanitizer.Add(Find_Integers());
sanitizer.Add(Find_WP_File_Inclusion_Sanitize());

result = include.InfluencedByAndNotSanitized(inputs, sanitizer).ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);