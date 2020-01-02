CxList methods = Find_Methods();

result.Add(Find_Outputs_Right_Side());

result.Add(methods.FindByShortNames(
	new List<string> {
		"perror", 
		"printf", 
		"putc",
		"puts",
		"putchar", 
		"vprintf",
		"setdlgtext", 
		"stdout" }));

CxList contentSetters = methods.FindByMemberAccess("CEdit.Set*");
contentSetters.Add(methods.FindByMemberAccess("CRichEditCtrl.Set*"));
contentSetters.Add(methods.FindByMemberAccess("CComboBox.Set*"));
contentSetters.Add(methods.FindByShortNames(new List<string> { "SetWindowText*", "setdlgtext" }));

result.Add(All.GetParameters(contentSetters, 1));

result.Add(methods.FindByMemberAccess("CFileException.ReportError"));
result.Add(methods.FindByMemberAccess("ostream.put"));
result.Add(methods.FindByMemberAccess("ostream.write"));
result.Add(methods.FindByMemberAccess("streambuf.sput*"));

// Add the SendMessage/PostMessage things when type is WM_SETTEXT
CxList sendMessage = methods.FindByShortNames(
	new List<string> {
		"SendMessage",
		"SendMessageCallback",
		"SendNotifyMessage",
		"PostMessage", 
		"PostThreadMessage"});

CxList sendMessageParams = All.GetParameters(sendMessage);
CxList sendMessageSet = sendMessageParams.FindByType("WM_SETTEXT");
sendMessage = sendMessage.FindByParameters(sendMessageSet);

result.Add(sendMessageParams.GetParameters(sendMessage, 2));
result.Add(sendMessageParams.GetParameters(sendMessage, 3));
result.Add(Find_Outputs_MFC());