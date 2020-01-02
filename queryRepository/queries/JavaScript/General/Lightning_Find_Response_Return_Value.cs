if(param.Length == 1)
{

	CxList ssAccess = param[0] as CxList;

	CxList allInController = Lightning_Find_All_In_Controller();
	CxList assignee = allInController.FindByParameters(ssAccess).GetAssignee();
	CxList action = allInController.FindAllReferences(assignee);
	CxList actionSetCallback = action.GetMembersOfTarget().FindByShortName("setCallback");
	
	CxList potentialCallBack = All.GetParameters(actionSetCallback);
	
	CxList methodFound = Get_CallBack_Function(potentialCallBack);
	CxList responseObject = allInController.GetParameters(methodFound);
	responseObject = allInController.FindAllReferences(responseObject);
	result = responseObject.GetMembersOfTarget().FindByShortName("getReturnValue");
}