if(Find_Ctp_Files().Count > 0)
{
	//find all underneath the controller
	CxList controller = All.FindByType(typeof(ClassDecl)).FindByShortName("*Controller");
	CxList sl = Find_Strings();
	CxList tr = All.FindByType(typeof(ThisRef));
	CxList mie = Find_Methods();
	CxList controllerAncs = (sl + tr + mie).GetByAncs(controller);

	CxList thisRef = controllerAncs.FindByType(typeof(ThisRef));
	//find phpReponse element
	CxList CakePHPRequest = thisRef.GetMembersOfTarget().FindByShortName("response");


	// $this->response->header

	CxList paramsList = controllerAncs.FindByType(typeof(MethodInvokeExpr));
	//all allowed array literals
	List < string > paramsListStrings = new List<string>(new string[] {"header", "download", "body"});
	
	paramsList = paramsList.FindByShortNames(paramsListStrings); 

	result = CakePHPRequest.GetMembersOfTarget() * paramsList;
	
	// Passing data to the view using $this->set('name', $value)
	CxList thisSet = thisRef.GetMembersOfTarget().FindByShortName("set");
	result.Add(All.GetParameters(thisSet, 1));
}