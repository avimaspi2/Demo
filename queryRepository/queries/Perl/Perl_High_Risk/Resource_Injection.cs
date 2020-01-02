//Find Methods
CxList methods = Find_Methods();

//Find socket creation
CxList sockets = methods.FindByShortName("socket");
sockets.Add(All.FindByShortName("IO::Socket*"));
sockets.Add(All.FindByName("IO.Socket"));

//Find method calls
CxList socket_vars = methods;
//Find vars that are sockets
socket_vars.Add(All.FindByType(typeof(Declarator)));
//Find elements influenced by sockets
socket_vars = socket_vars.InfluencedBy(sockets);

//Find inputs
CxList inputs = Find_Interactive_Inputs();

//Find sanitizers
CxList sanitizers = Find_General_Sanitize();

//Find sockets influenced by inputs
result = socket_vars.InfluencedByAndNotSanitized(inputs, sanitizers);