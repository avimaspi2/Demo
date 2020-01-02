// Test struts version
if(Find_Struts1_Presence().Count > 0){ 
	CxList configFiles = Find_Class_Decl().FindByName("Cxstruts_configxml");
	if (configFiles.Count>1)
	{
		result = configFiles;
	}
}