CxList methodInvoke = Find_Methods();
CxList onlyParams = Find_Parameters();
CxList onlyParamDecl = Find_ParamDecl();
CxList uRef = Find_UnknownReference();
CxList mA = Find_MemberAccesses();
CxList urMA = All.NewCxList();
urMA.Add(uRef);
urMA.Add(mA);

CxList allParams = onlyParams.Clone();
allParams.Add(onlyParamDecl);

CxList inLeftOfAssign = Find_Assign_Lefts();

CxList output = All.NewCxList();

CxList requiresHttp = Find_Require("http");
requiresHttp.Add(Find_Require("https"));
requiresHttp.Add(Find_Require("net"));
requiresHttp.Add(Find_Require("tls"));
requiresHttp.Add(Find_Require("dgram"));

CxList allInfluByRequireLibs = urMA.DataInfluencedBy(requiresHttp).GetMembersOfTarget();
CxList RefsMethods = allInfluByRequireLibs * methodInvoke;
//*************************************************************************
//support for connect/upgrade
//req.on('connect', function(res, socket, head) "socket" is a socket between client and server

CxList allOnMethods = RefsMethods.FindByMemberAccess("*.on");
allOnMethods.Add(RefsMethods.FindByMemberAccess("*.addListener"));
allOnMethods.Add(RefsMethods.FindByMemberAccess("*.once"));

CxList onMethodFirstParam = onlyParams.GetParameters(allOnMethods, 0);

CxList onConnectUpgradeParams = onMethodFirstParam.FindByShortNames(
	new List<string> {
		"\"connect\"",
		"\"upgrade\""
	}
);

CxList ConnectUpgradeOnMeth = onConnectUpgradeParams.GetAncOfType(typeof (MethodInvokeExpr)) * allOnMethods;

CxList requestCallback = onlyParams.GetParameters(ConnectUpgradeOnMeth, 1);

//Get All second parameters of xxx.on callback function as MethodDecl
CxList recCallAsMD = NodeJS_Get_CallBack_Method(requestCallback);

CxList connectUpgradeSocket = allParams.GetParameters(recCallAsMD, 1);
connectUpgradeSocket = urMA.FindAllReferences(connectUpgradeSocket);
RefsMethods.Add(connectUpgradeSocket.GetMembersOfTarget() * methodInvoke);

//****************************** end of connect/upgrade  ***********************************
//support for createServer 
CxList methodsInfluByRequireLibs = methodInvoke.DataInfluencedBy(requiresHttp);
//All methodInvoke influenced by required liblrary
CxList methInvFromRequireLibs = methodsInfluByRequireLibs * methodInvoke;
CxList createServer = methInvFromRequireLibs.FindByShortName("createServer");
//callback function can be first or second parameter of createServer function
requestCallback = onlyParams.GetParameters(createServer, 0);
requestCallback.Add(onlyParams.GetParameters(createServer, 1));

//support for on
/*
var server = http.createServer();
server.on('request', function(request, response) {});
*/
CxList onMethod = methInvFromRequireLibs.FindByShortName("on");
requestCallback.Add(onlyParams.GetParameters(onMethod, 0));
requestCallback.Add(onlyParams.GetParameters(onMethod, 1));

recCallAsMD = NodeJS_Get_CallBack_Method(requestCallback);	//get callback function as MethodDecl

//can be 0 or 2 parameters.
CxList callBackParams = allParams.GetParameters(recCallAsMD, 0);
callBackParams.Add(allParams.GetParameters(recCallAsMD, 1));

callBackParams = urMA.FindAllReferences(callBackParams);

CxList methodDefinition = All.NewCxList();
methodDefinition.Add(Find_MethodDecls(), Find_LambdaExpr(), Find_Declarators(), urMA);

CxList methodInvOfCallbackParams = methodInvoke.FindByParameters(callBackParams);
//method declaration
CxList defsOfInvoke = methodDefinition.FindDefinition(methodInvOfCallbackParams);
//lambda declaration
defsOfInvoke.Add(uRef.FindAllReferences(defsOfInvoke).GetAssigner());
CxList invokeParameters = allParams.GetParameters(defsOfInvoke);
CxList paramRefs = urMA.FindAllReferences(invokeParameters);

RefsMethods.Add(callBackParams.GetMembersOfTarget() * methodInvoke);
RefsMethods.Add(paramRefs.GetMembersOfTarget() * methodInvoke);

//****************************** end of createServer  ***********************************

// 1. write method parameters besides fs.write methods
CxList writeMethods = RefsMethods.FindByMemberAccess("*.write") - NodeJS_Find_Write();
writeMethods.Add(RefsMethods.FindByMemberAccess("*.end"));

output.Add(writeMethods);
////////////////////////////////////////////////////////////////////////////////////////////////////////-

// 2. send method for udp sockets
CxList createSocketDgram = methodInvoke.FindByMemberAccess("*.createSocket");
CxList dGramSocket = inLeftOfAssign.GetByAncs(createSocketDgram.GetAncOfType(typeof(AssignExpr)));
CxList allSockets = All.FindAllReferences(dGramSocket);
CxList allSocketsMembers = allSockets.GetMembersOfTarget() * methodInvoke;
CxList socketSend = allSocketsMembers.FindByShortName("send");
output.Add(socketSend);
////////////////////////////////////////////////////////////////////////////////////////////////////////-
//CxList httpWrites = methInvFromRequireHttp.FindByShortName("writeHead");

result = output;

CxList res = All.FindByShortNames(new List<string>{"res", "response"});

List<string> names = new List<string>(new string[]{"redirect", "send", "cookie", "locals", "param", "params", "setHeader",
	"download", "location", "sendFile", "write", "writeHead", "statusMessage", "addTrailers", "render" });
CxList temp = res.Clone();
CxList found;
CxList post = All.NewCxList();

for(int i = 0; i < 10 && temp.Count > 0; i++)
{              
	temp = temp.GetMembersOfTarget();
	found = temp.FindByShortNames(names);
	post.Add(found);
	temp -= found;
}

// Add response mapped by Express: 'res' in app.get(path , function(req, res...) {})'
CxList expressCallbak = NodeJS_Find_Express_Callback_Params(res);
post.Add(uRef.FindAllReferences(expressCallbak));
CxList potential = All.NewCxList();
CxList pMembers = All.NewCxList();
foreach(CxList p in post)
{
	potential = p.Clone();
	for(int j = 0; j < 10 && potential.Count > 0; j++)
	{
		pMembers = potential.GetMembersOfTarget();
		if(pMembers.Count == 0)
		{
			if (( potential.FindByType(typeof(MethodInvokeExpr)).Count > 0) || (potential.FindByAssignmentSide(CxList.AssignmentSide.Left).Count > 0) )
			{
				result.Add(potential);	// only if the member is a method or a setter - add it
			}
			break;
		}
		potential = pMembers;
	}
}
result -= result.FindByShortName("redirect");

// SWIG support
CxList swigOutputs = NodeJS_Find_Swig_Interactive_Outputs();
if(swigOutputs.Count > 0)
{
	result -= result.FindByShortName("render");
	result.Add(swigOutputs);
}
result.Add(Find_Framework_Outputs());
if(Hapi_Find_Server_Instance().Count > 0)
{
	result.Add(Hapi_Find_Outputs());	
}

CxList paramResult = result.FindByType(typeof(Param));
result -= paramResult;
result.Add(All.FindByFathers(paramResult));

result -= XS_Find_All();