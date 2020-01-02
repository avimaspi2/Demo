//result.Add(All.FindByMemberAccess("document.getElement*")); 
result.Add(Find_Outputs_CodeInjection());
result.Add(Find_Outputs_Redirection());
result.Add(Find_Outputs_Redirection_Methods());
result.Add(Find_Outputs_XSRF());
result.Add(Find_Outputs_XSS());
result.Add(Find_Web_Messaging_Outputs());
result.Add(Find_MsAjax_Outputs());
result.Add(Find_Framework_Outputs());
result.Add(ReactNative_Find_Outputs());

//Add location.hash if the file includes jQuery before 1.6.2
CxList oldJQuery = Find_Outdated_JQuery_File("1.6.2", @"jquery[/-]");
CxList hashes = Find_Members("location.hash");
hashes = hashes.GetAncOfType(typeof(MethodInvokeExpr));
hashes = hashes.FindByShortNames(new List<string>{"$","jQuery"});

foreach (CxList import in oldJQuery) 
{
	try{
		CSharpGraph g = import.GetFirstGraph();
		if (g.LinePragma != null && g.LinePragma.FileName != null)
		{
			string filename = g.LinePragma.FileName;
			result.Add(hashes.FindByFileName(filename));
		}
	} catch(Exception e){
		cxLog.WriteDebugMessage(e);
	}
}

CxList sap = Find_SAP_Library();
CxList newWebSocket = sap.FindByType("sap.ui.core.ws.WebSocket");
newWebSocket.Add(Find_ObjectCreations().FindByType("WebSocket"));

CxList webSocket = All.FindAllReferences(newWebSocket.GetAssignee());
webSocket.Add(newWebSocket);

CxList webSocketSend = webSocket.GetMembersOfTarget().FindByShortName("send");
result.Add(All.GetParameters(webSocketSend, 0));
result -= Find_Param();