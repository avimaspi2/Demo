CxList outputs = All.FindByMemberAccess("Console.Write*", false);
outputs.Add(All.FindByMemberAccess("Console.Out*", false));

CxList fathersWithoutAssignment = 	outputs.GetFathers() - 
									outputs.GetFathers().FindByType(typeof(AssignExpr));

result = outputs.FindByAssignmentSide(CxList.AssignmentSide.Left);
result.Add(outputs.FindByFathers(fathersWithoutAssignment));