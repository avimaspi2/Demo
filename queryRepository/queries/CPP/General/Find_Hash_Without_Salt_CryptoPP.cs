if(param.Length > 0){

	CxList strings = (CxList) param[0];
	CxList methods = (CxList) param[1];
	CxList salt = (CxList) param[2];
	
	///////////////
	///Crypto++///
	/////////////
	//
	//Crypto provides 2 ways of encrypting data:
	//MultiStep  -> Several methods that prepare the data to be encrypted, and the encryption can be divided in several steps
	//SingleStep -> One method, that receives the data and returns the encrypted data.
	///////////

	//Hash Classes 
	string[] classes = 
		{
		@"SHA",@"SHA1",@"SHA224", @"SHA256", @"SHA384", @"SHA512", 
		@"Tiger", @"Whirlpool", @"RIPEMD160", @"RIPEMD320", 
		@"RIPEMD128", @"RIPEMD256", @"MD2", @"MD4", @"MD5"
		}; 

	//1 MultiStep
	/////////////

	//Get Update Methods	
	CxList update_methods = methods.FindByShortName("update", false);

	//Get Update Objects
	CxList objects_upd = update_methods.GetTargetOfMembers();

	//Get objects that are of type contained in classes list
	objects_upd = objects_upd.FindByTypes(classes);

	//Get only the update members that are of type contained in classes list
	update_methods = objects_upd.GetMembersOfTarget();

	//Get Final Methods
	CxList final_methods = methods.FindByShortName("final", false);

	//Get Final Objects 
	CxList objects_fin = final_methods.GetTargetOfMembers();

	//Get objects that are of type contained in classes list
	objects_fin = objects_fin.FindByTypes(classes);

	//Get only the update members that are of type contained in classes list
	final_methods = objects_fin.GetMembersOfTarget();

	//Find Hash definition
	CxList def = All.FindDefinition(objects_upd);

	/*Find if exists flow between update objects, if it exists then there are 2 or more update objects.
	So they are sanitized, because a salt is modifing the input before digesting it.
	so two updates are the same as using salt on input and can be removed */
	objects_upd -= objects_upd.DataInfluencedBy(objects_upd).GetStartAndEndNodes(CxList.GetStartEndNodesType.StartAndEndNodes);

	//Get end nodes influeced by salt that are Methods
	CxList san = salt.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly).FindByType(typeof(MethodInvokeExpr));

	//Remove santized updates
	objects_upd -= san.GetTargetOfMembers();

	//Get the update objects that are influencing final objects
	result.Add(objects_upd.DataInfluencingOn(objects_fin));

	//2 SingleStep
	//////////////

	//Get Update Methods	
	CxList digest_methods = methods.FindByShortName("CalculateDigest", false);

	//Get Update Objects
	CxList objects_dig = digest_methods.GetTargetOfMembers();

	//Get objects that are of type contained in classes list
	objects_dig = objects_dig.FindByTypes(classes);

	//Get the digest methods
	digest_methods = objects_dig.GetMembersOfTarget();

	//Find digest methods that are influenced by strings and not sanitized
	result.Add(digest_methods.InfluencedByAndNotSanitized(strings, salt));

}