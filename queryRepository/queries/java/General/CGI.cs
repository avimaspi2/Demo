// This query determines if the program is a CGI application. We can conclude it is a CGI application if:
// Library cgi is used 
//		Import htpi.cgi.*;
CxList cgiApp = base.Find_Import().FindByShortName("cgi");

/*	The getProperty method accesses to CGI environment variable "query_string" 	      
    Properties p = System.getProperties();
    String query = p.getProperty("QUERY_STRING");
	or 
	System.getProperty("cgi.query_string")
*/
CxList strings = Find_Strings();
CxList methods = Find_Methods();
CxList getPropertyMethods = methods.FindByMemberAccess("System.getProperty");
getPropertyMethods.Add(methods.FindByMemberAccess("Properties.getProperty"));
CxList queryString = strings.GetParameters(getPropertyMethods)
							.FindByShortNames(new List <string> {"cgi.*","QUERY_STRING"});

/* 	There is a line printing "Content-type:text/html\r\n\r\n". This line is sent back to the browser and specify 
	the content type to be displayed on the browser screen (in the example is html but can be another extension)
		cout << "Content-type:text/html\r\n\r\n";	*/
CxList contentType = strings.FindByShortName("Content-type*", false);

CxList scriptEngine = methods.FindByMemberAccess("ScriptEngineManager.getEngineByName");

CxList getenvMethods = methods.FindByMemberAccess("System.getenv");
result = All.NewCxList();

// The use of System.getenv() method proves that is not a CGI application
if(getenvMethods.Count == 0) {
	result.Add(cgiApp);
	result.Add(queryString);
	result.Add(contentType);
	result.Add(scriptEngine);
}