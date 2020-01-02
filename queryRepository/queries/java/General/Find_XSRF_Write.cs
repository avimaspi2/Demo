CxList strings = Find_Strings();
CxList write = All.NewCxList();

write.Add(strings.FindByName("*update*", false));
write.Add(strings.FindByName("*delete*", false));
write.Add(strings.FindByName("*insert*", false));

result = write;