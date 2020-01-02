CxList inputs = Find_Interactive_Inputs();
CxList include = 
	All.FindByMemberAccess("response.include") + // prep from jsp:include
	All.FindByMemberAccess("response.Import") + // prep from c:import
	All.FindByMemberAccess("RequestDispatcher.include") +
	All.GetParameters(All.FindByMemberAccess("JspRuntimeLibrary.include"), 2); // only second parameter is relevant
CxList sanitize = Find_General_Sanitize();

result = inputs.InfluencingOnAndNotSanitized(include, sanitize, CxList.InfluenceAlgorithmCalculation.NewAlgorithm);