//Find all arrays
CxList arrays = All.FindByType(typeof(RankSpecifier));

//Find all arrays declarations
CxList arrays_decl = arrays.GetAncOfType(typeof(FieldDecl)) * Find_Field_Decl();

//Find all arrays that are ParamDecl
arrays = arrays.GetAncOfType(typeof(ParamDecl));

//Find all private array declarations
CxList private_arrays = arrays_decl.FindByFieldAttributes(Modifiers.Private);

//Find all public methods that have as arguments arrays
CxList public_methods = arrays.GetAncOfType(typeof(MethodDecl)).FindByFieldAttributes(Modifiers.Public);

CxList methods = Find_Methods();

//Find Sanitizers
CxList cloners = methods.FindByName("*.clone");
cloners.Add(methods.FindByName("*.arraycopy"));
cloners.Add(methods.FindByName("*.copyOf"));
cloners.Add(methods.FindByName("*.System.arraycopy"));
cloners.Add(methods.FindByName("*.Arrays.copyOf"));
cloners.Add(methods.FindByName("*.add")); //Adding is not assignment
cloners.Add(methods.FindByName("*.iterator")); // usually iterators are used to copy from an array to another
cloners.Add(methods.FindByName("*.getcache*", false)); 
cloners.Add(methods.FindByName("*.toString", false)); 
 //Returns an array string(strings in java are imutable) from ServletRequest so it is a sanitizer.
cloners.Add(methods.FindByMemberAccess("ServletRequest.getParameterValues"));
cloners.Add(methods.FindByMemberAccess("StringBuilder.*")); //Builds a string
cloners.Add(methods.FindByMemberAccess("StringBuffer.*")); //Builds a string
cloners.Add(Find_Integers());


CxList priv_arrays = All.FindAllReferences(private_arrays);
CxList public_arrays = arrays.GetByAncs(public_methods);
CxList priv_def = All.FindDefinition(private_arrays);

CxList priv_arrays_left = priv_arrays.FindByAssignmentSide(CxList.AssignmentSide.Left);

CxList flows_public_to_private = priv_arrays_left.InfluencedByAndNotSanitized(public_arrays, cloners);

foreach(CxList flow in flows_public_to_private.GetCxListByPath())
{
	CxList endNodes = flow.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
	CxList def = priv_def.FindDefinition(endNodes);
	result.Add(flow.ConcatenatePath(def, false));
}
result = result.ReduceFlow(CxList.ReduceFlowType.ReduceSmallFlow);