//
CxList inputs = Find_Interactive_Inputs();

CxList requestDispatcher = Find_Request_Dispatcher_Unvalidated_Forwards();

CxList rdParams = Find_UnknownReference().GetParameters(requestDispatcher);
rdParams.Add(Find_Strings().GetParameters(requestDispatcher));
rdParams.Add(Find_Methods().GetParameters(requestDispatcher));

CxList forward = Find_Forward_Unvalidated_Forwards();

result= inputs.DataInfluencingOn(rdParams).DataInfluencingOn(forward);