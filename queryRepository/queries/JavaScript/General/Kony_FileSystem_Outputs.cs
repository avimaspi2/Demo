if(cxScan.IsFrameworkActive("KonyInFF"))
{
	CxList konyAll = Kony_All();
	CxList invokes = Find_Methods() * konyAll;

	CxList newfileObj = konyAll.FindByName("*kony.io.File");
	newfileObj.Add(invokes.FindByName("kony.io.FileSystem.getFile"));

	CxList fileInstances = konyAll.FindAllReferences(newfileObj.GetAssignee(konyAll));

	CxList copy = fileInstances.GetMembersOfTarget().FindByShortName("copyTo");
	copy.Add(fileInstances.GetMembersOfTarget().FindByShortName("moveTo"));
	copy.Add(fileInstances.GetMembersOfTarget().FindByShortName("rename"));

	result.Add(konyAll.GetParameters(newfileObj));
	result.Add(konyAll.GetParameters(copy));
}