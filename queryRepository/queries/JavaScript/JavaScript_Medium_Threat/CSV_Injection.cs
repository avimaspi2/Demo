// Find CSV Injection
// Finds tainted data written to csv files 
CxList inputs = Find_Inputs();
if(cxScan.IsFrameworkActive("SAPUI"))
{
	inputs.Add(Find_SAPUI_OData_Reads());
}
inputs.Add(inputs.GetRightmostMember());

CxList csvFileSaveData = Find_CSV_Data_Write();

result = inputs.DataInfluencingOn(csvFileSaveData);