// From https://golang.org/pkg/os/exec/
// Exec outputs using OS Command calls 

CxList variables = Find_UnknownReferences();

/*** Finding exec.Command function ***/
CxList cmdFunctions = All.FindByMemberAccess("\"os/exec\".Command*");
CxList cmdAssignees = cmdFunctions.GetAssignee();

/*** Search for Cmd instances (cmd := exec.Command()), methods (exec.Command().Run()) and *Cmd type (cmd.Run()) ***/
CxList command = All.NewCxList();
command.Add(cmdFunctions);
command.Add(All.FindAllReferences(cmdAssignees));
command.Add(variables.FindByPointerType("exec.Cmd", true));

/*** Finding methods of Cmd: Run, Start, CombinedOutput, Output ***/
List<string> cmdMethods = new List<string>() { "Run", "Start", "CombinedOutput", "Output" };
CxList cmdMembers = command.GetMembersOfTarget();
result = cmdMembers.FindByShortNames(cmdMethods);