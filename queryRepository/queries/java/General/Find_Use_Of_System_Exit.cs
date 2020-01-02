CxList exit = All.FindByMemberAccess("System.exit");
exit.Add(All.FindByName("exit"));

CxList exitInMain = exit.GetByAncs(Find_MethodDeclaration().FindByShortName("main"));

result = exit - exitInMain;