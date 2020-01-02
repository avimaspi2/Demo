result = All.FindByName("Response");
result.Add(All.FindByName("Response.BinaryWrite"));
result.Add(All.FindByName("Response.Write"));
result.Add(All.FindByName("Response.WriteFile"));

	// page response
result.Add(All.FindByName("Page.Response"));
result.Add(All.FindByName("Page.Response.BinaryWrite"));
result.Add(All.FindByName("Page.Response.Write"));
result.Add(All.FindByName("Page.Response.WriteFile"));
	
	//current response
result.Add(All.FindByName("*.Current.Response"));
result.Add(All.FindByName("*.Current.Response.BinaryWrite"));
result.Add(All.FindByName("*.Current.Response.Write"));
result.Add(All.FindByName("*.Current.Response.WriteFile"));
	
	//HttpContext
result.Add(All.FindByMemberAccess("Context.Response"));
result.Add(All.FindByMemberAccess("HttpContext.Response"));


    //WebListeners for .Net Core
List <string> addAppendList = new List<string> (){"Append", "Add"};
List <string> writeList = new List<string> (){"WriteTimeout", "BeginWrite", "Write", "Write", "WriteByte"};

CxList context = All.FindByMemberAccess("WebListener.AcceptAsync");
CxList assigned = context.GetAssignee();
context.Add(All.FindAllReferences(assigned));

CxList contextMembers = context.GetMembersOfTarget();

CxList contextResponse = contextMembers.FindByShortName("Response");
CxList respAssigned = contextResponse.GetAssignee();
respAssigned = All.FindAllReferences(respAssigned);
CxList members = respAssigned.GetMembersOfTarget();

CxList headers = All.FindByMemberAccess("Response.Headers");
headers.Add(members.FindByShortName("Headers"));
CxList headerMembers = headers.GetMembersOfTarget().FindByShortNames(addAppendList);
result.Add(All.GetParameters(headerMembers));

CxList bodies = All.FindByMemberAccess("Response.Body");
bodies.Add(members.FindByShortName("Body"));
CxList bodyMembers = bodies.GetMembersOfTarget().FindByShortNames(writeList);
result.Add(All.GetParameters(bodyMembers, 0));

result -= result.FindByType(typeof(Param));