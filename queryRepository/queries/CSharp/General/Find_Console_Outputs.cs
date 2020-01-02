CxList outputs = All.FindByMemberAccess("Console.Write*");
outputs.Add(All.FindByMemberAccess("Console.Out*"));

CxList fathersWithoutAssignment = outputs.GetFathers();
fathersWithoutAssignment -= fathersWithoutAssignment.FindByType(typeof(AssignExpr));

result.Add(outputs.FindByAssignmentSide(CxList.AssignmentSide.Left));
result.Add(outputs.FindByFathers(fathersWithoutAssignment));