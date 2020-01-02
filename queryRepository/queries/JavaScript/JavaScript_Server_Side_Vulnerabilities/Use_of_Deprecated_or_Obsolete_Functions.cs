CxList objectCreations = Find_ObjectCreations();
CxList members = Find_MemberAccesses();
CxList unknReferences = Find_UnknownReference();
/*
find all connections methods of Net library and all createClient methods of http library
*/
CxList methodInvoke = Find_Methods() - XS_Find_All();

CxList httpLibsRequires = Find_Require("http");
httpLibsRequires.Add(Find_Require("net"));

CxList allInfluByRequire = methodInvoke.DataInfluencedBy(httpLibsRequires);
CxList methInvFromRequire = allInfluByRequire * methodInvoke;

CxList depMeth = methInvFromRequire.FindByShortNames(new List<string> {"connections", "createClient"});

// Support for connect library methods
CxList connectRequire = Find_Require("connect");

CxList allInfluByRC = methodInvoke.DataInfluencedBy(connectRequire);
//All methodInvoke influenced by required library
CxList methInvFromRC = allInfluByRC * methodInvoke;

CxList depMethC = methInvFromRC.FindByShortNames(
		new List<string> {"staticCache", "cookieParser", "limit", "multipart", "query"});

CxList depMethD = Find_Require("domain");

// Express.js deprecations
CxList allOfExpress = Find_Require("express", 2);
CxList membersFromExpress = allOfExpress.GetMembersOfTarget();
CxList depInExpress = membersFromExpress.FindByShortNames(new List<string>{"createServer", "configure"});
CxList expressCbParams = NodeJS_Find_Express_Callback_Params();

CxList exprCBMembers = unknReferences.FindAllReferences(expressCbParams).GetMembersOfTarget();
// Add deprecated methods
CxList deprecatedExpressMethods = exprCBMembers.FindByType(typeof(MethodInvokeExpr))
		.FindByShortNames(new List<string>{"accepted", "sendfile", "local", "locals"}, true);
depInExpress.Add(deprecatedExpressMethods);
// Add deprecated members
depInExpress.Add(exprCBMembers.FindByType(typeof(MemberAccess))
		.FindByShortNames(new List<string> {"charset", "headerSent"}));

// Other deprecated libraries
CxList reqOfDeprecatedLibs = Find_Require("sendgrid-nodejs");
reqOfDeprecatedLibs.Add(Find_Require("websocket-server"));
reqOfDeprecatedLibs.Add(Find_Require("votizen-logger"));
reqOfDeprecatedLibs.Add(Find_Require("pusher-pipe"));
reqOfDeprecatedLibs.Add(Find_Require("facebook-realtime-graph"));
reqOfDeprecatedLibs.Add(Find_Require("facebook-graph-client"));


// Support for deprecated crypto libs
//      https://developer.mozilla.org/en-US/docs/Archive/Mozilla/JavaScript_crypto/generateCRMFRequest
result.Add(Find_Members("crypto.generateCRMFRequest").FindByType(typeof(MethodInvokeExpr)));

// Deprecated in NodeJS V6
CxList deprecatedIn6 = All.NewCxList();
CxList utilRequire = Find_Require("util", 1).GetMembersOfTarget();

// Add SlowBuffer and Buffer method calls/constructors
List<string> bufferMethods = new List<string> {"SlowBuffer", "Buffer"};
deprecatedIn6.Add(objectCreations.FindByShortNames(bufferMethods));
deprecatedIn6.Add(methodInvoke.FindByShortNames(bufferMethods));

// _extend and log methods from "util"
deprecatedIn6.Add(utilRequire.FindByShortNames(new List<string>{"_extend", "log"}));

// Deprecated in NodeJS 8.0.0
// Usage of fs.read without buffer (ie string in callback) is deprecated
//    old usage uses 5 parameters instead of 6
CxList deprecatedIn8 = All.NewCxList();
CxList fsRequire = Find_Require("fs", 1).GetMembersOfTarget();
CxList fsReads = fsRequire.FindByShortName("read").FindByType(typeof(MethodInvokeExpr));
foreach (CxList method in fsReads) {
	MethodInvokeExpr methodObj = method.TryGetCSharpGraph<MethodInvokeExpr>();
	if (methodObj.Parameters.Count < 6) {
		deprecatedIn8.Add(method);	
	}
}

// SyncWriteStream from fs was deprecated
deprecatedIn8.Add(objectCreations.FindByShortName("SyncWriteStream"));

// Deprecated in NodeJS v10
CxList deprecatedIn10 = All.NewCxList();
CxList cryptoRequire = Find_Require("crypto", 1).GetMembersOfTarget();
// createCypher and createDecipher methods
deprecatedIn10.Add(cryptoRequire.FindByShortName("createCipher"));
CxList decipher = cryptoRequire.FindByShortName("createDecipher");
deprecatedIn10.Add(decipher);

// fields DEFAULT_ENCODING and fips from "crypto"
deprecatedIn10.Add(cryptoRequire.FindByShortNames(new List<string> {"DEFAULT_ENCODING", "fips"}));

// decipher.finaltol method is deprecated
CxList allDecipher = cryptoRequire.FindByShortName("createDecipheriv");
allDecipher.Add(decipher);

CxList finalToLMethods = methodInvoke.FindByShortName("finaltol").DataInfluencedBy(allDecipher);
deprecatedIn10.Add(finalToLMethods.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly));

// assert.fail() with more than one argument is deprecated
CxList failAsserts = methodInvoke.FindByMemberAccess("assert.fail");
CxList assertsWithMoreThanOneParam = All.GetParameters(failAsserts, 1);
deprecatedIn10.Add(assertsWithMoreThanOneParam.GetAncOfType(typeof(MethodInvokeExpr)));

// process.assert() is deprecated
deprecatedIn10.Add(methodInvoke.FindByMemberAccess("process.assert"));

// Timer.enroll and Timer.unenroll are deprecated
CxList timersRequire = Find_Require("timers", 1).GetMembersOfTarget();
deprecatedIn10.Add(timersRequire.FindByShortNames(new List<string>{"enroll", "unenroll"}));

// Providing custom inspect method using util.inspect.inspect is deprecated
CxList inspectMemberOfInspect = utilRequire.GetMembersOfTarget().FindByShortName("inspect");
deprecatedIn10.Add(inspectMemberOfInspect.FindByAssignmentSide(CxList.AssignmentSide.Left));

// Setting process.env with a object value is deprecated
CxList processEnvMembers = members.FindByMemberAccess("process.env");
CxList processEnvAssigners = processEnvMembers.GetFathers().FindByType(typeof(IndexerRef)).GetAssigner();
// add member access (ie process.env.a = "")
processEnvAssigners.Add(processEnvMembers.GetAssigner());
deprecatedIn10.Add(processEnvAssigners.FindByAbstractValue(x => x is ObjectAbstractValue));

//////////////////////////////////////////////////////////////////////////////////////////////////-
result.Add(depMeth);
result.Add(depMethC);
result.Add(depMethD);
result.Add(depInExpress);
result.Add(reqOfDeprecatedLibs);
result.Add(deprecatedIn6);
result.Add(deprecatedIn8);
result.Add(deprecatedIn10);
////////////////