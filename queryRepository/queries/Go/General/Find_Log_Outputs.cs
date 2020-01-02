// Find_Log_Outputs
CxList logs = All.NewCxList();

List<string> names = new List<string> {"Print*","Panic*","Fatal*"};
logs.Add(All.FindByMemberAccess("\"log\".*").FindByShortNames(names));

result.Add(logs);