/// <summary>
///  Enables/disables the returning of more detailed error messages to the
///  client in the error response
///  Should be disabled for browser-facing APIs due to the risk of XSS attacks
///  and (probably) enabled for internal or non-browser APIs
/// </summary>

CxList conf = Find_Hocon_Application_Conf();
//Get enabled "verbose-error-messages" flags from config files.
CxList verboseFlagValue = conf.FindByShortName("verbose-error-messages").GetAssigner().FindByShortName("on");
if(verboseFlagValue.Count > 0){
	//Assume the project being scanned has browser-facing APIs if the following import is used.
	CxList webAppImports = All.FindByRegex("akka.http.scaladsl.server.Directives");
	if(webAppImports.Count > 0)result = verboseFlagValue;
}