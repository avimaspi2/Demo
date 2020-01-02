// Read Android preferences
CxList SharedPref = All.FindByMemberAccess("SharedPreferences.*");
result = SharedPref.FindByShortNames(new List<string> {"getString","getStringSet","getAll"});