CxList deadCode = Find_Dead_Code_Contents();

CxList getWriter = 
	All.FindByMemberAccess("HttpServletResponse.getWriter") +
	All.FindByName("*response.getWriter") +  
	All.FindByName("*Response.getWriter");

getWriter -= getWriter.GetMembersOfTarget().GetTargetOfMembers();

CxList getFromSystem = All.FindByMemberAccess("System.getenv");

CxList inputs = getWriter + getFromSystem;

CxList outputs = Find_Interactive_Outputs();
outputs = outputs.FindByShortName("print*") + outputs.FindByShortName("write*");

CxList sanitize = Find_Integers() + deadCode;


result = outputs.InfluencedByAndNotSanitized(inputs, sanitize);