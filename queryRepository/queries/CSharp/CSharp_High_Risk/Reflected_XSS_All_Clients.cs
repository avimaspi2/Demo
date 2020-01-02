if(All.isWebApplication)
{
	CxList inputs = Find_Interactive_Inputs();

	CxList outputs = Find_XSS_Outputs() - Find_Console_Outputs();

	CxList sanitized = Find_XSS_Sanitize();
	
	// FileInfo can be a sanitizer:
	CxList fileInfos = All.FindByType("FileInfo").GetMembersOfTarget();
	
	CxList responses = Find_Response();
	CxList changeHeaderMethods = Find_Change_Response_Header();
	
	//FileInfo.extension is a sanitizer
	sanitized.Add(fileInfos.FindByShortName("Extension"));
	// TransmitFile method is not problematic since the input string is not transmitted
	sanitized.Add(responses.GetMembersOfTarget().FindByShortName("TransmitFile"));
	// Response.AppendHeader() is sanitizer if only the filename is used and not the initial input string
	sanitized.Add(changeHeaderMethods.FindByParameters(fileInfos.FindByShortNames(new List<string> {"FullName","Name"})));

	//get the sanitized HtmlFormattableString
	CxList myParam = Find_Param();
	CxList objHtml = outputs.FindByType("Microsoft.AspNetCore.Html.HtmlFormattableString");
	CxList firstparam = All.FindByFathers(myParam.GetParameters(objHtml, 0));
	CxList firstParamNotInfluenced = firstparam.NotInfluencedBy(inputs).GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly).GetFathers();
	CxList objHtmlSanitized = objHtml.FindByParameters(firstParamNotInfluenced);
	outputs -= objHtmlSanitized;
	
	CxList resultsAux = All.NewCxList();
	CxList paramAndChild = All.FindByFathers(myParam);
	CxList constructorList = objHtml - objHtmlSanitized;
	
	foreach(CxList constructor  in constructorList) {
		CxList auxparam = paramAndChild.GetParameters(constructor, 0);
		CxList auxFlow = inputs.InfluencingOn(auxparam);	
		resultsAux.Add(auxFlow.ConcatenatePath(constructor));		
	}

	result = inputs.InfluencingOnAndNotSanitized(outputs, sanitized);
	result.Add(resultsAux);
}