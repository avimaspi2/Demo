CxList classes = Find_Class_Decl();
CxList interfaces = All.FindByType(typeof(InterfaceDecl));

CxList actionClass = Find_Action_Classes();
 
CxList setMethods = All.FindByType(typeof(MethodDecl)).FindByShortName("set*");
setMethods = setMethods.FindByFieldAttributes(Modifiers.Public);
CxList secondParameter = All.GetParameters(setMethods, 1);
setMethods -= secondParameter.GetFathers().GetFathers();

CxList mySetMethods = setMethods.GetByAncs(actionClass);
CxList struts2Inputs = All.GetParameters(mySetMethods, 0);
CxList inputsToSearch = struts2Inputs;

CxList classList = All.NewCxList();
CxList inputTypes;
int counter = 0;
do {
	inputTypes = All.NewCxList();
	foreach(CxList myInput in inputsToSearch)
	{
		CSharpGraph gr = myInput.data.GetByIndex(0) as CSharpGraph;
		CxList myClassList = (classes + interfaces).FindByShortName(gr.TypeName);
		inputTypes = myClassList - classList;
		classList.Add(inputTypes);
	}
	if (inputTypes.Count > 0)
	{
		mySetMethods = setMethods.GetByAncs(inputTypes);
		inputsToSearch = All.GetParameters(mySetMethods, 0);
		struts2Inputs.Add(inputsToSearch);
	}
}
while (inputTypes.Count > 0 && counter++ < 100);

CxList inActionClass = All.GetByAncs(actionClass);
CxList session = Find_Methods().FindByShortName("getSession");
CxList map = inActionClass.FindByType("Map");
session = map.DataInfluencedBy(session);
CxList sessionAware = inActionClass.InheritsFrom("SessionAware");
session.Add(inActionClass.FindByShortName("session").GetByAncs(sessionAware));

CxList sessionTarget = session.GetMembersOfTarget();
CxList sessionGet = sessionTarget.FindByShortName("get");
sessionGet -= sessionGet.FindByShortName("getAttribute");  // This is treated as real input/DB when applicable
CxList sessionGetParams = inActionClass.GetByAncs(All.GetParameters(sessionGet, 0));

CxList potentialInputs = struts2Inputs + sessionGetParams;
result = All.FindDefinition(All.GetByAncs(potentialInputs) - potentialInputs);
result = potentialInputs + All.GetParameters(All.FindByType(typeof(MethodDecl)).FindByShortName("set*").GetByAncs(result));

result -= Find_Interactive_Inputs();