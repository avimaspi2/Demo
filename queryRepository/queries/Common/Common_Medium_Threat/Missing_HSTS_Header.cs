/*
	This query checks if all responses sent from a server have the
Strict-Transport-Security header (HSTS).
	If the header isn't set at all, every response written to a client is returned as result.
	When the header is set, this query checks if the value "max-age" is not lower than 31536000
seconds(one year) and if the flag "includeSubDomains" is set to "true" (most of this
validation is made by the general query "Validate_HSTS_Header"). If these two
conditions aren't verified the result will contain the headers where the validation failed.
	Also, this header can be set with XML configuration files. In this case, if the header
properties are well set, the whole project is considered sanitized.
*/
bool show_only_one_HSTS_result = true;
CxList xmlSanitizers = general.Find_HSTS_Configuration_In_Config_File();
CxList codeSanitizers = general.Find_HSTS_Configuration_In_Code();
CxList AspNetCoreCodeSanitizers = general.Find_HSTS_Configuration_In_Code_ASPNetCore();
if(xmlSanitizers.Count > 0 || codeSanitizers.Count > 0 || AspNetCoreCodeSanitizers.Count > 0){
	CxList globalConfig = general.Check_HSTS_Configuration();
	if(show_only_one_HSTS_result){
		if(globalConfig.Count > 0){
			result.Add(globalConfig.GetFirstGraph().NodeId, globalConfig.GetFirstGraph());
		}
	}
	else{
		result = globalConfig;	
	}
}
else{
	/* at this point we're assuming that there's no XML configuration regarding
	HSTS, so from now on the algorithm follows these steps:
		- find every object that contains a response to a request
		- find every header manipulation methods of the responses found
		- find every method that sends the reponse to the client
		- return every response object that is sent without HSTS and every header 
	values that don't meet the required conditions (mentioned in the first comment)*/
	CxList writeResponse = general.Find_HTTP_Response_Write();
	CxList aux = writeResponse.GetLeftmostTarget();
	if(aux.Count == 0){
		aux = All.GetByAncs(writeResponse).FindByType(typeof(UnknownReference));
		aux.Add(writeResponse);
	}
	
	CxList responseHeaderChanges = general.Find_Change_Response_Header();
	CxList headers = general.Get_HSTS_Headers(responseHeaderChanges);
	
	CxList responsesWithHSTS = general.Get_Responses_With_HSTS(aux, headers);

	if(show_only_one_HSTS_result){
		foreach(CxList x in responsesWithHSTS.GetCxListByPath()){
			CxList header = x.GetStartAndEndNodes(Checkmarx.DataCollections.CxQueryProvidersInterface.CxList.GetStartEndNodesType.StartNodesOnly);
			if(Validate_HSTS_Header(header).Count > 0){
				CxList write = x.GetRightmostMember() * writeResponse;
				result = x.ConcatenatePath(write, false);
				break;
			}
		}
		if(result.Count == 0){
			result = (aux - responsesWithHSTS.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly)).GetRightmostMember();
		}
		if(result.Count > 0){
			CxList theSingleResult = All.NewCxList();
			theSingleResult.Add(result.GetFirstGraph().NodeId, result.GetFirstGraph());
			result = theSingleResult;
		}
	}
	else{
		CxList badResponses = All.NewCxList();
		foreach(CxList x in responsesWithHSTS.GetCxListByPath()){
			CxList header = x.GetStartAndEndNodes(Checkmarx.DataCollections.CxQueryProvidersInterface.CxList.GetStartEndNodesType.StartNodesOnly);
			if(Validate_HSTS_Header(header).Count > 0){
				badResponses.Add(x);
			}
		}
		foreach(CxList p in badResponses.GetCxListByPath()){
			CxList write = p.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly).GetRightmostMember() * writeResponse;
			if(write.Count > 0){
				result.Add(p.ConcatenatePath(write, false));
			}
		}
		result.Add((aux - responsesWithHSTS.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly)).GetRightmostMember());	
	}
		
}