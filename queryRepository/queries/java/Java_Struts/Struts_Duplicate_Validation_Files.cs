// Test struts version
if(Find_Struts1_Presence().Count > 0){
	CxList validationFiles = Find_Class_Decl().FindByName("Cxvalidationxml");
	if (validationFiles.Count>1)
	{
		result = validationFiles;
	}
}