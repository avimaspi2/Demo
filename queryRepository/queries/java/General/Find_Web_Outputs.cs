CxList methods = Find_Methods();
//CxList classes = Find_Class_Decl();
CxList unknownRefs = Find_UnknownReference();

CxList methodsDecl = Find_MethodDeclaration();

CxList getOutputStream = All.FindByMemberAccess("Socket.getOutputStream");
CxList outputStreamCommands = getOutputStream.GetMembersOfTarget();
outputStreamCommands = outputStreamCommands.FindByShortNames(new List<string> {"print*","write*"});

//searching for just methods
CxList response = methods.FindByMemberAccess("response.*");

List<string> responseMethods = new List<string> { "send*", "set*" , "parse*" ,"write*" };

response = response.FindByShortNames(responseMethods, false);

CxList setEvent = response.FindByShortName("setEvent");
response -= setEvent;

CxList This = response.GetTargetOfMembers().FindByType(typeof(ThisRef));
CxList thisMembers = This.GetMembersOfTarget();

CxList implemented = methodsDecl.FindDefinition(thisMembers);
CxList haveImplement = thisMembers.FindAllReferences(implemented);
//subtract all the incorrect results from response
response -= haveImplement; 

CxList output = All.NewCxList();

output.Add(response);

output.Add(All.FindByMemberAccess("JspWriter.print*"));
				
output.Add(outputStreamCommands);

output.Add(All.FindByMemberAccess("Text.setText"));
output.Add(All.FindByMemberAccess("TextComponent.setText"));
output.Add(All.FindByMemberAccess("TextArea.setText"));
output.Add(All.FindByMemberAccess("TextField.setText"));
output.Add(All.FindByMemberAccess("Label.setText*"));
output.Add(All.FindByMemberAccess("JTextPane.set*"));
CxList pst = methods.FindByShortName("printStackTrace");
CxList pstBlackList = All.FindByTypes(new string[]{"ByteArrayOutputStream", "CharArrayWriter", "StringWriter"});
pstBlackList.Add(unknownRefs.FindAllReferences(pstBlackList.GetAncOfType(typeof(Declarator))));
output.Add(pst - pst.FindByParameters(pstBlackList));
 
output.Add(All.FindByName("out.*"));

getOutputStream = output.FindByShortName("getOutputStream");
 
output -= output.FindByShortName("get*");

outputStreamCommands = getOutputStream.GetMembersOfTarget();

outputStreamCommands = outputStreamCommands.FindByShortNames(new List<string> {"print*","write*"});

output.Add(outputStreamCommands);

output.Add(All.FindByMemberAccess("ZipOutputStream.*"));

CxList http_methods = All.FindByMemberAccess("HttpServletResponse.*");
http_methods -= http_methods.FindByShortName("sendRedirect");
output.Add(All.GetParameters(http_methods));

CxList servlet_methods = All.FindByMemberAccess("ServletOutputStream.*");
CxList servlet_outputs = servlet_methods.FindByShortNames(new List<string> {"print*","write*"});
output.Add(servlet_outputs);

CxList strWriter = All.FindByType("StringWriter");
CxList prnWriter = All.FindByMemberAccess("PrintWriter.print*");
prnWriter.Add(All.FindByMemberAccess("PrintWriter.format"));
prnWriter.Add(All.FindByMemberAccess("PrintWriter.append"));
prnWriter.Add(All.FindByMemberAccess("ServletOutputStream.print*"));
prnWriter = prnWriter - prnWriter.DataInfluencedBy(strWriter);
prnWriter.Add(methods.FindByMemberAccess("PrintWriter.write").InfluencedBy(http_methods.FindByShortName("getWriter")));

CxList ResponseWriter = All.FindByMemberAccess("ServletResponse.getWriter").
	GetMembersOfTarget().FindByType(typeof(MethodInvokeExpr));
ResponseWriter = ResponseWriter.FindByShortNames(new List<string>{"append","format","print*","write*"});


/* struts 2 */
CxList jspTags = Find_Output_Tags();
CxList jspOutputs = All.NewCxList();
jspOutputs.Add(jspTags.FindByMemberAccess("_checkbox.label_", false));
jspOutputs.Add(jspTags.FindByMemberAccess("_checkbox.name_", false));
jspOutputs.Add(jspTags.FindByMemberAccess("_checkboxlist.name_", false));
jspOutputs.Add(jspTags.FindByMemberAccess("_combobox.name_", false));
jspOutputs.Add(jspTags.FindByMemberAccess("_doubleselect.name_", false));
jspOutputs.Add(jspTags.FindByMemberAccess("_elseif.test_", false));
jspOutputs.Add(jspTags.FindByMemberAccess("_file.name_", false)); 
jspOutputs.Add(jspTags.FindByMemberAccess("_form.action_", false));
jspOutputs.Add(jspTags.FindByMemberAccess("_hidden.name_", false));
jspOutputs.Add(jspTags.FindByMemberAccess("_hidden.value_", false));
jspOutputs.Add(jspTags.FindByMemberAccess("_if.test_", false));
jspOutputs.Add(jspTags.FindByMemberAccess("_include.value_", false));
jspOutputs.Add(jspTags.FindByMemberAccess("_label.key_", false));
jspOutputs.Add(jspTags.FindByMemberAccess("_label.value_", false));
jspOutputs.Add(jspTags.FindByMemberAccess("_optiontransferselect.name_", false));
jspOutputs.Add(jspTags.FindByMemberAccess("_param.name_", false));
jspOutputs.Add(jspTags.FindByMemberAccess("_param.value_", false));
jspOutputs.Add(jspTags.FindByMemberAccess("_password.label_", false));
jspOutputs.Add(jspTags.FindByMemberAccess("_password.name_", false));
jspOutputs.Add(jspTags.FindByMemberAccess("_property.default_", false));
jspOutputs.Add(jspTags.FindByMemberAccess("_property.value_", false));
jspOutputs.Add(jspTags.FindByMemberAccess("_radio.label_", false));
jspOutputs.Add(jspTags.FindByMemberAccess("_radio.list_", false));
jspOutputs.Add(jspTags.FindByMemberAccess("_radio.name_", false));
jspOutputs.Add(jspTags.FindByMemberAccess("_select.label_", false));
jspOutputs.Add(jspTags.FindByMemberAccess("_select.name_", false));
jspOutputs.Add(jspTags.FindByMemberAccess("_set.value_", false));
jspOutputs.Add(jspTags.FindByMemberAccess("_submit.key_", false));
jspOutputs.Add(jspTags.FindByMemberAccess("_submit.name_", false));
jspOutputs.Add(jspTags.FindByMemberAccess("_text.name_", false));
jspOutputs.Add(jspTags.FindByMemberAccess("_textarea.label_", false));
jspOutputs.Add(jspTags.FindByMemberAccess("_textarea.name_", false));
jspOutputs.Add(jspTags.FindByMemberAccess("_textfield.label_", false));
jspOutputs.Add(jspTags.FindByMemberAccess("_textfield.name_", false));
jspOutputs.Add(jspTags.FindByMemberAccess("_updownselect.name_", false));
jspOutputs.Add(jspTags.FindByMemberAccess("_url.value_", false));

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

/* //////////////// */


CxList struts2Outputs = All.NewCxList();
struts2Outputs.Add(jspOutputs);

CxList jspCode = Find_Jsp_Code(); 
struts2Outputs.Add(jspCode.FindByShortNames(new List<string> {"actionmessage","actionerror","fielderror"}));
			
//	jspCode.FindByShortName("actionmessage")
//	+ jspCode.FindByShortName("actionerror")
//	+ jspCode.FindByShortName("fielderror"));
struts2Outputs = All.GetByAncs(struts2Outputs); // too many false positives - the problem is the text.
/* end struts 2 */

/* spring */
CxList mav2 = jspCode.FindByMemberAccess("response.write");
CxList mav = All.GetByAncs(mav2) - mav2;
mav -= mav.GetByAncs(output);
/* end spring */

/* jersey */
// Find all attributes of consumes
CxList customAttribute = Find_CustomAttribute();
CxList consumesAttr = customAttribute.FindByCustomAttribute("Consumes");
CxList consumes = consumesAttr.GetFathers();

// Simple case - if this is a method attribute
CxList consumesMethod = consumes.FindByType(typeof(MethodDecl));

// Complex case - if this is a class attribute, we need to take all of its methods
CxList consumesClass = consumes.FindByType(typeof(ClassDecl));
CxList classConsumesMethod = All.GetByAncs(consumesClass).FindByType(typeof(MethodDecl));
// Remove methods that are in sub-classes of the "Consumes"-class
classConsumesMethod -= classConsumesMethod.GetByAncs(classConsumesMethod.GetAncOfType(typeof(ClassDecl)) - consumesClass);
// Add the class methods
consumesMethod.Add(classConsumesMethod);

//Get the return statements of the relevant methods
CxList returnStatements = All.GetByMethod(consumesMethod).FindByType(typeof(ReturnStmt));

CxList jersey = All.FindByFathers(returnStatements);
/* end jersey */

/* ResponseBody CustomAttribute */
CxList responseBody = customAttribute.FindByCustomAttribute("ResponseBody");
// Get relevan methods
CxList responseBodyMethods = responseBody.GetFathers();
CxList returns = Find_ReturnStmt().GetByAncs(responseBodyMethods);
// Get the returned objects
CxList returned = All.FindByFathers(returns);
// Filter the returned objects
CxList hasMembers = returned.GetTargetsWithMembers();
CxList members = hasMembers.GetRightmostMember();
CxList hasNoMembers = returned - hasMembers;
result.Add(members);
result.Add(hasNoMembers);
/* ResponseBody CustomAttribute */

CxList jsfFrameworksOutputs = methods.FindByShortName("CxJsfOutput");

CxList frameworkFactoryOutputs = methods.FindByShortName("CxOutput");

result.Add(output); 
result.Add(prnWriter);
result.Add(ResponseWriter);
result.Add(struts2Outputs);
result.Add(fmtTagOutputs);
result.Add(methodGetContentsReturnValues);
result.Add(mav);
result.Add(jersey);
result.Add(Find_Citrus_Outputs());
result.Add(jsfFrameworksOutputs);
result.Add(frameworkFactoryOutputs);

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

/*Remove jaxrsPrivateMethods*/
CxList pathAnnotations = customAttribute.FindByCustomAttribute("Path");
CxList jaxrsClass = pathAnnotations.GetAncOfType(typeof(ClassDecl));

CxList jaxrsMethods = methodsDecl.GetByAncs(jaxrsClass);
CxList jaxrsPrivateMethods = jaxrsMethods.FindByFieldAttributes(Modifiers.Private);
result -= result.GetByAncs(jaxrsPrivateMethods);
/* end Remove jaxrsPrivateMethods */ 

/* end Response cleanup */
 
//result.Add(Find_AtgDspOutputs());

CxList toRemove = Find_Dead_Code_Contents();
toRemove.Add(http_methods.FindByParameters(result.FindByType(typeof(Param))));
toRemove.Add(All.FindByMemberAccess("Urls.identityProviderAuthnResponse").GetMembersOfTarget());
result -= toRemove;