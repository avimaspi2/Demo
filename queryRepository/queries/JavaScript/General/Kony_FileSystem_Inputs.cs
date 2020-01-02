if(cxScan.IsFrameworkActive("KonyInFF"))
{
	CxList konyAll = Kony_All();
	CxList invokes = Find_Methods() * konyAll;

	CxList newfileObj = konyAll.FindByName("*kony.io.File");
	newfileObj.Add(invokes.FindByName("kony.io.FileSystem.getFile"));

	CxList fileInstances = konyAll.FindAllReferences(newfileObj.GetAssignee(konyAll));

	CxList read = fileInstances.GetMembersOfTarget().FindByShortName("read");
	read.Add(fileInstances.GetMembersOfTarget().FindByShortName("readAsText"));

	result = read;
}