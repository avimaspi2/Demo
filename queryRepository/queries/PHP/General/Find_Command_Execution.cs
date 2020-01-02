CxList methods = Find_Methods();

CxList mail = methods.FindByShortName("mail");
CxList mbSendMail = methods.FindByShortName("mb_send_mail");
CxList mailSendingMethods = mail;
mailSendingMethods.Add(mbSendMail);

List < string > methodsStrings = 
	new List<string>(new string[] 
	{"system", 
	"passthru", 
	"exec", 
	"shell_exec", 
	"popen", 
	"proc_open",
	"pcntl_exec", 
	"expect_popen", 
	"w32api_invoke_function", 
	"w32api_register_function", 
	"$_ShellCommand"}); //Handles the case of backticks:

result = methods.FindByShortNames(methodsStrings);
result.Add(mailSendingMethods);
result.Add(All.GetParameters(mailSendingMethods, 4));

result.Add(methods.FindByMemberAccess("PHP_Shell.*"));