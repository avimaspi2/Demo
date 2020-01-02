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

CxList console = All.FindByMemberAccess("Console.*");

output.Add(console.FindByShortName("printf"));
output.Add(console.FindByShortName("format"));

CxList PrintWriter = All.FindByMemberAccess("PrintWriter.*");

output.Add(PrintWriter.FindByShortName("format"));
output.Add(PrintWriter.FindByShortName("print"));
output.Add(PrintWriter.FindByShortName("printf"));
output.Add(PrintWriter.FindByShortName("println"));
output.Add(PrintWriter.FindByShortName("write"));

result = output - Find_Dead_Code_Contents();