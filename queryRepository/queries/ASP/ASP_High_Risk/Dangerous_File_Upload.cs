CxList file = All.FindByName("*.postedfile.filename");
CxList inputs = Find_Interactive_Inputs();
CxList save = All.FindByName("*.postedfile.saveas");
CxList MapPath = All.FindByName("server.mappath");

result = save.DataInfluencedBy(file + inputs) - 
			save.DataInfluencedBy(MapPath);