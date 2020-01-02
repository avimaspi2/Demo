CxList methods = Find_Methods();
result = methods.FindByRegex("print[ ]*>>");
	//methods.FindByShortName("print").FindByParameterValue(0, ">>*", BinaryOperator.IdentityEquality);

CxList open = All.FindByMemberAccess("*.open");
CxList fileOpen = All.FindByAssignmentSide(CxList.AssignmentSide.Right).FindByShortName("open") - open;
CxList varsInfluencedByOpen = All.FindAllReferences(
	All.DataInfluencedBy(fileOpen).FindByAssignmentSide(CxList.AssignmentSide.Left));

result.Add(varsInfluencedByOpen.GetMembersOfTarget().FindByShortName("write")); 

result.Add(Find_Log_Outputs());

// Pickle Stream Dump
result.Add(Find_Methods_By_Import("pickle", new string[]{"dump","dumps"}));
result.Add(Find_Methods_By_Import("cPickle", new string[]{"dump","dumps"}));
CxList classes = Find_ClassDecl();
CxList picklers = classes.InheritsFrom("pickle.Pickler");
foreach(CxList pickler in picklers){
	CSharpGraph pickl = pickler.GetFirstGraph();
	result.Add(methods.FindByMemberAccess(pickl.ShortName + "." + "dump"));
}