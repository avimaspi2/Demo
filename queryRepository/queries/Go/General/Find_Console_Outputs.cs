// Find_Console_Outputs
List<string> names = new List<string> {"Print", "Printf", "Println"};
result = All.FindByMemberAccess("\"fmt\".*").FindByShortNames(names);