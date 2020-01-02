/*This query will look for XSS saniization (escaping)*/

if(cxScan.IsFrameworkActive("XSJS"))
{
	CxList XSAll = XS_Find_All();
	CxList methodInvoke = Find_Methods() * XSAll;
	CxList allParams = Find_Parameters() * XSAll;
	CxList uRef = Find_UnknownReference() * XSAll;
	CxList mA = Find_MemberAccesses() * XSAll;
	CxList urMA = uRef.Clone();
	urMA.Add(mA);

	CxList sanitize = methodInvoke.FindByShortNames(
		new List<string>{
			"escape",
			"encodeURIComponent",
			"*.escapeId","encodeURI"
			}
		);

	//Add all parameters of sanitation methods to sanitizers
	CxList _params = allParams.Clone();
	_params.Add(uRef);
	CxList sanitizeParams = _params.GetParameters(sanitize);

	sanitize.Add(urMA.GetByAncs(sanitizeParams));

	result.Add(sanitize);
	result.Add(XS_Find_Integers());

	//add mail.send as a sanitizer for the xss - this flow should be minimal
	result.Add(All.FindByMemberAccess("Mail.send"));

	// add to the result of Query update to sanitation
	CxList dbIn = XS_Find_DB_In();
	CxList method_Of_DB_In = XSAll.FindByParameters(dbIn);
	//we are only interested in ExecuteUpdate and prepareStatement
	CxList executeQuery = method_Of_DB_In.FindByShortName("executeQuery");

	//execute update is added in XS_Find_Integers
	//executeQuery should be added to sanitizers only in case it is influenced by string that starts with an update command.
	CxList stringLiteral = Find_String_Literal();
		
	CxList stringXSAll = stringLiteral * XSAll;	
	
	List<string> names = new List<string>(new string[]{"*UPDATE *","*DELETE *","*INSERT *","*CREATE *","*ALTER *","*DROP *"});
	CxList onlyRelevantStrings = stringXSAll.FindByShortNames(names, false);
	//to cover the cases when a query starts with a whitespace sequence
	List<string> croppedNames = new List<string>(new string[]{"UPDATE","DELETE","INSERT","CREATE","ALTER","DROP"});
	CxList valid = All.NewCxList();
	foreach(CxList curString in onlyRelevantStrings)
	{
		string name = curString.GetName();
		string nameAfterTrim = name.Trim();
		foreach(string cn in croppedNames)
		{
			if(nameAfterTrim.StartsWith(cn))
			{
				valid.Add(curString);
			}
		}	
	}
	result.Add(executeQuery.DataInfluencedBy(valid));

	// add application/json content type as sanitizer
	CxList applicationJson = stringLiteral.FindByShortName("*application/json*");
	CxList contentTypeResponse = applicationJson.GetAssignee().GetTargetOfMembers();
	CxList responsesReferences = All.FindAllReferences(contentTypeResponse).FindByFiles(contentTypeResponse);
	CxList responsesMembers = responsesReferences.GetMembersOfTarget();
	result.Add(responsesMembers);
}