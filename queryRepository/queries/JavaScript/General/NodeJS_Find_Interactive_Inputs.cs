CxList methodInvoke = Find_Methods();
CxList onlyParams = Find_Param();
CxList onlyParamDecl = Find_ParamDecl();
CxList uRef = Find_UnknownReference();
CxList memAcc = Find_MemberAccesses();
CxList objCreate = Find_ObjectCreations();
CxList urMA = All.NewCxList();
urMA.Add(uRef);
urMA.Add(memAcc);

CxList allParams = onlyParams.Clone();
allParams.Add(onlyParamDecl);

CxList inLeftOfAssign = Find_Assign_Lefts();

CxList input = All.NewCxList();

CxList requires = Find_Require("http");
requires.Add(Find_Require("https"));
requires.Add(Find_Require("restify"));
requires.Add(Find_Require("net"));

CxList allInfluByRequireHttp = methodInvoke.DataInfluencedBy(requires);
CxList methInvFromRequireHttp = allInfluByRequireHttp * methodInvoke;
////////////////////////////////////////////////////////////////////////////////////////////////////

/*
	1. support for http.request/https.request and http.get/https.get methods
*/
//slight difference between request and get methods
CxList httpRequest = methInvFromRequireHttp.FindByShortNames(
	new List<string> {
		"request",
		"get"
		}
	);

/*
	2. support for http.createServer and https.createServer and net.createServer methods 
*/
CxList httpCreateServer = methInvFromRequireHttp.FindByShortName("createServer");
CxList httpsCreateServer = methInvFromRequireHttp.FindByShortName("createServer");


////////////////////////////////////////////////////////////////////////////////////////////////////////-
//in case second (for some methods first) parameter exists it is a method callBack function, parameter of this method is an input
CxList recCallAsMD = All.NewCxList();
CxList requestCallback = onlyParams.GetParameters(httpRequest, 1);
requestCallback.Add(onlyParams.GetParameters(httpsCreateServer, 1));
requestCallback.Add(onlyParams.GetParameters(httpCreateServer, 0));
	 //requestCallback.Add(onlyParams.GetParameters(dataLineOnMeth, 1));
	
//Get All second parameters of http.request (https.request) callback function as MethodDecl
recCallAsMD = NodeJS_Get_CallBack_Method(requestCallback);

//the input is first parameter of callbadk method
input.Add(allParams.GetParameters(recCallAsMD, 0));
////////////////////////////////////////////////////////////////////////////////////////////////////////-

/*
	3. get all xxx.on('data', function(input)) or xxx.on('line', function(input))
		"input" parameter is an input.
*/

CxList requireOfLibs = Find_Require("http");
requireOfLibs.Add(Find_Require("https"));
requireOfLibs.Add(Find_Require("net"));
requireOfLibs.Add(Find_Require("tls"));
requireOfLibs.Add(Find_Require("dgram"));

CxList getReadableStreamSomehow = methodInvoke.FindByShortName("getReadableStreamSomehow");
	
//get httpVar of ==> var httpVar = require('http')		(as well for https)

CxList leftOfLibsRequire = inLeftOfAssign.GetByAncs(getReadableStreamSomehow);
leftOfLibsRequire.Add(requireOfLibs);
CxList allLibs = All.FindAllReferences(leftOfLibsRequire);
CxList allInfluByRequireLibs = urMA.DataInfluencedBy(allLibs);
//All unknownRef of memberAccess influenced by required liblrary
allInfluByRequireLibs = allInfluByRequireLibs * urMA;

CxList allInputRefs = input.Clone();
allInputRefs.Add(urMA.FindAllReferences(input));
allInputRefs.Add(urMA.FindAllReferences(allInfluByRequireLibs));

CxList inputsRefsMethods = allInputRefs.GetMembersOfTarget() * methodInvoke;
//the input is first parameter of callbadk method
input.Add(allParams.GetParameters(recCallAsMD, 0));

CxList allOnMethods = inputsRefsMethods.FindByMemberAccess("*.on");
allOnMethods.Add(inputsRefsMethods.FindByMemberAccess("*.addListener"));
allOnMethods.Add(inputsRefsMethods.FindByMemberAccess("*.once"));

CxList onMethodFirstParam = onlyParams.GetParameters(allOnMethods, 0);

CxList onConnectUpgradeParams = onMethodFirstParam.FindByShortNames(
	new List<string> {
		"\"connect\"",
		"\"upgrade\""
		}
	);

	
CxList ConnectUpgradeOnMeth = onConnectUpgradeParams.GetAncOfType(typeof (MethodInvokeExpr)) * allOnMethods;

requestCallback = onlyParams.GetParameters(ConnectUpgradeOnMeth, 1);

//Get All second parameters of xxx.on callback function as MethodDecl
recCallAsMD = NodeJS_Get_CallBack_Method(requestCallback);

input.Add(allParams.GetParameters(recCallAsMD, 0));
CxList connectUpgradeSocket = allParams.GetParameters(recCallAsMD, 1);
CxList conUpgSocketRefs = urMA.FindAllReferences(connectUpgradeSocket);
conUpgSocketRefs = conUpgSocketRefs.GetMembersOfTarget() * methodInvoke;

//get .on .addListener and .once methods for socket of connect/upgrade callback
CxList aditionalOnMethods = conUpgSocketRefs.FindByMemberAccess("*.on");
aditionalOnMethods.Add(conUpgSocketRefs.FindByMemberAccess("*.addListener"));
aditionalOnMethods.Add(conUpgSocketRefs.FindByMemberAccess("*.once"));


onMethodFirstParam.Add(onlyParams.GetParameters(aditionalOnMethods, 0));
allOnMethods.Add(aditionalOnMethods);
CxList onDataLineParams = onMethodFirstParam.FindByShortNames(
	new List<string> {
		"\"data\"",
		"\"line\"",
		"\"message\""
		}
	);

CxList dataLineOnMeth = onDataLineParams.GetAncOfType(typeof (MethodInvokeExpr)) * allOnMethods;

requestCallback = onlyParams.GetParameters(dataLineOnMeth, 1);

//Get All second parameters of xxx.on callback function as MethodDecl
recCallAsMD = NodeJS_Get_CallBack_Method(requestCallback);
input.Add(allParams.GetParameters(recCallAsMD, 0));

//**********************************************************************************
/*
	4. read method  - xxx.read([size]), read(0) will not consume any bytes
		only read methods without parameters or with 1 parameter
*/
CxList readable = memAcc.FindByShortName("Readable", false);
CxList leftOfReadableRequire = inLeftOfAssign.GetByAncs(readable.GetAncOfType(typeof(AssignExpr)));
CxList allReadable = All.FindAllReferences(leftOfReadableRequire);
allReadable.Add(objCreate.FindByShortName(allReadable));
CxList allInfluByRequireReadable = urMA.DataInfluencedBy(allReadable);

//All unknownRef of memberAccess influenced by required liblrary
allInfluByRequireReadable = allInfluByRequireReadable * urMA;
allInfluByRequireReadable.Add(allReadable);

allInfluByRequireReadable = urMA.FindAllReferences(allInfluByRequireReadable);
CxList inputsReadableMethods = allInfluByRequireReadable.GetMembersOfTarget() * methodInvoke;

CxList _methds2 = All.NewCxList();
_methds2.Add(inputsRefsMethods);
_methds2.Add(inputsReadableMethods);

CxList allReadMethods = _methds2.FindByMemberAccess("*.read");

CxList readSParam = onlyParams.GetParameters(allReadMethods, 1);
//remove all read methods with more then 1 parameter
allReadMethods -= readSParam.GetAncOfType(typeof (MethodInvokeExpr)) * allReadMethods;
CxList readFParam = onlyParams.GetParameters(allReadMethods, 0);
CxList readFParamZerro = readFParam.FindByShortName("0");
//remove all read methods with size parameter = 0
allReadMethods -= readFParamZerro.GetAncOfType(typeof (MethodInvokeExpr)) * allReadMethods;
input.Add(allReadMethods);
////////////////////////////////////////////////////////////////////////////////////////////////////////-

result = input;

// Find req/request using heuristics
CxList req = All.FindByShortNames(new List<string>{"req","request"});

//find fst param when snd is res/response but fst param is not req
//for situations like ({query}, res) => {... query.q ...}
CxList allLambdas = Find_LambdaExpr();
CxList allRelevantSecondParamFromLambdas = onlyParamDecl.GetParameters(allLambdas, 1).FindByShortNames(new List<string> {"res", "response"});
CxList allRelevantLambdas = allRelevantSecondParamFromLambdas.GetAncOfType(typeof(LambdaExpr));
CxList firstParamOfRelevantlambdas = onlyParamDecl.GetParameters(allRelevantLambdas, 0) - req;
CxList relevantVars = uRef.FindAllReferences(firstParamOfRelevantlambdas).GetMembersOfTarget().GetAssignee();

req.Add(uRef.FindAllReferences(firstParamOfRelevantlambdas));

List<string> methodNames = new List<string>(new string[]{"all","body","checkout","connect","cookie","copy","delete","get","head","hostname","ip","lock",
	"merge","mkactivity","mkcol","move","m-search","notify","options","originalUrl","param","params","path","patch","post","propfind","proppatch","purge",
	"put","query","report","search","signedCookies","subdomains","subscribe","trace","unlock","unsubscribe","use",
	"session", "getSession", "url", "baseUrl", "headers"});
CxList temp = req.Clone();

CxList found = All.NewCxList();
CxList post = All.NewCxList();

for(int i = 0; i < 10 && temp.Count > 0; i++)
{              
	temp = temp.GetMembersOfTarget();
	found = temp.FindByShortNames(methodNames);
	post.Add(found);
	temp -= found;
}

post.Add(uRef.FindAllReferences(relevantVars.FindByShortNames(methodNames)));

// Add request mapped by Express: 'req' in app.get(path , function(req, res...) {})'
CxList expressCallbak = NodeJS_Find_Express_Callback_Params(req);
post.Add(uRef.FindAllReferences(expressCallbak));
CxList pMembers = All.NewCxList();
foreach(CxList p in post)
{
	CxList potential = p.Clone();
	for(int j = 0; j < 10 && potential.Count > 0; j++)
	{
		pMembers = potential.GetMembersOfTarget();
		if(pMembers.Count == 0 && (potential.FindByAssignmentSide(CxList.AssignmentSide.Left).Count == 0))
		{
			result.Add(potential);
			break;
		}
		potential = pMembers;
	}
}
if(Hapi_Find_Server_Instance().Count > 0)
{
	result.Add(Hapi_Find_Inputs());
}
result -= XS_Find_All();