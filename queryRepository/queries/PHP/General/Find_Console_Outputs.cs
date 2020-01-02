CxList outputs = 	All.FindByMemberAccess("Console.Write*") +
	 				All.FindByMemberAccess("Console.Out*");

CxList fathersWithoutAssignment = 	outputs.GetFathers() - 
									outputs.GetFathers().FindByType(typeof(AssignExpr));

result = 	outputs.FindByAssignmentSide(CxList.AssignmentSide.Left) + 
			outputs.FindByFathers(fathersWithoutAssignment);