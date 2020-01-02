CxList strings = Find_String_Literal();

// SAPUI csv file save  
string className = "sap/ui/core/util/File";
CxList loadedUIComponentsRefs = Find_SAPUI_Objects_By_Name(className);

CxList mimeTypeCSV = strings.FindByShortName("text/csv");
CxList fileExtensionCSV = strings.FindByShortName("csv");

CxList fileSave = loadedUIComponentsRefs.GetMembersOfTarget().FindByShortName("save");
fileSave = fileSave.FindByParameters(fileExtensionCSV.GetParameters(fileSave, 2));
fileSave = fileSave.FindByParameters(mimeTypeCSV.GetParameters(fileSave, 3));

result = All.GetParameters(fileSave, 0);