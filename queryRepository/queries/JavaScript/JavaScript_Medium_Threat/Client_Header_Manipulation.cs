CxList inputs = Find_Inputs();

// Look for XHR.setRequestHeader(header,value);
CxList XHRHeaders = Find_XHR_Headers();
CxList setRequestHeader = XHRHeaders.FindByShortName("setRequestHeader");
CxList sanitizer = Sanitize();
CxList setRequestHeaderParam = All.GetParameters(setRequestHeader);
result = setRequestHeaderParam.InfluencedByAndNotSanitized(inputs, sanitizer);

// Look for $.ajax({headers:values})
CxList XHRParams = XHRHeaders - setRequestHeader;
CxList headersAssigned = Find_Assign_Lefts().FindByShortName("headers");
XHRParams -= Find_Parameters();

CxList full = inputs.DataInfluencingOn(XHRParams);
foreach(CxList flow in full.GetCxListByPath()){
	CxList headers = flow.GetStartAndEndNodes(CxList.GetStartEndNodesType.AllNodes) * headersAssigned;
	if(headers.Count > 0)
	{
		result.Add(flow);
	}
}