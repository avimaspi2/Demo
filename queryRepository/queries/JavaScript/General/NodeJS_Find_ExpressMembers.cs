// This query searches for express methods which receive callback as a paramterm and return the callback parameters.
// The result is filtered by the first parameter (CxList).
// Default filter = Find_ParamDecls();
CxList unknown = Find_UnknownReference();
CxList assignLeft = Find_Assign_Lefts();
// Find: 'express' in var express = require("express");
CxList express = Find_Require("express", 1);
// Find: 'router' in var router = express.Router();
CxList createRouter = express.GetMembersOfTarget().FindByShortName("Router");
CxList createRouterAssign = createRouter.FindByAssignmentSide(CxList.AssignmentSide.Right);
CxList routers = (createRouter - createRouterAssign);
routers.Add(assignLeft.FindByFathers(createRouterAssign.GetFathers()));
// Find: 'app' in var express = require("express"); var app = express();
CxList app = Find_Require("express", 2);
// Find: 'router' in var router = app.router;
CxList routerMethods = app.GetMembersOfTarget().FindByShortNames(new List<string>{"router", "route"});
routers.Add(routerMethods);
routers.Add(routers.GetAssignee());
routers.Add(unknown.FindAllReferences(routers));
CxList members = All.NewCxList();
CxList temp = routers.GetMembersOfTarget();
for (int i = 0; i < 10 && temp.Count > 0 ; i++)
{
	members.Add(temp);
	temp = temp.GetMembersOfTarget();
}

string[] HTTPMethods = {"all", "checkout", "connect", "copy", "delete", "get", "head", "lock",
	"merge", "mkactivity", "mkcol", "move", "m - search", "notify", "options", "patch", "post",
	"propfind", "proppatch", "purge", "put", "report", "search", "subscribe", "trace", "unlock",
	"unsubscribe"};

// Find: router.put(...) and app.put(...) which recieve a callbak as a parameter
CxList methods = members.FindByShortNames(new List<string>(HTTPMethods));
List<string> appMethods = new List<string>{"engine","listen","method","on", "param", "render", "use"};

appMethods.AddRange(HTTPMethods);
// Add app.param(...), app.render(...)
methods.Add(app.GetMembersOfTarget().FindByShortNames(appMethods));
result = methods;