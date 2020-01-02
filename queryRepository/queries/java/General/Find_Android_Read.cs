// Read Android preferences
CxList SharedPref = All.FindByMemberAccess("SharedPreferences.*");

result = SharedPref.FindByMemberAccess("SharedPreferences.getString");
result.Add(SharedPref.FindByMemberAccess("SharedPreferences.getStringSet")); 
result.Add(SharedPref.FindByMemberAccess("SharedPreferences.getAll"));