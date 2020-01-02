if(cxScan.IsFrameworkActive("SAPUI"))
{
	// Find the calls to file upload methods from XML files
	result = Find_XML_Calls_to_Libraries("sap.ca.ui", "AddPicture");
	result.Add(Find_XML_Calls_to_Libraries("sap.ca.ui", "FileUpload"));
	result.Add(Find_XML_Calls_to_Libraries("sap.m", "UploadCollection"));
	result.Add(Find_XML_Calls_to_Libraries("sap.ui.commons", "FileUploader"));
	result.Add(Find_XML_Calls_to_Libraries("sap.ui.unified", "FileUploader"));

	// Find the calls to file upload methods from Typescript files
	CxList sapLibraryCreate = Find_SAP_Library().FindByType(typeof(ObjectCreateExpr));
	result.Add(sapLibraryCreate.FindByTypes(new string[] {
		"sap.ca.ui.FileUpload", 
		"sap.ca.ui.AddPicture",
		"sap.ui.commons.FileUploader",
		"sap.ui.unified.FileUploader", 
		"sap.m.UploadCollection"}));
}