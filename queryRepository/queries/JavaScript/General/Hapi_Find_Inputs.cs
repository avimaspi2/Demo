/*We search for Hapi inputs in 2 ways:
1) Heuristic - find request object, cookies and more*
2) Look for the objects specificlly */

// Hapi Queries
CxList strings = NodeJS_Find_Strings();
CxList heuristicRequests = Hapi_Find_Request_Objects();
CxList cookiesInputs = Hapi_Find_Cookies_Inputs();
CxList joiReferences = Hapi_Find_Joi_References();

//After finding the request object(heuristic + specific), lets start looking for inputs.
List<string> inputsMethods = new List<string> {"params", "query", "paramsArray", "payload", "raw", "url"};
CxList heuristicRequestsMembers = heuristicRequests.GetMembersOfTarget().FindByShortNames(inputsMethods);

/*
	Cases:
		 	request.query.name, where 'name' is the parameter received from the client.

*/
CxList requestInputs = heuristicRequestsMembers.GetMembersOfTarget();

/*
	Cases:
	 		var queryString = request.query;
			var input = queryString.name;
*/
CxList leftSide = heuristicRequestsMembers.GetFathers();
leftSide = leftSide.FindByAssignmentSide(CxList.AssignmentSide.Left);
CxList allLeftSideRefsRightMostMember = All.FindAllReferences(leftSide).GetRightmostMember();

requestInputs.Add(allLeftSideRefsRightMostMember);

// Add the peek event
// 'peek' - emitted for each chunk of payload data read from the client connection
CxList peekEvent = heuristicRequests.GetRightmostMember().FindByShortName("on");
CxList peek = strings.FindByShortName("peek");
peek = peek.GetParameters(peekEvent, 0);
peekEvent = peekEvent.FindByParameters(peek);
CxList peekEventInput = All.NewCxList();
if(peekEvent.Count > 0)
{	
	CxList callbackFunc = All.GetParameters(peekEvent, 1);
	peekEventInput.Add(All.GetParameters(callbackFunc, 0));
}

//Add potential inputs which are validated through the Joi validation
CxList joiValidate = joiReferences.GetMembersOfTarget().FindByType(typeof(MethodInvokeExpr)).FindByShortName("validate");
CxList parameters = All.GetParameters(joiValidate);
CxList validateFirstParameter = parameters.GetParameters(joiValidate, 0);

CxList validateInput = heuristicRequests.DataInfluencingOn(validateFirstParameter)
	.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);

validateInput.Add(validateFirstParameter * requestInputs);

joiValidate = joiValidate.FindByParameters(validateInput);

CxList joiValidateCallback = parameters.GetParameters(joiValidate, 2);
CxList joiParams = All.GetParameters(joiValidateCallback, 1);

result.Add(requestInputs);
result.Add(cookiesInputs);
result.Add(peekEventInput);
result.Add(joiParams);