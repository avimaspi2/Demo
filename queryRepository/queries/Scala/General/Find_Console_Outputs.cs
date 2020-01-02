CxList output = All.NewCxList();

output.Add(All.FindByName("out.printf"));
output.Add(All.FindByName("out.format"));
output.Add(All.FindByName("out.append"));
output.Add(All.FindByName("out.write"));
 
output.Add(All.FindByName("*.out.printf"));
output.Add(All.FindByName("*.out.format"));
output.Add(All.FindByName("*.out.append"));
output.Add(All.FindByName("*.out.write"));
output.Add(All.FindByName("System.out.printf"));
output.Add(All.FindByName("System.out.format"));
output.Add(All.FindByName("System.out.append"));
output.Add(All.FindByName("System.out.write"));
output.Add(All.FindByName("*.System.out.printf"));
output.Add(All.FindByName("*.System.out.format"));
output.Add(All.FindByName("*.System.out.append"));
output.Add(All.FindByName("*.System.out.write"));
output.Add(All.FindByShortNames(new List<string> {"println", "print"}));
CxList forEach = (All.FindByShortName("foreach").FindByType(typeof(MethodInvokeExpr)));
CxList forEachParameters = All.GetParameters(forEach);
//Get forEach parameters that are outputs
CxList outputForEachParameters = output * forEachParameters;
//Keep only outputs that are methods
output = output * Find_Methods();
//add only non Param forEach parameters
output.Add(outputForEachParameters - outputForEachParameters.FindByType(typeof(Param)));


CxList console = All.FindByMemberAccess("Console.*");

output.Add(console.FindByShortName("printf"));
output.Add(console.FindByShortName("format"));

CxList PrintWriter = All.FindByMemberAccess("PrintWriter.*");

output.Add(PrintWriter.FindByShortNames(new List<string> {"format", "printf", "write"}));


result = output;