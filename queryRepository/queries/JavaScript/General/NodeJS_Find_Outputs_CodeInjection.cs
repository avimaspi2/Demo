CxList methodInvoke = Find_Methods();
CxList onlyParams = Find_Parameters();
CxList uRef = Find_UnknownReference();
CxList mA = Find_MemberAccesses();
CxList urMA = uRef.Clone();
urMA.Add(mA);

CxList output = All.NewCxList();
CxList requireOfCP = Find_Require("child_process");

//get first parameter of execute methods in file that incudes child_process library
List<String> allFilesWithCP = new List<String>();
CxList allINCP = All.NewCxList();
foreach(CxList reqCP in requireOfCP)
{
	try
	{
		CSharpGraph reqCPGR = reqCP.GetFirstGraph();
		String reqCPName = reqCPGR.LinePragma.FileName;
		if (!allFilesWithCP.Contains(reqCPName))
		{
			allFilesWithCP.Add(reqCPName);
			allINCP.Add(All.FindByFileName(reqCPName));
		}
	}
	catch (Exception ex)
	{
		cxLog.WriteDebugMessage(ex);
	}
}

CxList methInvCP = allINCP.FindByType(typeof (MethodInvokeExpr));

CxList execMethInCP = methInvCP.FindByShortName("exec");
execMethInCP.Add(methInvCP.FindByShortNames(
	new List<string> {
		"execFile",
		"fork",
		"spawn"
		}
	)
	);
execMethInCP.Add(methodInvoke.FindByShortName("eval"));

CxList _params2 = urMA.Clone();
_params2.Add(onlyParams);
CxList methodParam = _params2.GetParameters(execMethInCP, 0);
//in case first parameter of type ==>  aaa + bbb
CxList partsOfMP = urMA.GetByAncs(methodParam);

output.Add(methodParam);
output.Add(partsOfMP);

result = output;

CxList workers = methodInvoke.FindByType("Worker");	// worker create expression forks a child proccess which executes a Typescript file
result.Add(workers);