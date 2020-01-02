CxList methods = Find_Methods();

CxList httpRequires = Find_Require("http");

List<string> serverMethods = new List<string>{"Server", "createServer"};
result = httpRequires.GetMembersOfTarget().FindByShortNames(serverMethods);

//Find Server or createServer in var x = require("http").[Server, createServer]
CxList httpRequireMembers = methods.GetMembersOfTarget();
result.Add(httpRequireMembers.FindByShortNames(serverMethods));