string resourceType = "Camera";
string openString = "open";
string closeString = "release";
result.Add(Find_Android_Improper_Resource(resourceType, openString, closeString));

resourceType = "MediaPlayer";
openString = "start";
closeString = "release";
result.Add(Find_Android_Improper_Resource(resourceType, openString, closeString));

resourceType = "SQLiteDatabase";
string createResourceType = "SQLiteOpenHelper";
openString = "getWritableDatabase";
closeString = "close";
result.Add(Find_Android_Improper_Resource(resourceType, openString, closeString, createResourceType));