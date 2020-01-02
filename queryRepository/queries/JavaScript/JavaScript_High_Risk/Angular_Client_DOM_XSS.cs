CxList methods = Find_Methods();
CxList inputs = Find_Inputs();
CxList outputs = Angular_Find_Outputs_XSS();
CxList sanitizer = methods.FindByShortName("CxDefaultSanitizer");
CxList desanitizer = methods.FindByShortName("bypassSecurityTrust*");

CxList allPotentialXssFlows = outputs.InfluencedBy(inputs);
CxList flowsWithoutSanitizer = outputs.InfluencedByAndNotSanitized(inputs, sanitizer);
CxList flowsWithSanitizer = allPotentialXssFlows - flowsWithoutSanitizer;
CxList flowsWithoutDesanitizer = outputs.InfluencedByAndNotSanitized(inputs, desanitizer);
CxList flowsWithDesanitizer = allPotentialXssFlows - flowsWithoutDesanitizer;

CxList flowsWithBothSanitizerAndDesanitizer = flowsWithSanitizer * flowsWithDesanitizer;


result.Add(flowsWithoutSanitizer);
result.Add(flowsWithBothSanitizerAndDesanitizer);