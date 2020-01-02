// Query - Insecure Data Storage
// -----------------------------
//Query find All using of External Storage.

//Find Using of External Storage for File
CxList CreateOfObject = Find_Object_Create();
CxList FileObjects = CreateOfObject.FindByShortName("File");

CxList strings = Find_Strings();
CxList sd = strings.FindByName(@"*/sdcard/*", false);

CxList MethodInvoke = Find_Methods(); 
CxList ExternalStorage = MethodInvoke.FindByMemberAccess("Environment.getExternalStorageDirectory");
ExternalStorage.Add(MethodInvoke.FindByMemberAccess("Environment.getExternalStoragePublicDirectory"));
ExternalStorage.Add(MethodInvoke.FindByMemberAccess("Context.getExternalCacheDir"));
ExternalStorage.Add(MethodInvoke.FindByMemberAccess("Context.getExternalFilesDir"));
ExternalStorage.Add(sd);

CxList ExternalStorUsageFile = FileObjects.FindByParameters(ExternalStorage);

result.Add(ExternalStorUsageFile);

//Find Using of ExternalStorage for Sequel Database
CxList OpenOrCreateDatabase = MethodInvoke.FindByShortName("openOrCreateDatabase"); 
CxList ExternalStorUsageDB = OpenOrCreateDatabase.FindByParameters(ExternalStorage);

result.Add(ExternalStorUsageDB);

//Find Using of getSharedPreferences function with "MODE_WORLD_READABLE" parameter.
CxList getSharPref = MethodInvoke.FindByShortName("getSharedPreferences");
CxList readableString = Find_Params().FindByShortName("MODE_WORLD_READABLE");
CxList getSharWithReadableParam = Find_By_Parameter_Position(getSharPref, 1, readableString);

result.Add(getSharWithReadableParam);