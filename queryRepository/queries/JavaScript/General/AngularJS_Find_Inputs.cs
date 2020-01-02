if(cxScan.IsFrameworkActive("AngularJS"))
{
	List<string> names = new List<string>(new string[]{"$location","$routeParams"});
	CxList potentialInputs = All.FindByShortNames(names);
	//This lambda verify if the potential input is inside in a ng-controller or ng-app
	Func<CxList,bool> isAngularJSInput = (potentialInput) => 
		{	
		return (potentialInput.GetAncOfType(typeof(ViewDecl)).Count > 0);	
		}; 

	foreach(CxList pIn in potentialInputs)
	{
		if (isAngularJSInput(pIn))
		{
			result.Add(pIn);
		}
	}
}