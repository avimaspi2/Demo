/*
	Find_Botan_PBDKF2_Function
		pbkdf         - a CxList containing the PBDKF algorithm to be used (Optional)
		functionNames - a List<string> or string, containing the name(s) of the derivation functions to search
*/
if (param.Length >= 1 && param.Length <=2)
{	
	int i = 0;
		
	CxList pbkdf = (param.Length == 1)? Find_Botan_PBDKF2() : param[i++] as CxList;
	var functionNames = param[i];
		
	//Look for the variable that holds the retrievers
	//ex. PBKDF* pbkdf = get_pbkdf("PBKDF2(SHA-256)"); 
	CxList leftSide = pbkdf.GetAssignee();
	result = All.FindAllReferences(leftSide).GetMembersOfTarget();

	//Look for direct usages of the retrieved PBDKF2
	//ex. get_pbkdf("PBKDF2(SHA-256)")->derive_key(32, "password", &salt[0], salt.size(), iter);
	result.Add(pbkdf.GetMembersOfTarget());
	
	if(functionNames is string){
		result = result.FindByShortName(functionNames as string);
	} else {
		result = result.FindByShortNames(functionNames as List<string>);
	}
}
else
{
	cxLog.WriteDebugMessage("Number of parameters should be 1 or 2");			
}