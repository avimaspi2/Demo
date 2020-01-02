CxList imports = base.Find_Import();

SortedList<int,object> fileIds = new SortedList<int,object>();
foreach(CxList elem in Find_Struts2_Presence()){
	try{
		CSharpGraph cs = elem.TryGetCSharpGraph<CSharpGraph>();
		int fId = cs.LinePragma.GetFileId();
		fileIds[fId] = null;
	}
	catch(Exception ex){
		cxLog.WriteDebugMessage(ex);
	}
}
	
CxList struts2 = All.NewCxList();
foreach(int fId in fileIds.Keys){
	struts2.Add(imports.FindByFileId(fId));
}

result = imports.FindByName("*.struts.*");
result.Add(imports.FindByName("*.struts"));
result -= struts2;
result -= imports.FindByName("*.model");