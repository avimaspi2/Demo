CxList Inputs = NodeJS_Find_Interactive_Inputs();
////////////////////////////////////////////////////////////////////////////////////////////////////
CxList methodInvoke = Find_Methods();
CxList onlyParams = Find_Parameters();
CxList uRef = Find_UnknownReference();
CxList mA = Find_MemberAccesses();
CxList urMA = All.NewCxList();
urMA.Add(uRef);
urMA.Add(mA);

CxList outputs = All.NewCxList();

CxList requireOfFS = Find_Require("fs");

CxList allInfluByRequireFS = methodInvoke.DataInfluencedBy(requireOfFS);
//All methodInvoke influenced by required library
CxList methInvFromRequireFS = allInfluByRequireFS * methodInvoke;

CxList fsPath = methInvFromRequireFS.FindByShortNames(
	new List<string> {
		"appendFile", 
		"createWriteStream",
		"createReadStream",
		"open",
		"readFile",
		"writeFile"
	}
);

CxList _params2 = urMA.Clone();
_params2.Add(onlyParams);

CxList pathOut = _params2.GetParameters(fsPath, 0);
//in case path parameter of type ==>  aaa + bbb
CxList partsOfPath = urMA.GetByAncs(pathOut);
outputs.Add(pathOut);
outputs.Add(partsOfPath);
if(Hapi_Find_Server_Instance().Count > 0)
{
	outputs.Add(Hapi_Find_File_Serving());
}
////////////////////////////////////////////////////////////////////////////////////////////////////

CxList sanitize = NodeJS_Find_Path_Traversal_Sanitize();

result = outputs.InfluencedByAndNotSanitized(Inputs, sanitize).ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);
result.Add(Inputs * outputs - sanitize);