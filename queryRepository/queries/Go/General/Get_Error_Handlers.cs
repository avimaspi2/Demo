//This is an helper query for improper_error_handling: returns valid errors handlers
result = Find_Console_Outputs();
result.Add(Find_Log_Outputs());
result.Add(All.FindByMemberAccess("\"fmt\".*"));
result.Add(All.FindByMemberAccess("\"log\".*"));
result.Add(All.FindByMemberAccess("\"net/http\".Error"));

result.Add(All.FindByMemberAccess("*.Print*"));
result.Add(All.FindByMemberAccess("*.Log*"));
result.Add(All.FindByMemberAccess("*.Warn*"));