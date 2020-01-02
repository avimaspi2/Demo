CxList methods = Find_Methods();
CxList classes = Find_Class_Decl();

CxList getOutputStream = All.FindByMemberAccess("Socket.getOutputStream");
CxList outputStreamCommands = getOutputStream.GetMembersOfTarget();
outputStreamCommands = 
	outputStreamCommands.FindByShortName("print*") +
	outputStreamCommands.FindByShortName("write*");

CxList output = 
	All.FindByName("response.*", false) +
	All.FindByMemberAccess("HttpServletResponse.*") +
				
	All.FindByMemberAccess("JspWriter.print*") +
				
	outputStreamCommands +

	All.FindByMemberAccess("Text.setText") + 	
	All.FindByMemberAccess("TextComponent.setText") +
	All.FindByMemberAccess("TextArea.setText") +
	All.FindByMemberAccess("TextField.setText") + 
	All.FindByMemberAccess("Label.setText*") +
	All.FindByMemberAccess("JTextPane.set*") + 
	All.FindByShortName("printStackTrace") + 

	All.FindByMemberAccess("ZipOutputStream.*") +
 
	All.FindByName("out.*");

output -= output.FindByShortName("get*");

getOutputStream = output.FindByShortName("getOutputStream");
outputStreamCommands = getOutputStream.GetMembersOfTarget();
outputStreamCommands = 
	outputStreamCommands.FindByShortName("print*") +
	outputStreamCommands.FindByShortName("write*");

output -= getOutputStream;
output.Add(outputStreamCommands);

CxList strWriter = All.FindByType("StringWriter");
CxList prnWriter = All.FindByMemberAccess("PrintWriter.print*");
prnWriter.Add(All.FindByMemberAccess("ServletOutputStream.print*"));
prnWriter = prnWriter - prnWriter.DataInfluencedBy(strWriter);

CxList ResponseWriter = All.FindByMemberAccess("ServletResponse.getWriter").
	GetMembersOfTarget().FindByType(typeof(MethodInvokeExpr));
	
ResponseWriter = ResponseWriter.FindByShortName("write*") + ResponseWriter.FindByShortName("print*");

/* struts 2 */
CxList jspTags = Find_Output_Tags();
CxList jspOutputs = 
	jspTags.FindByMemberAccess("_checkbox.label_", false)
	+ jspTags.FindByMemberAccess("_checkbox.name_", false)
	+ jspTags.FindByMemberAccess("_checkboxlist.name_", false) 
	+ jspTags.FindByMemberAccess("_combobox.name_", false) 
	+ jspTags.FindByMemberAccess("_doubleselect.name_", false) 
	+ jspTags.FindByMemberAccess("_elseif.test_", false)
	+ jspTags.FindByMemberAccess("_file.name_", false) 
	+ jspTags.FindByMemberAccess("_form.action_", false)
	+ jspTags.FindByMemberAccess("_hidden.name_", false) 
	+ jspTags.FindByMemberAccess("_hidden.value_", false)
	+ jspTags.FindByMemberAccess("_if.test_", false)
	+ jspTags.FindByMemberAccess("_include.value_", false)
	+ jspTags.FindByMemberAccess("_label.key_", false)
	+ jspTags.FindByMemberAccess("_label.value_", false)
	+ jspTags.FindByMemberAccess("_optiontransferselect.name_", false) 
	+ jspTags.FindByMemberAccess("_param.name_", false) 
	+ jspTags.FindByMemberAccess("_param.value_", false) 
	+ jspTags.FindByMemberAccess("_password.label_", false) 
	+ jspTags.FindByMemberAccess("_password.name_", false) 
	+ jspTags.FindByMemberAccess("_property.default_", false)
	+ jspTags.FindByMemberAccess("_property.value_", false)
	+ jspTags.FindByMemberAccess("_radio.label_", false)
	+ jspTags.FindByMemberAccess("_radio.list_", false)
	+ jspTags.FindByMemberAccess("_radio.name_", false) 
	+ jspTags.FindByMemberAccess("_select.label_", false)
	+ jspTags.FindByMemberAccess("_select.name_", false) 
	+ jspTags.FindByMemberAccess("_set.value_", false)
	+ jspTags.FindByMemberAccess("_submit.key_", false)
	+ jspTags.FindByMemberAccess("_submit.name_", false) 
	+ jspTags.FindByMemberAccess("_text.name_", false)
	+ jspTags.FindByMemberAccess("_textarea.label_", false)
	+ jspTags.FindByMemberAccess("_textarea.name_", false)
	+ jspTags.FindByMemberAccess("_textfield.label_", false) 
	+ jspTags.FindByMemberAccess("_textfield.name_", false) 
	+ jspTags.FindByMemberAccess("_updownselect.name_", false)
	+ jspTags.FindByMemberAccess("_url.value_", false);

CxList struts2Outputs = jspOutputs;

CxList jspCode = Find_Jsp_Code(); 
struts2Outputs.Add(
	jspCode.FindByShortName("actionmessage")
	+ jspCode.FindByShortName("actionerror")
	+ jspCode.FindByShortName("fielderror"));
struts2Outputs = All.GetByAncs(struts2Outputs); // too many false positives - the problem is the text.
/* end struts 2 */

/* spring */
CxList mav2 = jspCode.FindByMemberAccess("response.write");
CxList mav = All.GetByAncs(mav2) - mav2;
mav -= mav.GetByAncs(output);
/* end spring */

/* jersey */
// Find all attributes of consumes
CxList consumesAttr = All.FindByCustomAttribute("Consumes");
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


result = output + 
	prnWriter + 
	ResponseWriter + 
	struts2Outputs + 
	mav +
	jersey;

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

result -= Find_Dead_Code_Contents();