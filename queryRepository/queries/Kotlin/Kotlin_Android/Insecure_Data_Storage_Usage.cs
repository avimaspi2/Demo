/*
Insecure_Data_Storage_Usage
	This query should identify any local storage use, where local storage is at 
	least world readable, like external storage (external storage can also be a 
	destination to create a database in, too).

Sources
	Any source.

Sinks
	External storage, including databases that are written to external storage, 
	storing files in a SD card, databases written to external storage etc. This 
	also includes files written to internal storage IF the file permissions are 
	set with setReadable(true, false), allowing any other application to read its 
	contents. It should be noted that SharedPreferences used to have a 
	WORLD_READABLE mode, but this has been deprecated in API 17 and works very 
	poorly anyway (does not always guarantee world readability). It should still 
	be verified, and is covered in the Android query.

*/

CxList methods = Common_General.Find_Methods();
CxList fileWrite = Find_Write();

//External Storage
CxList externalStorageDir = methods.FindByMemberAccess("Environment.getExternalStoragePublicDirectory");
result.Add(externalStorageDir.DataInfluencingOn(fileWrite));
CxList db = Find_DB_In();
result.Add(externalStorageDir.DataInfluencingOn(db));

//Internal Storage 
CxList internalStorageDir = methods.FindByShortName("getFilesDir");
CxList setReadable = methods.FindByMemberAccess("File.setReadable");
CxList trueAbsValue = Common_General.Find_True_Abstract_Value();
CxList falseAbsValue = Common_General.Find_False_Abstract_Value();
CxList setReadableTrueFirstParam = setReadable.FindByParameters(All.GetParameters(setReadable, 0) * trueAbsValue);
CxList setReadableFalseSecondParam = setReadable.FindByParameters(All.GetParameters(setReadable, 1) * falseAbsValue);

CxList vulnerableSetReadable = setReadableTrueFirstParam * setReadableFalseSecondParam;
CxList vulnerableFileRefs = All.FindAllReferences(vulnerableSetReadable.GetTargetOfMembers().FindByType("File"));

CxList vulnerableFilesWrittenTo = fileWrite.DataInfluencedBy(vulnerableFileRefs);
CxList vulnerableWrites = vulnerableFilesWrittenTo.GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);

result.Add(internalStorageDir.DataInfluencingOn(vulnerableWrites));