//Botan
List<string> setIVFunctions = new List<string> {"set_iv"};
		
CxList setIVMethods = Find_Methods().FindByShortNames(setIVFunctions);
result = All.GetParameters(setIVMethods, 0).GetTargetOfMembers();
result -= Find_Parameters();