CxList objCreate = Find_ObjectCreations();
CxList methods = Find_Methods();
CxList inLeftOfAssign = Find_Assign_Lefts();

CxList windowsRequireRefs = Find_Require("windows");
CxList registries = All.NewCxList();
registries.Add(windowsRequireRefs.GetMembersOfTarget());
registries.Add(windowsRequireRefs * methods);

CxList leftOfRegistries = inLeftOfAssign.FindByFathers(registries.GetAncOfType(typeof(AssignExpr)));
CxList allWindowsRegistries = All.FindAllReferences(leftOfRegistries);

result.Add(All.FindAllReferences(allWindowsRegistries).GetMembersOfTarget());


CxList winregs = Find_Require("winreg", 2);

CxList winregsObj = (winregs * objCreate).GetAssignee();
winregs.Add(winregsObj);

result.Add(All.FindAllReferences(winregs).GetMembersOfTarget());
result -= XS_Find_All();