CxList imports = Find_Imports();
String[] subprocessMethods = new string[] { "call", "Popen", "check_output", "check_call"};
String[] popenMethods = new string[] {"popen2", "popen3", "popen4"};
String[] commandMethods = new string[] {"getstatusoutput", "getoutput"};

CxList subprocessMethodsList = Find_Methods_By_Import("subprocess", subprocessMethods, imports);

CxList popenMethodsList	= Find_Methods_By_Import("popen2", popenMethods, imports);

CxList commandMethodsList = Find_Methods_By_Import("command", commandMethods, imports);

CxList commands = All.NewCxList();
commands.Add(subprocessMethodsList);
commands.Add(popenMethodsList);
commands.Add(commandMethodsList);

CxList inputs = Find_Interactive_Inputs();
inputs -= inputs.FindByAssignmentSide(CxList.AssignmentSide.Left);

CxList sanitize = (All.GetByAncs(All.GetParameters(commands)) - All.GetByAncs(All.GetParameters(commands, 0))); 
sanitize.Add(Find_Integers());

String[] osMethods = new string[] { "system", "popen", "spawn*", "exec*", "startfile"};
CxList osMethodsList = Find_Methods_By_Import("os", osMethods, imports);

CxList falseOsMethods = osMethodsList.FindByShortName("spawn");
osMethodsList -= falseOsMethods;

// Only the first parameter of os.startfile is vulnerable
CxList osStartFile = osMethodsList.FindByShortName("startfile");
commands.Add(All.GetParameters(osStartFile, 0));
osMethodsList -= osStartFile;

CxList osEnvAsLastParam = osMethodsList.FindByShortNames(new List <string> 
							{"spawnle", "spawnlpe", "spawnve", "spawnvpe",
							 "execle", "execlpe", "execve", "execvpe"});
CxList osMethodsParameters = All.GetParameters(osEnvAsLastParam);
// Add the parameters of os commands, except for those that accept environment as a parameter
CxList vulnerableParameters = All.GetParameters(osMethodsList - osEnvAsLastParam);
CxList osSpawnMethods = osMethodsList.FindByShortName("spawn*");
commands.Add(vulnerableParameters);

CxList remainedMethods = All.NewCxList();
remainedMethods.Add(osEnvAsLastParam);

CxList lastIndexParameters = All.NewCxList();
for(int i = 0; i < 10 && remainedMethods.Count > 0; i++)
{
	CxList parametersAtIndex = osMethodsParameters.GetParameters(remainedMethods, i);
	remainedMethods = remainedMethods.FindByParameters(parametersAtIndex);
	CxList nonEnvParameter = lastIndexParameters.GetParameters(remainedMethods);
	CxList envParameter = lastIndexParameters - nonEnvParameter;
	lastIndexParameters = parametersAtIndex;
	commands.Add(nonEnvParameter);
	sanitize.Add(envParameter);
}

// The first parameter of os.spawn*e is the mode, and is not vulnerable
CxList safeSpawnParameter = commands.GetParameters(osSpawnMethods, 0);
commands -= safeSpawnParameter;
sanitize.Add(safeSpawnParameter);

result = commands.InfluencedByAndNotSanitized(inputs, sanitize);