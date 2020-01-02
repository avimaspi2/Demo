/* Here we get with 5 types of unused variables
1. Variables and function parameters that are never ever used.
2. Variables that are initialized but never used.
3. Function parameters that are changed within the function and only then used. This might not be a problem
   but it certainly is a bad coding practice
4.Disable: Initial value set to a member is never used, but overridden by other values
*/

/// Part 1 - Variables and function parameters that are never ever used

CxList neverUsed = Unused_Variables_And_Functions_Params();
	
/// Part 2 - Variables that are initialized but never used

CxList onlyInitialized = Unused_Initialized_Variables();

/// Part 3 - Function parameters that are changed within the function and only then used

CxList changedParams = Unused_Param_Changed_In_Function();

/// Disable: Part 4 - Initial value set to a member is never used, but overridden by other values

/// Final result
result = neverUsed;
result.Add(onlyInitialized);
result.Add(changedParams);

result -= Find_Properties_Files();

result -= result.FindAllReferences(All.GetByAncs(Find_Conditions()));

//fmt_message_Key and fmt_bundle_file exists in FMT taglib of jsp files
CxList fmt = result.FindByFileName("*.MF");
fmt.Add(result.FindByShortName("fmt_message_Key"));
fmt.Add(result.FindByShortName("fmt_bundle_file*"));
result -= fmt;
CxList usedAssignRef = Find_Elimination_Variables();
// remove interface method params and variables
result -= result.GetByAncs(All.FindByType(typeof(InterfaceDecl)));
usedAssignRef = usedAssignRef.GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly);
result -= result.FindAllReferences(usedAssignRef);

// Exclude JSF temp variables
result -= Find_JSF_Temp_Variables();