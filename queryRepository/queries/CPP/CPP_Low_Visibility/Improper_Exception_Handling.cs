CxList Error_Prone_Code = null;
CxList pure_C_Data = Find_C_Files_Contents();

bool exit = false;
object [] oTag = null;

if(param.Length == 0)//initialization
{
	oTag = new object[1];
	oTag[0] = new ArrayList();
	CxList methods = Find_Methods();
	Error_Prone_Code = Find_DB();
	Error_Prone_Code.Add(methods.FindByShortName("_wfopen"));
	Error_Prone_Code.Add(methods.FindByShortName("fopen_s"));
	Error_Prone_Code.Add(methods.FindByShortName("_wfopen_s"));
	Error_Prone_Code.Add(methods.FindByShortName("_alloca"));
	Error_Prone_Code.Add(methods.FindByShortName("EnterCriticalSection"));
	Error_Prone_Code.Add(methods.FindByShortName("LeaveCriticalSection"));

	Error_Prone_Code -= pure_C_Data;
	exit = false;
}
else
{
	if(param.Length == 1) // oTag not passed in parameter
	{
		oTag = new object[1];
		oTag[0] = new ArrayList();
	}
	else
	{ // param.Length == 2
		object test = param[1] as object;
		if (test == null)
		{
			oTag = new object[1];
			oTag[0] = new ArrayList();
		}
		else
		{
			oTag = param[1] as object[];
		}
	}
	exit = false;
	Error_Prone_Code = param[0] as CxList;
	foreach(int NodeId in Error_Prone_Code.GetArrayOfNodeIds())
	{
		if(((ArrayList)oTag[0]).Contains(NodeId))
		{
			result = Error_Prone_Code;
			exit = true;
			break;
		}
		else
		{
			((ArrayList)oTag[0]).Add(NodeId);
		}
	}
}

if (!exit)
{
	CxList inTry = Error_Prone_Code.GetAncOfType(typeof(TryCatchFinallyStmt));
	CxList notInTry = Error_Prone_Code - Error_Prone_Code.GetByAncs(inTry);

	if(notInTry.Count > 0)
	{
		CxList invokes = (All - pure_C_Data).FindAllReferences(notInTry.GetAncOfType(typeof(MethodDecl))) - 
			notInTry.GetAncOfType(typeof(MethodDecl));
	
		if(invokes.Count == 0 || Improper_Exception_Handling(invokes, oTag).Count > 0)
		{
			result.Add(notInTry);
		}	
	}
}