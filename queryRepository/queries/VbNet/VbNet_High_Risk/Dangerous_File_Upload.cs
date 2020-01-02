CxList file = All.FindByName("*.PostedFile.FileName", false);
CxList inputs = Find_Interactive_Inputs();
CxList save = All.FindByName("*.PostedFile.SaveAs", false);
CxList MapPath = All.FindByName("Server.MapPath", false);

result = save.DataInfluencedBy(file + inputs) - 
			save.DataInfluencedBy(MapPath);