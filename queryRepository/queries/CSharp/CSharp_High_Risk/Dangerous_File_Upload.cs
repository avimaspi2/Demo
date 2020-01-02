CxList file = All.FindByName("*.PostedFile.FileName");
CxList inputs = Find_Interactive_Inputs();
CxList save = All.FindByName("*.PostedFile.SaveAs");
CxList MapPath = All.FindByName("Server.MapPath");

result = save.DataInfluencedBy(file + inputs) - 
			save.DataInfluencedBy(MapPath);