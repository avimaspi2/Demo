//Find all arrays
CxList arrays_init = All.FindByType(typeof(RankSpecifier));

//Find all arrays declarations
CxList arrays_decl = arrays_init.GetAncOfType(typeof(FieldDecl)) * Find_Field_Decl();

//Find all arrays that are returned methods
//CxList arrays = arrays_init.GetAncOfType(typeof(MethodDecl));
CxList arrays = All.FindAllReferences(arrays_decl).GetAncOfType(typeof(MethodDecl));

arrays = arrays_init.GetFathers().GetFathers() * arrays;

//Find public methods that have arrays as arguments
CxList public_methods = arrays.FindByFieldAttributes(Modifiers.Public);

//Find all methods with return stmt;
CxList return_list = Find_ReturnStmt();

//Get the public methods that have return statements
return_list = return_list.GetByAncs(public_methods);

//Find all private array declarations
CxList private_arrays = arrays_decl.FindByFieldAttributes(Modifiers.Private);

CxList methods = Find_Methods();

//Find Sanitizers
CxList cloners = methods.FindByName("*.clone");
cloners.Add(methods.FindByName("*.arraycopy"));
cloners.Add(methods.FindByName("*.copyOf"));
cloners.Add(methods.FindByName("*.System.arraycopy"));
cloners.Add(methods.FindByName("*.Arrays.copyOf"));
cloners.Add(methods.FindByName("*.iterator")); // usually iterators are used to copy from an array to another
cloners.Add(methods.FindByName("*.getcache*", false)); 
cloners.Add(methods.FindByName("*.toString", false)); 

 //Returns an array string(strings in java are imutable) from ServerletRequest so it is a sanitizer.
cloners.Add(methods.FindByMemberAccess("ServletRequest.getParameterValues"));
cloners.Add(methods.FindByMemberAccess("StringBuilder.*")); //Builds a string
cloners.Add(methods.FindByMemberAccess("StringBuffer.*")); //Builds a string

	


//Get the expressions from the return statements
CxList return_exp = All.FindByFathers(return_list);
return_exp -= return_exp.GetTargetOfMembers();
return_exp = return_exp.FindByType(typeof(UnknownReference)) + return_exp.FindByType(typeof(MemberAccess));

CxList priv_defs = All.FindDefinition(private_arrays);

foreach(CxList ret in return_exp)
{
	CxList retDef = ret.InfluencedByAndNotSanitized(priv_defs, cloners);
	CxList retDefStartNodes = retDef.GetStartAndEndNodes(CxList.GetStartEndNodesType.StartNodesOnly);
	CxList intersectStart = retDefStartNodes * priv_defs;
	if (intersectStart.Count > 0)
	{
		//The result flow should contain only the definition of the private array and the return statement 
		//of this array from a public method, hence the ConcatenatePath
		CxList res = priv_defs.FindDefinition(ret).ConcatenatePath(ret, false);
		result.Add(res);
	}
}

result = result.ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);