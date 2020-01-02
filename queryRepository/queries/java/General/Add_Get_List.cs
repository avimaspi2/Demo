CxList allDeclAndFieldDecl = Find_Field_Decl();
allDeclAndFieldDecl.Add(Find_Declarators());

CxList allUnknownRefAndParams = Find_ParamDecl();
allUnknownRefAndParams.Add(Find_UnknownReference());

CxList allLists = allUnknownRefAndParams.FindAllReferences(allDeclAndFieldDecl.FindByRegex(@"List\s*<"));

CxList allAdds = allLists.GetMembersOfTarget().FindByShortName("add");
allAdds.Add(allLists.GetMembersOfTarget().FindByShortName("addAll"));

foreach(CxList addA in allAdds.GetCxListByPath())
{
	CxList addAStart = addA.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly).GetTargetOfMembers();
	CxList getList = allLists.FindAllReferences(addAStart).GetMembersOfTarget().FindByShortName("get");
	
	foreach(CxList g in getList)
	{
		CustomFlows.AddFlow(addA, g);
	}
}