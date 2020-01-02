/*
 * This query will find all inputs used as second argument in "os/exec" package.
 *  This argument will never be used as execution path because only as a first argument or as
 *  third argument in combination with a double arguments like "/bin/bash"+"-c" or "cmd.exe"+"/C".
 *  This situation happens similarly in CommandContext where this one has one more argument in the first position.
*/
CxList allStrings = All.FindByAbstractValue(abstractValue => abstractValue is StringAbstractValue); 

CxList allShStrings = All.NewCxList();
string[] whiteList = new string[] {"cmd.exe", "bash", "dash", "sh", "/bin/bash", "/bin/sh","/bin/dash"};
foreach(string cmd in whiteList)
{
	IAbstractValue absvalue = new StringAbstractValue(cmd);
	allShStrings.Add(allStrings.FindByAbstractValue(abstractValue => abstractValue.IncludedIn(absvalue)));
}

CxList methods = Find_Members_By_Import("os/exec", new string[] {"Command"});
CxList knownExecs = allShStrings.GetParameters(methods, 0);
result.Add(All.GetParameters(methods.FindByParameters(knownExecs),1));


methods = Find_Members_By_Import("os/exec", new string[] {"CommandContext"});
knownExecs = allShStrings.GetParameters(methods, 1);
result.Add(All.GetParameters(methods.FindByParameters(knownExecs),2));
result.Add(Find_WhiteListSanitizers());