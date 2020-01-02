if(cxScan.IsFrameworkActive("SAPUI"))
{
	/// get the object creations from SAP library (new sap.*())
	/// 	example: new sap.ui.model.json.JSONModel();
	CxList sapLibraryObjCreations = Find_SAP_Library() * Find_ObjectCreations();

	/// that instantiate a SAP OData Model
	/// 	example: new sap.ui.model.odata.v4.ODataModel();
	CxList sapodata = sapLibraryObjCreations.FindByType("sap.ui.model.odata*");
	result = sapodata.FindByType("*.ODataModel");
}