CxList nodeDbOut = NodeJS_Find_DB_Out();
CxList Inputs = All.NewCxList();
Inputs.Add(nodeDbOut);
Inputs.Add(NodeJS_Find_Read());
////////////////////////////////////////////////////////////////////////////////////////////////////
CxList methodInvoke = Find_Methods();
CxList onlyParams = Find_Parameters();
CxList uRef = Find_UnknownReference();
CxList mA = Find_MemberAccesses();
CxList urMA = uRef.Clone();
urMA.Add(mA);

CxList outputs = All.NewCxList();
	
CxList requireOfFs = Find_Require("fs");

CxList allInfluByRequireFS = methodInvoke.DataInfluencedBy(requireOfFs);
//All methodInvoke influenced by required library
CxList methInvFromRequireFS = allInfluByRequireFS * methodInvoke;

CxList fsPath = methInvFromRequireFS.FindByShortNames(
	new List<string> {
		"appendFile", 
		"createWriteStream", 
		"open",
		"readFile",
		"writeFile"
	}
);

CxList _params = urMA.Clone();
_params.Add(onlyParams);
CxList pathOut = _params.GetParameters(fsPath, 0);

//Remove anonymous functions from pathOut
CxList anony = Find_LambdaExpr();
pathOut -= anony;

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

result = outputs.InfluencedByAndNotSanitized(Inputs, sanitize, CxList.InfluenceAlgorithmCalculation.NewAlgorithm)
	.ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow);
result.Add(nodeDbOut * outputs - sanitize);