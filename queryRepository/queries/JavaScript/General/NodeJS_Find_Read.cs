/*
	Use of Find_Read requires NewAlgorithm
	1. support for fs.copyFile/copyFileSync
	2. support for fs.read and fs.readSync methods (second parameter of fs.read method is an input)
	3. support for fs.readFile method (second parameter of fs.read method is an input)
*/

/*
	Auxiliary method to find all references of a given reference after it
	Params:
		start - A CxList with the referecne we are looking for
		references - A CxList with possible references, that we want to filter
	Return:
		A CxList with the references that exists after the line pragma of "start"
*/
Func<CxList, CxList, CxList> FindReferencesAfter = (start, references) => {
	CxList res = All.NewCxList();
	if (references.Count == 0)
	{
		return res;	
	}
	try
	{
		CSharpGraph startGraph = start.GetFirstGraph();
		int startGraphLine = startGraph.LinePragma.Line;
		
		foreach(CxList refer in references)
		{
			CSharpGraph referGraph = refer.GetFirstGraph();
			int referGraphLine = referGraph.LinePragma.Line;
			if (referGraphLine > startGraphLine) res.Add(refer);
		}
	}
	catch(Exception e)
	{
		cxLog.WriteDebugMessage(e);	
	}
	return res;			
};


CxList methodInvoke = Find_Methods();
CxList allStrings = Find_String_Literal();
CxList onlyParams = Find_Parameters();
CxList onlyParamDecl = Find_ParamDecl();

CxList allParams = onlyParams.Clone();
allParams.Add(onlyParamDecl);

CxList uRef = Find_UnknownReference();
CxList mA = Find_MemberAccesses();

CxList urMA = All.NewCxList();
urMA.Add(uRef);
urMA.Add(mA);

CxList inLeftOfAssign = Find_Assign_Lefts();

CxList input = All.NewCxList();

CxList requireOfFS = Find_Require("fs");

CxList allInfluByRequireFS = methodInvoke.DataInfluencedBy(requireOfFS);
//All methodInvoke influenced by required library
CxList methInvFromRequireFS = allInfluByRequireFS * methodInvoke;
////////////////////////////////////////////////////////////////////////////////////////////////////

/*
	1. Support for fs.copyFile and fs.copyFileSync (the second parameter can be a buffer or a string, 
		and in the first case it is an input (the result of read the file).
	As we dont have flows between that method call and the parameter, add all the 
		references of the parameter used after
*/
CxList fsCopy = methInvFromRequireFS.FindByShortNames(new List<string>{"copyFile", "copyFileSync"});
CxList copySecParam = All.GetParameters(fsCopy, 1);

// Remove strings, as those are just a copy of a file from one place to another.
CxList paramAsStrings = copySecParam.FindByAbstractValue(absValue => absValue is StringAbstractValue);
paramAsStrings.Add(allStrings);
copySecParam = copySecParam - paramAsStrings;

CxList allInFather = uRef.GetByAncs(fsCopy.GetAncOfType(typeof(MethodDecl)));

foreach(CxList copyParam in copySecParam)
{
	CxList currRefs = allInFather.FindAllReferences(copyParam);
	input.Add(FindReferencesAfter(copyParam, currRefs));
}

/*
	2. support for fs.read and fs.readSync methods (second parameter of fs.read method is an input)
*/
CxList fsRead = methInvFromRequireFS.FindByShortNames(
	new List<string> {
		"read",
		"readSync"
		}
	);

CxList _params2 = urMA.Clone();
_params2.Add(onlyParams);
CxList fsInput = _params2.GetParameters(fsRead, 1);
CxList fatherMethod = fsInput.GetAncOfType(typeof (MethodDecl));
CxList allInFatherMethod = All.GetByAncs(fatherMethod);
CxList allInFaterMethodNotInLeft = allInFatherMethod - inLeftOfAssign;

//get allreferences of inputs in parent method that not in left and comes after the input 
CxList fsInList = All.NewCxList();
foreach(CxList fsIn in fsInput)
{
	CxList currRefs = allInFaterMethodNotInLeft.FindAllReferences(fsIn);
	CxList refsAfter = FindReferencesAfter(fsIn, currRefs);
	if (refsAfter.Count == 0) continue;
	foreach(CxList refAfter in refsAfter) {
		fsInList.Add(fsIn.ConcatenatePath(refAfter));
	}
}
input.Add(fsInList);
input.Add(fsInput);
////////////////////////////////////////////////////////////////////////////////////////////////////

//get fs.readFileSync(filename, [options])
input.Add(methInvFromRequireFS.FindByShortName("readFileSync"));
//get fs.createReadStream (returns a buffer stream for the file)
input.Add(methInvFromRequireFS.FindByShortName("createReadStream"));

/*
	3. support for fs.readFile method (nodeJS callback "function(x,data)")
*/
CxList fsReadFile = methInvFromRequireFS.FindByShortName("readFile");
//get fs callback method parameter of readFile method
CxList fsCallback = NodeJS_Get_CallBack_Method(allParams.GetParameters(fsReadFile));;	
//the input is second parameter of callbadk method
input.Add(allParams.GetParameters(fsCallback, 1));
////////////////////////////////////////////////////////////////////////////////////////////////////
//3. support for xxx.on('data', function (chunk)
//   xxx are of type fs
CxList recCallAsMDSecond = All.NewCxList();
CxList allOnMethods = methInvFromRequireFS.FindByMemberAccess("*.on");
allOnMethods.Add(methInvFromRequireFS.FindByMemberAccess("*.addListener"));
allOnMethods.Add(methInvFromRequireFS.FindByMemberAccess("*.once"));

CxList onMethodFirstParam = onlyParams.GetParameters(allOnMethods, 0);	//first parameter of .on, .addListener, .once

//first parameter of callBack of .on, .addListener, .once functions with connect/upgrade parameter is an input
//second parameter of callBack of .on, .addListener, .once functions with connect/upgrade parameter is a socket for client/srver comunication
CxList onConnectUpgradeParams = onMethodFirstParam.FindByShortNames(
	new List<string> {
		"\"connect\"",
		"\"upgrade\""
		}
	);

CxList ConnectUpgradeOnMeth = onConnectUpgradeParams.GetAncOfType(typeof (MethodInvokeExpr)) * allOnMethods;

fsCallback = onlyParams.GetParameters(ConnectUpgradeOnMeth, 1);

//Get All second parameters of xxx.on callback function as MethodDecl
recCallAsMDSecond = NodeJS_Get_CallBack_Method(fsCallback);
//add first parameter of callBack function of connect/upgrade as input
input.Add(allParams.GetParameters(recCallAsMDSecond, 0));

//get socket that is second parameter of connect/upgrade callBack function
CxList connectUpgradeSocket = allParams.GetParameters(recCallAsMDSecond, 1);
CxList conUpgSocketRefs = urMA.FindAllReferences(connectUpgradeSocket);
conUpgSocketRefs = conUpgSocketRefs.GetMembersOfTarget() * methodInvoke;

//get .on .addListener and .once methods for socket references of connect/upgrade callback
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

fsCallback = onlyParams.GetParameters(dataLineOnMeth, 1);

//Get All second parameters of xxx.on callback function as MethodDecl
recCallAsMDSecond = NodeJS_Get_CallBack_Method(fsCallback);
input.Add(allParams.GetParameters(recCallAsMDSecond, 0));

//**********************************************************************************

result = input - XS_Find_All();