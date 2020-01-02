CxList eventListener = All.FindByMemberAccess("*.AddEventListener", false);
eventListener.Add(Find_JQuery_Methods().FindByShortName("on"));

/*$(window).on("message", function(e) {*/
/*window.addEventListener('message', foo,false);*/
/*
document.addEventListener('message', function (event) {*/
CxList message = Find_String_Short_Name(All, "message", false).GetParameters(eventListener);

CxList method = All.GetParameters(eventListener.FindByParameters(message), 1);
CxList decls = Find_MethodDecls();
CxList eventMethods = All.NewCxList();

foreach(CxList function in method)
{
	CSharpGraph g = function.GetFirstGraph();
	if(g != null && g.ShortName != null && g.LinePragma != null)
	{
		string fileName = g.LinePragma.FileName;
		if(function.FindByShortName("*var").Count > 0)
		{			
			CxList newFunc = All.FindByShortName(function).FindAllReferences(function).GetMembersOfTarget();
			eventMethods.Add(decls.FindDefinition(newFunc));
		}
		eventMethods.Add(decls.FindByShortName(function).FindByFileName(fileName));	
	}
}
CxList eventAsParam = All.GetParameters(eventMethods);

result = All.FindAllReferences(eventAsParam);