/*with this query you can catch the razor inputs comming from models*/
CxList pagesWithModel = All.FindByRegexExt("@model", "*.cshtml");
CxList unknownReferences = Find_Unknown_References();
CxList validReferences = All.NewCxList();
foreach(CxList model in pagesWithModel){
	try{
		String fname = model.GetFirstGraph().LinePragma.FileName;
		validReferences.Add(unknownReferences.FindByFileName(fname));
	}
	catch(Exception){}
}

result = validReferences.FindByShortName("Model").GetMembersOfTarget();