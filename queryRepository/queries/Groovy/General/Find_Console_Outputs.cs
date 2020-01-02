CxList methods = Find_Methods();
CxList output = All.NewCxList();

output.Add(All.FindByName("out.print"));
output.Add(All.FindByName("out.println"));  
output.Add(All.FindByName("out.printf"));
output.Add(All.FindByName("out.format"));
output.Add(All.FindByName("out.append"));
output.Add(All.FindByName("out.write"));
output.Add(All.FindByName("*.out.print"));
output.Add(All.FindByName("*.out.println"));  
output.Add(All.FindByName("*.out.printf"));
output.Add(All.FindByName("*.out.format"));
output.Add(All.FindByName("*.out.append"));
output.Add(All.FindByName("*.out.write"));
output.Add(All.FindByName("System.out.print"));
output.Add(All.FindByName("System.out.println"));
output.Add(All.FindByName("System.out.printf"));
output.Add(All.FindByName("System.out.format"));
output.Add(All.FindByName("System.out.append"));
output.Add(All.FindByName("System.out.write"));
output.Add(All.FindByName("*.System.out.print"));
output.Add(All.FindByName("*.System.out.println"));
output.Add(All.FindByName("*.System.out.printf"));
output.Add(All.FindByName("*.System.out.format"));
output.Add(All.FindByName("*.System.out.append"));
output.Add(All.FindByName("*.System.out.write"));
output.Add(methods.FindByShortNames(new List<string>{
		"print", "printf", "println", "sprintf"}));

result = output - Find_Dead_Code_Contents();