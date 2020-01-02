CxList methods = Find_Methods();

result = methods.FindByShortName("puts") +
	methods.FindByShortName("print") + 
	methods.FindByShortName("printf");


/*
CxList outputs = ll.FindByMemberAccess("Console.Write*") +
	 				All.FindByMemberAccess("Console.Out*");

CxList fathersWithoutAssignment = 	outputs.GetFathers() - 
									outputs.GetFathers().FindByType(typeof(AssignExpr));

result = 	outputs.FindByAssignmentSide(CxList.AssignmentSide.Left) + 
			outputs.FindByFathers(fathersWithoutAssignment);

*/