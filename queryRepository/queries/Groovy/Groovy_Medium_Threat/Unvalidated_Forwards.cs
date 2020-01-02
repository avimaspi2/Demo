CxList inputs = Find_Interactive_Inputs();

CxList requestDispatcher = All.FindByMemberAccess("HttpServletRequest.getRequestDispatcher");

CxList rdParams = All.FindByType(typeof(UnknownReference)).GetParameters(requestDispatcher); 
CxList forward = All.FindByMemberAccess("RequestDispatcher.forward"); 
forward += All.FindByShortName("forward") + All.FindByShortName("redirect"); 

result = inputs.DataInfluencingOn(forward);