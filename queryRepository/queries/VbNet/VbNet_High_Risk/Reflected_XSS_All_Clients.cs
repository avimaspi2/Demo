if(All.isWebApplication)
{
	CxList inputs = Find_Interactive_Inputs(); 
	
	inputs -= All.GetAssignee(inputs);
	
	CxList outputs = Find_XSS_Outputs() - Find_Console_Outputs();
	
	outputs = outputs.FindByAssignmentSide(CxList.AssignmentSide.Left);	

	CxList sanitized = Find_XSS_Sanitize();

	result = All.FindXSS(inputs, outputs, sanitized);	
}