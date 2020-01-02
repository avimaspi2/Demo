CxList storages = All.NewCxList();

//Internal file storage: Store app-private files on the device file system.

storages.Add(Find_Write());

//External file storage: Store files on the shared external file system.
//This is usually for shared user files, such as photos.


//Shared preferences: Store private primitive data in key - value pairs.
CxList SharedPreferences = All.NewCxList();

SharedPreferences.Add(All.FindByMemberAccess("Editor.putInt"));
SharedPreferences.Add(All.FindByMemberAccess("Editor.putLong"));
SharedPreferences.Add(All.FindByMemberAccess("Editor.putString"));
SharedPreferences.Add(All.FindByMemberAccess("Editor.putStringSet"));

storages.Add(SharedPreferences);


//Databases: Store structured data in a private database.
storages.Add(Find_DB());

// output
result.Add(storages);