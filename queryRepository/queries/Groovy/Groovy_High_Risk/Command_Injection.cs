CxList inputs = Find_Interactive_Inputs(); 

CxList processStart = All.FindByMemberAccess("ProcessBuilder.start");
CxList processCommandWithStart = All.FindByMemberAccess("ProcessBuilder.command").GetTargetOfMembers().DataInfluencingOn(processStart).GetMembersOfTarget();
CxList exec = 
	All.FindByMemberAccess("*.execute") +
	All.FindByMemberAccess("Runtime.exec") + 
	All.FindByMemberAccess("getRuntime.exec") +
	All.FindByMemberAccess("System.exec") +
	All.FindByMemberAccess("Call.setOperationName") +
	processStart + All.GetParameters(processCommandWithStart)// ProcessBuilder
	;

CxList execParam = All.GetParameters(exec);
CxList execFirstParam = All.GetParameters(exec, 0);
CxList sanitize = 
	Find_General_Sanitize() + 
	Find_Integers() + 
	Get_ESAPI().FindByMemberAccess("Encoder.encodeForOS") +
	All.GetByAncs(execParam - execFirstParam); // exec's non-first parameters

result = exec.InfluencedByAndNotSanitized(inputs, sanitize, CxList.InfluenceAlgorithmCalculation.NewAlgorithm)
	+ exec * inputs;