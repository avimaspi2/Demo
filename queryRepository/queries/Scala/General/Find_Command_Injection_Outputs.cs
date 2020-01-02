CxList allMethods = Find_Methods();
CxList processStart = All.FindByMemberAccess("ProcessBuilder.start");
CxList processCommandWithStart = All.FindByMemberAccess("ProcessBuilder.command").GetTargetOfMembers().DataInfluencingOn(processStart).GetMembersOfTarget();
CxList exec = All.FindByMemberAccess("Runtime.exec");
exec.Add(All.FindByMemberAccess("getRuntime.exec"));
exec.Add(All.FindByMemberAccess("System.exec"));
exec.Add(All.FindByMemberAccess("Call.setOperationName"));
exec.Add(processStart);
exec.Add(All.GetParameters(allMethods.FindByMemberAccess("process.stringToProcess")));
exec.Add(All.GetParameters(processCommandWithStart));

CxList outputStream = All.FindByMemberAccess("Process.getOutputStream");
CxList osWrite = All.FindByMemberAccess("OutputStream.write");

// Find only Process.OutputStream.write
CxList processOStreamWrite = osWrite.InfluencedBy(outputStream).GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
exec.Add(processOStreamWrite);

//Scala exec
CxList scalaExec = All.FindByTypes(new String[]{"Process","string","Seq"}).GetMembersOfTarget();
scalaExec = scalaExec.FindByShortNames(new List<string>{"!!","!","lines"});
exec.Add(scalaExec.GetTargetOfMembers());

result = exec;