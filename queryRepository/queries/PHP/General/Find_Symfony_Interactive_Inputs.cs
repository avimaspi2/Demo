if(Find_Twig().Count > 0)
{	
	//symfony 1 input coverage:
	//inside an action 
	// public function executeIndex(sfWebRequest $request)
	CxList ur = All.FindByType(typeof(UnknownReference));
	CxList symfony1ControllerClass = All.FindByType(typeof(ClassDecl)).InheritsFrom("sfActions");
	CxList actionDecl = All.FindByType(typeof(MethodDecl)).GetByAncs(symfony1ControllerClass);
	CxList paramRequest = All.FindByType("sfWebRequest").FindByFathers(All.GetParameters(actionDecl));
	paramRequest = paramRequest.GetFathers();
	CxList allRequestReferences = ur.FindAllReferences(paramRequest);
	CxList requestInvokesMethod = allRequestReferences.GetMembersOfTarget();

	CxList mie = Find_Methods();
	CxList AllGets = (mie.FindByShortName("get") + mie.FindByShortName("all"));
	//http://www.symfony-project.org/api/1_0/sfWebRequest
	CxList requestTypes = mie.FindByShortNames(new List<string>
		{"getFile*", "getParameter*", "getPostParameter*", "getCookie",	"extractParameters",
		"getContent","getRequestParameter*","getReferer","getPathInfoArray","getHost",
		"getRelativeUrlRoot","getScriptName", "getUri"});
		
	result.Add(requestInvokesMethod * requestTypes);


	//$this->getRequest();
	// $params = $this->getContext()->getRequest()->getRequestParameters();
	//$this->getContext()->getRequest();

	CxList getReqDirect = requestTypes * mie.FindByShortName("getRequest").GetMembersOfTarget();
	result.Add(getReqDirect);
	CxList myReq = mie.FindByShortName("getRequest");
	myReq -= All.FindByMemberAccess("getRequest.*").GetTargetOfMembers();
	CxList ae = myReq.GetAncOfType(typeof(AssignExpr));
	CxList unr = ur.GetByAncs(ae);
	CxList curRep = unr.FindByAssignmentSide(CxList.AssignmentSide.Left);
	CxList repRef = All.FindByType(typeof(UnknownReference)).FindAllReferences(curRep);
	CxList method = repRef.GetMembersOfTarget();
	result.Add(method * requestTypes);

	//$sf_request->getParameter('dd');
	result.Add(ur.FindByShortName("sf_request").GetMembersOfTarget() * requestTypes);
	//$sf_params->get('total');
	result.Add(ur.FindByShortName("sf_params").GetMembersOfTarget() *
		mie.FindByShortName("get"));


	//$sf_context->getRequest(); --to check
	//$request->getParameterHolder()->get('foo');

	result.Add(mie.FindByShortName("getParameterHolder").GetMembersOfTarget().FindByShortName("get"));

	//symfony 1  forms interactive inputs:
	/*

	$this->form->bind($request->getParameter('contact'));
	if ($this->form->isValid())
	{
	$contact = $this->form->getValues();
	}

	*/
	CxList tr = All.FindByType(typeof(ThisRef)).GetMembersOfTarget().FindByShortName("form");

	tr = tr.GetMembersOfTarget().FindByShortName("getValue*") 
		+ tr.GetMembersOfTarget().FindByShortName("getData");
	result.Add(tr.GetByAncs(actionDecl));


	//symfony 2 interactive inputs:


	//this->get('request')
	CxList controllerS2 = All.FindByType(typeof(ClassDecl)).InheritsFrom("Controller");
	CxList S2Action = All.FindByType(typeof(MethodDecl)).GetByAncs(controllerS2);
	CxList thisGet = All.FindByType(typeof(ThisRef)).GetMembersOfTarget().FindByShortName("get").FindByType(typeof(MethodInvokeExpr));
	thisGet = thisGet.GetByAncs(S2Action);
	CxList thisGetRequest = All.FindByShortName("request").GetParameters(thisGet);
	thisGet = thisGet.FindByParameters(thisGetRequest);

	CxList ma = All.FindByType(typeof(MemberAccess));
	CxList requestType = ma.FindByShortNames(new List<string> 
		{"query", "request", "server", "files","cookies",
		"headers","content", "attributes"});
//		ma.FindByShortName("query") +
//		ma.FindByShortName("request") +
//		ma.FindByShortName("server") +
//		ma.FindByShortName("files") +
//		ma.FindByShortName("cookies") +
//		ma.FindByShortName("headers") +
//		ma.FindByShortName("content") +
//		ma.FindByShortName("attributes");

	result.Add((method * requestType + requestType * mie.FindByShortName("getRequest").GetMembersOfTarget()).GetMembersOfTarget() * AllGets);

	//result.Add(ma.DataInfluencedBy(thisGet));


	//            $request = Request::createFromGlobals();
	//  $request->query->get('foo');
	CxList cfg = All.FindByMemberAccess("Request.createFromGlobals");
	ae = myReq.GetAncOfType(typeof(AssignExpr));
	unr = ur.GetByAncs(ae);
	curRep = unr.FindByAssignmentSide(CxList.AssignmentSide.Left);
	repRef = All.FindByType(typeof(UnknownReference)).FindAllReferences(curRep);
	method = repRef.GetMembersOfTarget();

	result.Add((method * requestType).GetMembersOfTarget() * AllGets);

	//public function updateFunction(Request $request)
	CxList methodDecl = All.FindByType(typeof(MethodDecl));
	CxList Request = (All.FindByType(typeof(TypeRef)).FindByShortName("Request")).GetByAncs(All.GetParameters(/*S2Action*/methodDecl));//fix this so it will not only be action
	//if it's not a controller make sure that Request exsits. if it's a controller any parameter will be input:
	CxList action = methodDecl.FindByShortName("*Action");
	result.Add(All.FindByType(typeof(ParamDecl)).GetParameters(action));
	result -= All.FindByShortName("__CX*");

	Request = Request.GetAncOfType(typeof(ParamDecl));

	Request.Add((All.FindByType(typeof(ParamDecl)) - All.FindByShortName("__CX*")).GetParameters(S2Action.FindByShortName("*Action")));

	CxList allRequestRef = ur.FindAllReferences(Request).GetMembersOfTarget();
	result.Add((allRequestRef * ma).GetMembersOfTarget() * AllGets);



	//now we add any parameter of action definition.


	//form Symfony 2
	/*
	$form = this->createFormBuilder($task)->...;
	a) $form->getValues();
	b) $data = $form->getData()
	c)$username = $form["username"]->getData();
	*/

	CxList cfb = mie.FindByShortName("createFormBuilder") + mie.FindByShortName("createForm");
	CxList getValuesData = (mie.FindByShortName("getValues") + mie.FindByShortName("getData"));
	result.Add(getValuesData.DataInfluencedBy(cfb));


	//$crawler = $client->request('POST', '/api/teams/1/attend');

	CxList newFindByShortName = All.FindByShortNames(new List<string>{"GET", "POST", "SERVER", "COOKIE","FILES"});
	
	CxList postGet = //(All.FindByShortName("GET") + 
					 // All.FindByShortName("POST") +
					 // All.FindByShortName("SERVER") + 
					 // All.FindByShortName("COOKIE") + 
					 // All.FindByShortName("FILES"))
					  newFindByShortName.FindByType(typeof(StringLiteral));
	result.Add(postGet.GetParameters(mie.FindByShortName("request"), 0).GetAncOfType(typeof(MethodInvokeExpr)));
}