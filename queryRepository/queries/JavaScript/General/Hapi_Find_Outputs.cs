//Find outputs heuristic and specific
CxList strings = NodeJS_Find_Strings();

// Hapi Queries
CxList serverObject = Hapi_Find_Server_Instance();
CxList replyInterface = Hapi_Find_Reply_Interface();
CxList responseObject = Hapi_Find_Response_Objects();
CxList replyInterfaceInvokes = Hapi_Find_Reply_Interface_Invokes();

// View
CxList viewRefs = replyInterface.GetMembersOfTarget().FindByShortName("view");

//Find reply methods with 1 parameter only
CxList secondParameterOfReplyInterface = All.GetParameters(replyInterfaceInvokes, 1);
CxList allParameterOfReplyInterface = All.GetParameters(replyInterfaceInvokes);
CxList paramIntersect = secondParameterOfReplyInterface * allParameterOfReplyInterface;
CxList replyInterfaceWithOneParameterOnly = replyInterfaceInvokes - All.FindByParameters(paramIntersect);

//Find the response object of the request func
//Add functions which are under .ext: server.ext('onPreResponse', function(request, reply) {..})
CxList extFuncs = serverObject.GetMembersOfTarget().FindByShortName("ext");
CxList extFuncsFirstParameter = All.GetParameters(extFuncs, 1);
CxList extFirstParameter = All.GetParameters(extFuncsFirstParameter, 1);
CxList extFuncsReturn = All.FindAllReferences(extFirstParameter) - extFirstParameter; 

// Add the peek event
// 'peek' - emitted for each chunk of data written back to the client connection.
CxList peekEvent = responseObject.GetMembersOfTarget().FindByShortName("on");
CxList peek = strings.FindByShortNames(new List<string> {"peek"});
peek = peek.GetParameters(peekEvent, 0);
peekEvent = peekEvent.FindByParameters(peek);
CxList peekEventOutput = All.NewCxList();
if(peek.Count > 0)
{
	CxList callbackFunc = All.GetParameters(peekEvent, 1);
	peekEventOutput.Add(All.GetParameters(callbackFunc, 0));
	peekEventOutput = All.FindAllReferences(peekEventOutput);
}

result.Add(viewRefs);
result.Add(replyInterfaceWithOneParameterOnly);
result.Add(extFuncsReturn);
result.Add(peekEventOutput);