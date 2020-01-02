if(Find_Ctp_Files().Count > 0)
{
	//find all underneath the controller
	CxList controller = All.FindByType(typeof(ClassDecl)).FindByShortName("*Controller");
	CxList controllerAncs = All.GetByAncs(controller);
	CxList tr = controllerAncs.FindByType(typeof(ThisRef));
	CxList ma = controllerAncs.FindByType(typeof(MemberAccess));
	CxList uref = controllerAncs.FindByType(typeof(UnknownReference));
	
	CxList req = controllerAncs.FindByShortName("request");
	CxList reqArr = controllerAncs.FindByShortName("request_*");
	
	List <string> metInvList = new List<string>{"query", "input", "header", "data", "params", "get*"};
	List <string> memAccList = new List<string>{"query_*", "data_*", "params_*", "pass"};
	List <string> pasArgList = new List<string>{"passedArgs", "passedArgs_*"};
			
	//find phpRequest elements
	
	//$this->request...
	CxList thisTargets = tr.GetMembersOfTarget();
	CxList thisRequests = thisTargets * req;
	CxList thisReqMot = thisRequests.GetMembersOfTarget();
	CxList input = All.NewCxList();
	CxList thisReqInv = thisReqMot.FindByShortNames(metInvList).FindByType(typeof(MethodInvokeExpr));
	CxList thisReqMem = thisReqMot.FindByShortNames(memAccList).FindByType(typeof(MemberAccess));
	
	//$this->passedArgs, $this->passedArgs[..], func_get_args()
	CxList thisPasArg = thisTargets.FindByShortNames(pasArgList);
	CxList funcGetArg = controllerAncs.FindByShortName("*func_get_args*");
	//debug(func_get_args())
	funcGetArg.Add(uref.FindByShortName("__CX_MethodArgv"));
	
	input.Add(thisReqMem);
	input.Add(thisReqInv);
	input.Add(thisTargets * reqArr);
	input.Add(thisPasArg);
	input.Add(funcGetArg);

	//$request->....	
	CxList requestAnc = uref * req;
	CxList ReqMetInv = requestAnc.GetMembersOfTarget().FindByShortNames(metInvList).FindByType(typeof(MethodInvokeExpr));
	input.Add(ReqMetInv);
	
	
	//controllers/action/param1/param2 corresponds to ControllersController::action($param1, $ param2)
	//input as action parameter : public function delete($id){....}
	
	CxList action = All.FindByType(typeof(MethodDecl)).GetByAncs(controller);
	CxList actionParam = All.FindByType(typeof(ParamDecl)).GetParameters(action) - All.FindByShortName("*_CX_*");
	input.Add(actionParam);

	result = input;
}