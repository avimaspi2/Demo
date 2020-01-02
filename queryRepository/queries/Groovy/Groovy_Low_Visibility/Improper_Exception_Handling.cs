CxList Error_Prone_Code = null;

bool exit = false;
object [] oTag = null;
if(param.Length == 0)//initialization
{
	oTag = new object[1];
	oTag[0] = new ArrayList();;
	Error_Prone_Code = Find_Commands_With_Exception();
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
		if(((ArrayList) oTag[0]).Contains(NodeId))
		{
			result = Error_Prone_Code;
			exit = true;
			break;
		}
		else
		{
			((ArrayList) oTag[0]).Add(NodeId);
		}
	}
}

if (!exit)
{
	CxList inTry = Error_Prone_Code.GetAncOfType(typeof(TryCatchFinallyStmt));
	CxList notInTry = Error_Prone_Code - Error_Prone_Code.GetByAncs(inTry);

	if(notInTry.Count > 0)
	{
		CxList potentiallyUnsafeMethods = notInTry.GetAncOfType(typeof(MethodDecl));
		CxList invokes = All.FindAllReferences(potentiallyUnsafeMethods) - potentiallyUnsafeMethods;
		CxList recursiveResult = Improper_Exception_Handling(invokes, oTag);
		if(invokes.Count == 0 || recursiveResult.Count > 0)
		{
			CxList safeMethods = potentiallyUnsafeMethods.FindDefinition(invokes - recursiveResult);
			result.Add(notInTry - (notInTry.GetByAncs(safeMethods)));
			result = result.GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly);
		}	
	}
}