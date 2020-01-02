// Query Stored Command Injection
// ==============================

CxList inputs = Find_Read();

CxList exec = 
	All.FindByMemberAccess("Runtime.exec") + 
	All.FindByMemberAccess("getRuntime.exec") +
	All.FindByMemberAccess("System.exec");

CxList execParam = All.GetParameters(exec);
CxList execFirstParam = All.GetParameters(exec, 0);

CxList sanitize = 
	Find_General_Sanitize() + 
	Find_Integers() + 
	All.GetByAncs(execParam - execFirstParam);	// exec's non-first parameters

result.Add(exec.InfluencedByAndNotSanitized(inputs, sanitize).ReduceFlow(CxList.ReduceFlowType.ReduceBigFlow));

/*
Handling the case where a "make" command is executed with no destination path as a parameter.
This command may be exploited when the PATH environment variable is pointed to a malicous binary.
*/
CxList execMethod = exec.FindByType(typeof(MethodInvokeExpr));
CxList execSecondParams = All.GetParameters(execMethod, 1).GetByAncs(execMethod);
execMethod = execMethod - execSecondParams;
CxList execParams = All.GetParameters(execMethod, 0).FindByType(typeof(StringLiteral));

CxList exploitMakeCommand = All.NewCxList();
foreach (CxList curParam in execParams)
{
	try{
		StringLiteral execliteral = curParam.data.GetByIndex(0) as StringLiteral;	
		
		if (execliteral.Value.Length > 2)
		{
			
			//check content of string literals trimmed from quotes and white spaces
			if (execliteral.Value.ToLower().Substring(1, execliteral.Value.Length - 2).Trim().Equals("make"))
			{
				exploitMakeCommand.Add(curParam);
			}
		}
	}
	catch (Exception e){}
}

result.Add(exploitMakeCommand);