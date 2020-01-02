// Find_Interactive_Outputs
CxList prints = All.NewCxList();

List<string> names = new List<string> {"Print*","Sprint*","Fprint*","Errorf"};
prints.Add(All.FindByMemberAccess("\"fmt\".*").FindByShortNames(names));

result.Add(prints);
result.Add(Find_Web_Outputs());