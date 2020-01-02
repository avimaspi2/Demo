/// <summary>
/// This query searches for app and http.server that are not configured with the express.csrf or csurf protection for XSRF.
/// </summary>
CxList declarators = Find_Declarators();
CxList unknown = Find_UnknownReference();
CxList vulnerable = All.NewCxList();
CxList vulnerableOld = All.NewCxList();
CxList vulnerableNew = All.NewCxList();
CxList methods = Find_Methods();

CxList parameters = Find_Parameters();

// Find references of express objects/function: var express = require("express");
CxList express = Find_Require("express", 1);
CxList expressMembers = (unknown * express).GetMembersOfTarget();

CxList expressCSRF = expressMembers.FindByMemberAccess("*.csrf");

// Find referemces of app objects: var express = require("express"); var app = express();
CxList app = Find_Require("express", 2);
CxList appRef = unknown * app;
// Find var csrf = require("csurf"); and var csrfProtection = csrf();
CxList csrfProtection = Find_Require("csurf", 2);
// Find app.use(express.csrf()); and var csrf = require("csurf"); app.use(csrf);
CxList appUse = appRef.GetMembersOfTarget().FindByMemberAccess("*.use");

CxList useExpressCSRF = expressCSRF.GetParameters(appUse);
CxList appUseCSRF = appUse.FindByParameters(useExpressCSRF).GetTargetOfMembers();
appUseCSRF.Add(appUse.FindByParameters(useExpressCSRF.GetFathers() * parameters).GetTargetOfMembers());
CxList safeAppUse = appUse.FindByParameters(csrfProtection);
safeAppUse.Add(appUse.FindByParameters(csrfProtection.GetFathers() * parameters));
appUseCSRF.Add(safeAppUse.GetTargetOfMembers());
vulnerableOld.Add(All.FindDefinition((appRef - appRef.FindAllReferences(appUseCSRF)) - express));

// Find cases where the router is initialized before the csrf protection
//   which can prevents validating the document_csrf token correctly
CxList expressRouter = expressMembers.FindByMemberAccess("*.Router");
CxList useExpressRouter = expressRouter.GetParameters(appUse);
useExpressRouter.Add(appRef.GetMembersOfTarget().FindByMemberAccess("*.router"));
useExpressRouter.Add(unknown.FindAllReferences(expressRouter.GetAssignee()));
CxList appUseRouter = appUse.FindByParameters(useExpressRouter).GetTargetOfMembers();
appUseRouter.Add(appUse.FindByParameters(useExpressRouter.GetFathers() * parameters).GetTargetOfMembers());
CxList influenced = appUseRouter.DataInfluencingOn(appUseCSRF);
// Remove router from the results (we are interessed in its app instead)
vulnerableOld -= declarators.FindDefinition(useExpressRouter);
vulnerableOld.Add(declarators.FindDefinition(influenced));

// Find app methods recieving the csurf object as a parameter, so it could be used to craate and validate tokens
string[] HTTPMethods = {"all", "checkout", "connect", "copy", "delete", "get", "head", "lock", "merge", "mkactivity",
	"mkcol", "move", "m - search", "notify", "options", "patch", "post", "propfind", "proppatch", "purge",
	"put", "report", "search", "subscribe", "trace", "unlock", "unsubscribe"};
CxList appsMethods = appRef.GetMembersOfTarget().FindByShortNames(new List<string>(HTTPMethods));
CxList safeMethods = appsMethods.FindByParameters(csrfProtection);
safeMethods.Add(appsMethods.FindByParameters(csrfProtection.GetFathers() * parameters));
// If all methods of the app get the csurf object as parameter - the app is using csurf.
CxList safeAppRef = appRef.FindAllReferences(appUseCSRF);
safeAppRef.Add((appsMethods - safeMethods).GetTargetOfMembers());
safeAppRef.Add(express);
CxList vul = appRef - safeAppRef;

vulnerableNew.Add(All.FindDefinition(vul));

// Find router usages, and and add the respective app
CxList useWithRouters = appUse.FindByParameters(unknown.FindAllReferences(vulnerableNew));
if (useWithRouters.Count > 0) {
	vulnerableNew.Add(All.FindDefinition(useWithRouters.GetTargetOfMembers()));	
}

// Return only app(s) that do not use csrf or csurf
vulnerable.Add(vulnerableOld * vulnerableNew);
result.Add(vulnerable);

// Find http(s) server that is created using an Express app without csrf/csurf protection
CxList vulnerableApp = vulnerable.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
CxList vulnerableAppRef = appRef.FindAllReferences(vulnerableApp);

CxList http = Find_Require("http", 1);
http.Add(Find_Require("https", 1));
CxList httpMembers = http.GetMembersOfTarget();
List<string> httpMethods = new List<string>{"Server", "createServer"};
CxList server = httpMembers.FindByShortNames(httpMethods);

//Find Server or createServer in var x = require("http").[Server, createServer]
CxList httpRequireMembers = methods.GetMembersOfTarget();
server.Add(httpRequireMembers.FindByShortNames(httpMethods));

CxList serverUsingExpress = server.FindByParameters(appRef - vulnerableAppRef); 
// Find: http.Server() and http.CreateServer(app) where app is not using csrf/csurf
CxList vulnerableServer = server - serverUsingExpress;
result.Add(vulnerableServer);