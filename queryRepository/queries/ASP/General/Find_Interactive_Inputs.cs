CxList inputs = 	
	All.FindByMemberAccess("text.value") +
	All.FindByMemberAccess("password.value") +
	All.FindByMemberAccess("hidden.value") +
	All.FindByMemberAccess("file.value") +	
	All.FindByMemberAccess("text.value") +
	All.FindByMemberAccess("password.value") +
	All.FindByMemberAccess("hidden.value") +
	All.FindByMemberAccess("file.value") +	
	All.FindByMemberAccess("select.value") +				
	All.FindByMemberAccess("textbox.text") + 
	All.FindByMemberAccess("richtextbox.text") + 	
	All.FindByMemberAccess("dropdownlist.selecteditem") + 
	All.FindByMemberAccess("dropdownlist.selectedvalue") + 
	All.FindByMemberAccess("listbox.selecteditem") + 
	All.FindByMemberAccess("listbox.selectedvalue") + 
	All.FindByMemberAccess("radiobuttonList.selecteditem") + 
	All.FindByMemberAccess("radiobuttonlist.selectedvalue") + 		
	All.FindByMemberAccess("checkboxlist.selecteditem") + 
	All.FindByMemberAccess("checkboxlist.selectedvalue") + 
	All.FindByMemberAccess("listcontrol.selectedvalue") + 
	All.FindByMemberAccess("listitem.value") + 
	All.FindByMemberAccess("listControl.text") + 
	All.FindByMemberAccess("textbox.text") + 
	All.FindByMemberAccess("dropdownlist.selecteditem") + 
	All.FindByMemberAccess("httpPostedfile.filename") + 
	All.FindByMemberAccess("httpPostedfile.contenttype") + 
	All.FindByMemberAccess("datarow.item") + 
	All.FindByMemberAccess("hiddenfield.value") + 
	All.FindByMemberAccess("fileupload.filename") + 
	All.FindByMemberAccess("htmlinputhidden.value") + 
	All.FindByMemberAccess("htmlinputpassword.value") + 
	All.FindByMemberAccess("htmlinputtext.value") +
	All.FindByMemberAccess("htmlinputfile.value") + 
	All.FindByMemberAccess("htmlselect.value") + 
	All.FindByMemberAccess("htmltextarea.value") +
	All.FindByMemberAccess("textbox.text") +
	All.FindByMemberAccess("httprequest.*") +
	All.FindByMemberAccess("tcpclient.getstream") +
	All.FindByMemberAccess("tcplistener.acceptsocket") + 
	All.FindByMemberAccess("console.read*") +
	All.FindByName("request",false) + All.FindByName("page.request",false) + 
	All.FindByName("request.Cookies*",false) + 
	All.FindByName("request.Form",false) +
	All.FindByName("request.headers",false) +
	All.FindByName("request.params",false) +
	All.FindByName("request.QueryString*",false) +
	All.FindByName("request.rawurl",false) +
	All.FindByName("request.urlreferrer",false) +
	All.FindByName("request.url",false) +
	All.FindByName("request.clientcertificate",false) +
	All.FindByName("page.request.Cookies*",false) +
	All.FindByName("page.request.Form",false) +
	All.FindByName("page.request.headers",false) +
	All.FindByName("page.request.params",false) +
	All.FindByName("page.request.QueryString*",false) +
	All.FindByName("page.request.rawurl",false) +
	All.FindByName("page.request.urlReferrer",false) +
	All.FindByName("page.request.url",false) +
	All.FindByName("page.request.clientCertificate",false) + 
	
	All.FindByMemberAccess("request.url",false) + 
	All.FindByMemberAccess("request.Cookies*",false) + 
	All.FindByMemberAccess("request.Form*",false) + 
	All.FindByMemberAccess("request.params",false) + 
	All.FindByMemberAccess("request.QueryString*",false) + 
	All.FindByMemberAccess("request.rawurl",false) + 
	All.FindByMemberAccess("request.urlreferrer",false) + 
	All.FindByMemberAccess("request.url",false) + 
	All.FindByMemberAccess("request.binaryread",false) + 	
	All.FindByMemberAccess("request.item",false) + 
	All.FindByMemberAccess("request.userlanguages",false) + 
	All.FindByMemberAccess("request.headers",false) +
	All.FindByMemberAccess("commandeventargs.commandargument",false) +

	All.FindByMemberAccess("request.url",false).GetMembersOfTarget() + 
	All.FindByMemberAccess("request.Cookies*",false).GetMembersOfTarget() + 
	All.FindByMemberAccess("request.Form*",false).GetMembersOfTarget() + 
	All.FindByMemberAccess("request.params",false).GetMembersOfTarget() + 
	All.FindByMemberAccess("request.QueryString*",false).GetMembersOfTarget() + 
	All.FindByMemberAccess("request.rawurl",false).GetMembersOfTarget() + 
	All.FindByMemberAccess("request.urlreferrer",false).GetMembersOfTarget() + 
	All.FindByMemberAccess("request.url",false).GetMembersOfTarget() + 
	All.FindByMemberAccess("request.binaryread",false).GetMembersOfTarget() + 	
	All.FindByMemberAccess("request.item",false).GetMembersOfTarget() + 
	All.FindByMemberAccess("request.userlanguages",false).GetMembersOfTarget() + 
	All.FindByMemberAccess("request.headers",false).GetMembersOfTarget() +
	All.FindByMemberAccess("commandeventargs.commandArgument",false).GetMembersOfTarget();


result = inputs - inputs.FindByAssignmentSide(CxList.AssignmentSide.Left) - Not_Interactive_Inputs() -
	All.FindByMemberAccess("request.*").GetTargetOfMembers();

// Host cannot be controlled by the user
result -= result.FindByMemberAccess("request.url.host");

CxList ServerVariables = Find_ServerVariables_Input();
result.Add(ServerVariables);