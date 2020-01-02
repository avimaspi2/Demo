CxList methods = Find_Methods();
CxList classes = Find_Class_Decl();

CxList getOutputStream = All.FindByMemberAccess("Socket.getOutputStream");
CxList outputStreamCommands = getOutputStream.GetMembersOfTarget();
outputStreamCommands = outputStreamCommands.FindByShortNames(new List<string> {"print*","write*"});

CxList response = All.FindByMemberAccess("response.*");

CxList output = All.NewCxList();

output.Add(response);	
output.Add(outputStreamCommands);
output.Add(All.FindByMemberAccess("Text.setText"));
output.Add(All.FindByMemberAccess("TextComponent.setText"));
output.Add(All.FindByMemberAccess("TextArea.setText"));
output.Add(All.FindByMemberAccess("TextField.setText"));
output.Add(All.FindByMemberAccess("Label.setText*"));
output.Add(All.FindByMemberAccess("JTextPane.set*"));
output.Add(All.FindByShortName("printStackTrace"));
 
CxList systOut = All.FindByType("System.out") + All.FindByMemberAccess("System.out");
output.Add(systOut.GetMembersOfTarget().
	FindByShortNames(new List<string>{"write", "append", "format", "print", "println"}, false));

getOutputStream = output.FindByShortName("getOutputStream");
 
output -= output.FindByShortName("get*");

outputStreamCommands = getOutputStream.GetMembersOfTarget();

outputStreamCommands = outputStreamCommands.FindByShortNames(new List<string> {"print*","write*"});

output.Add(outputStreamCommands);

output.Add(All.FindByMemberAccess("ZipOutputStream.*"));

CxList http_methods = All.FindByMemberAccess("HttpServletResponse.*");
http_methods -= http_methods.FindByShortName("sendRedirect");
output.Add(All.GetParameters(http_methods));

CxList strWriter = All.FindByType("StringWriter");
CxList prnWriter = All.FindByMemberAccess("PrintWriter.print*");
prnWriter.Add(All.FindByMemberAccess("PrintWriter.format"));
prnWriter.Add(All.FindByMemberAccess("PrintWriter.append"));
prnWriter.Add(All.FindByMemberAccess("ServletOutputStream.print*"));
prnWriter = prnWriter - prnWriter.DataInfluencedBy(strWriter);

CxList ResponseWriter = All.FindByMemberAccess("ServletResponse.getWriter").
	GetMembersOfTarget().FindByType(typeof(MethodInvokeExpr));
	
ResponseWriter = ResponseWriter.FindByShortNames(new List<string>{"append","format","print*","write*"});

/* Special handling for key in "fmt:" taglib. The "key" field is translated to an assignment to "fmt_message_Key"
   and considered an output. */
CxList fmtTagOutputs = All.FindByShortName("fmt_message_Key");
/* //////////////// */

/* Special handling taglib as class. 
   The output of a method getContents() in a class inheriting from ListResourceBundle 
   is considered an output. */
CxList contents = All.FindByShortName("getContents").FindByType(typeof(MethodDecl));
CxList methodGetContents = contents.GetByAncs(All.InheritsFrom("ListResourceBundle"));
methodGetContents = All.GetByAncs(methodGetContents);
CxList methodReturn = methodGetContents.FindByType(typeof(ReturnStmt));
CxList methodGetContentsReturnValues = methodGetContents.GetByAncs(methodReturn) - methodReturn;

result.Add(output); 
result.Add(prnWriter);
result.Add(ResponseWriter);
result.Add(fmtTagOutputs);
result.Add(methodGetContentsReturnValues);
result.Add(Find_WebServices_Outputs());

result -= result.FindByShortName("sendRedirect");
result -= result.FindByShortName("safeSendRedirect"); // ESAPI
result -= result.FindByMemberAccess("Response.ok");
result -= result.FindByName("*Response.ok");
result -= result.FindByMemberAccess("response.Import");

/* Response cleanup */
CxList Response = result.FindByMemberAccess("Response.*", true);
Response.Add(Response.GetMembersOfTarget());
Response.Add(Response.GetMembersOfTarget());
Response.Add(Response.GetMembersOfTarget());
Response.Add(Response.GetMembersOfTarget());
Response.Add(Response.GetMembersOfTarget());
Response.Add(Response.GetMembersOfTarget());
result -= Response;
Response -= Response.GetTargetOfMembers();
result.Add(Response);
/* end Response cleanup */

result -= http_methods.FindByParameters(result.FindByType(typeof(Param)));