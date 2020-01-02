CxList cout = All.FindByShortName("cout");

CxList methods = Find_Methods();
CxList exp = cout.GetAncOfType(typeof(BinaryExpr));
result.Add(All.FindByFathers(exp) - cout);

result.Add(methods.FindByShortName("perror")); 
result.Add(methods.FindByShortName("printf"));
result.Add(methods.FindByShortName("putc")); 
result.Add(methods.FindByShortName("puts"));
result.Add(methods.FindByShortName("putchar"));
result.Add(methods.FindByShortName("vprintf"));
result.Add(methods.FindByShortName("setdlgtext"));
result.Add(All.GetParameters(methods.FindByShortName("setdlgtext"), 1)); 

result.Add(methods.FindByMemberAccess("ostream.put")); 
result.Add(methods.FindByMemberAccess("ostream.write"));
result.Add(methods.FindByMemberAccess("streambuf.sputc"));
result.Add(methods.FindByMemberAccess("streambuf.sputs"));

// Swift
result.Add(methods.FindByShortName("print"));
result.Add(methods.FindByShortName("println"));
result.Add(methods.FindByShortName("print:"));
result.Add(methods.FindByShortName("println:"));
result.Add(methods.FindByShortName("debugPrint:separator:terminator:*"));

result.Add(All.FindByShortName("stdout"));

result.Add(All.GetParameters(methods.FindByMemberAccess("CEdit.Set*"), 1));
result.Add(All.GetParameters(methods.FindByMemberAccess("CRichEditCtrl.Set*"), 1));
result.Add(All.GetParameters(methods.FindByMemberAccess("CComboBox.Set*"), 1));
result.Add(All.GetParameters(methods.FindByShortName("SetWindowText*"), 1));

result.Add(methods.FindByMemberAccess("CFileException.ReportError"));

// Add the SendMessage/PostMessage things when type is WM_SETTEXT
CxList sendMessage = methods.FindByShortName("SendMessage");
sendMessage.Add(methods.FindByShortName("SendMessageCallback"));
sendMessage.Add(methods.FindByShortName("SendNotifyMessage"));
sendMessage.Add(methods.FindByShortName("PostMessage"));
sendMessage.Add(methods.FindByShortName("PostThreadMessage"));

CxList sendMessageParams = All.GetParameters(sendMessage);
CxList sendMessageSet = sendMessageParams.FindByType("WM_SETTEXT");
sendMessage = sendMessage.FindByParameters(sendMessageSet);

result.Add(sendMessageParams.GetParameters(sendMessage, 2));
result.Add(sendMessageParams.GetParameters(sendMessage, 3));

result.Add(Find_Interactive_Outputs_User());
result.Add(Find_Web_Outputs());