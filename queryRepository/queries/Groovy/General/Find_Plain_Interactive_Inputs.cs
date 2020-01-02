CxList methodDecl = All.FindByType(typeof(MethodDecl));

CxList inputs = All.NewCxList();
inputs.Add(All.FindByMemberAccess("request.getCharacterEncoding"));
inputs.Add(All.FindByMemberAccess("request.getContentType"));
inputs.Add(All.FindByMemberAccess("request.ContentType"));
inputs.Add(All.FindByMemberAccess("request.getInputStream"));
inputs.Add(All.GetParameters(All.FindByMemberAccess("request.getParameter"), 0));
inputs.Add(All.FindByMemberAccess("request.getParameterValues"));
inputs.Add(All.FindByMemberAccess("request.ParameterValues"));
inputs.Add(All.FindByMemberAccess("request.getReader"));
inputs.Add(All.FindByMemberAccess("request.getParameterNames"));
inputs.Add(All.FindByMemberAccess("request.ParameterNames"));
inputs.Add(All.FindByMemberAccess("request.getParameterMap") );
inputs.Add(All.FindByMemberAccess("request.ParameterMap"));
inputs.Add(All.FindByMemberAccess("request.getHeader"));
inputs.Add(All.FindByMemberAccess("request.getHeaders"));
inputs.Add(All.FindByMemberAccess("request.getHeaderNames"));
inputs.Add(All.FindByMemberAccess("request.getQueryString"));
inputs.Add(All.FindByMemberAccess("request.getRequestedSessionId"));
inputs.Add(All.FindByMemberAccess("request.getPathInfo"));
inputs.Add(All.FindByMemberAccess("request.getRemoteUser"));
inputs.Add(All.FindByMemberAccess("request.getRequestURI") );
inputs.Add(All.FindByMemberAccess("request.getRequestURL"));
inputs.Add(All.FindByMemberAccess("MultipartHttpServletRequest.getFile"));
inputs.Add(All.FindByMemberAccess("RequestContext.get*"));
inputs.Add(All.FindByMemberAccess("Text.getText"));
inputs.Add(All.FindByMemberAccess("TextComponent.getText"));
inputs.Add(All.FindByMemberAccess("Socket.getInputStream"));
inputs.Add(All.FindByMemberAccess("JTextComponent.get*"));
inputs.Add(All.FindByMemberAccess("TextArea.getText"));
inputs.Add(All.FindByMemberAccess("TextField.getText"));
inputs.Add(All.FindByMemberAccess("wmgetRequestedSessionId.getRequestedSessionId*"));
inputs.Add(All.FindByMemberAccess("WebSession.getRequest"));
inputs.Add(All.FindByMemberAccess("Params.*"));



CxList cookiesInput = All.NewCxList();
cookiesInput.Add(All.FindByMemberAccess("Cookie.get*"));
cookiesInput.Add(All.FindByMemberAccess("request.getCookies"));
cookiesInput.Add(All.FindByMemberAccess("request.cookies"));

CxList webSessionMethods = All.FindByMemberAccess("WebSession.getRequest");
cookiesInput.Add(webSessionMethods.GetMembersOfTarget().FindByShortName("getCookies"));


//inputs.Add(cookiesInput - cookiesInput.DataInfluencingOn(cookiesInput));
inputs.Add(cookiesInput);

CxList ThisBaseMembers = All.FindByType(typeof(ThisRef));
ThisBaseMembers.Add(All.FindByType(typeof(BaseRef)));

ThisBaseMembers = ThisBaseMembers.GetMembersOfTarget();

inputs = inputs - ThisBaseMembers;

CxList methods = Find_Methods();
inputs.Add(methods.FindByShortName("getRawParameter"));

inputs.Add(All.GetParameters(methodDecl.FindByName("*.main")
	.FindByFieldAttributes(Modifiers.Public | Modifiers.Static)));
inputs.Add(All.GetParameters(methodDecl.FindByName("main")
	.FindByFieldAttributes(Modifiers.Public | Modifiers.Static)));

if(!All.isWebApplication){	
	CxList unknown_and_indexer_refs = All.NewCxList();
	unknown_and_indexer_refs.Add(All.FindByType(typeof(UnknownReference)));
	unknown_and_indexer_refs.Add(All.FindByType(typeof(IndexerRef)));
	
	CxList inCommand = All.FindByName("System.in.*");
	CxList parameters = All.GetParameters(inCommand, 0);
	inputs.Add(parameters);
	inputs.Add(All.FindByName("System.in"));
	inputs.Add(All.FindByMemberAccess("URLConnection.getInputStream"));
	inputs.Add(unknown_and_indexer_refs.FindByShortName("args"));
}


/* struts */
CxList executes = Find_Execute();

CxList actionFormParam = All.GetParameters(executes);
actionFormParam = actionFormParam.FindByType("HttpServletRequest") ;
actionFormParam.Add(actionFormParam.FindByType("ActionForm"));

CxList classes = Find_Class_Decl();
CxList actionFormClasses = classes.InheritsFrom("ActionForm");
CxList notResponseWrite = All - All.GetByAncs(All.FindByMemberAccess("response.write"));
foreach(CxList currFormClass in actionFormClasses)
{
	CSharpGraph gr = currFormClass.data.GetByIndex(0) as CSharpGraph;
	actionFormParam.Add(notResponseWrite.FindByMemberAccess(gr.ShortName + ".get*"));
}
/* end struts */

/* struts 2 */
CxList jspTags = Find_Input_Tags();
CxList jspTagsInputs = All.NewCxList();
jspTagsInputs.Add(jspTags.FindByMemberAccess("_checkbox.name_", false));
jspTagsInputs.Add(jspTags.FindByMemberAccess("_checkboxlist.name_", false));
jspTagsInputs.Add(jspTags.FindByMemberAccess("_combobox.name_", false) );
jspTagsInputs.Add(jspTags.FindByMemberAccess("_doubleselect.name_", false));
jspTagsInputs.Add(jspTags.FindByMemberAccess("_file.name_", false));
jspTagsInputs.Add(jspTags.FindByMemberAccess("_hidden.name_", false));
jspTagsInputs.Add(jspTags.FindByMemberAccess("_optiontransferselect.name_", false));
jspTagsInputs.Add(jspTags.FindByMemberAccess("_param.name_", false));
jspTagsInputs.Add(jspTags.FindByMemberAccess("_param.value_", false));
jspTagsInputs.Add(jspTags.FindByMemberAccess("_password.name_", false));
jspTagsInputs.Add(jspTags.FindByMemberAccess("_radio.name_", false));
jspTagsInputs.Add(jspTags.FindByMemberAccess("_select.name_", false));
jspTagsInputs.Add(jspTags.FindByMemberAccess("_submit.name_", false));
jspTagsInputs.Add(jspTags.FindByMemberAccess("_textarea.name_", false));
jspTagsInputs.Add(jspTags.FindByMemberAccess("_textfield.name_", false));
jspTagsInputs.Add(jspTags.FindByMemberAccess("_updownselect.name_", false));	
	 

//CxList struts2Inputs = Find_Strings().GetByAncs(jspTagsInputs);
CxList struts2Inputs = jspTagsInputs;
/* end struts 2 */

/* spring */
CxList action = All.NewCxList();
action.Add(All.FindByShortName("onSubmit"));
action.Add(All.FindByShortName("onBind"));
action.Add(All.FindByShortName("validatePage"));
action.Add(All.FindByShortName("processFinish"));
action.Add(All.FindByShortName("processCancel"));
action.Add(All.FindByShortName("doSubmitAction"));
action.Add(All.FindByShortName("onFormChange"));
action.Add(All.FindByShortName("processFormSubmission"));
action.Add(All.FindByShortName("referenceData"));

CxList controller = Find_Controllers();

action = action.GetByAncs(controller);	
CxList submitParam = All.GetParameters(action).FindByType("object");

CxList ThrowAwayController = All.InheritsFrom("ThrowAwayController");
CxList ThrowAwayControllerMembers = All.GetByAncs(ThrowAwayController);
CxList ThrowAwayControllerFields = ThrowAwayControllerMembers.FindAllReferences(ThrowAwayControllerMembers.FindByType(typeof(FieldDecl)));

CxList springAnnotations = Find_Request_Mapping_Params();
CxList annotatedMethods = All.FindByCustomAttribute("RequestMapping").GetFathers();
springAnnotations = springAnnotations.GetByAncs(annotatedMethods);

CxList pathAttributes = All.FindByCustomAttribute("Path");
CxList pathClasses = pathAttributes.GetAncOfType(typeof(ClassDecl));
CxList pathMethods = pathAttributes.GetAncOfType(typeof(MethodDecl));
pathMethods.Add(All.GetByAncs(pathClasses).FindByType(typeof(MethodDecl)));

CxList designatorsMethods = All.NewCxList();
designatorsMethods.Add(	All.FindByCustomAttribute("GET"));	
designatorsMethods.Add(All.FindByCustomAttribute("POST"));
designatorsMethods.Add(All.FindByCustomAttribute("PUT"));
designatorsMethods.Add(All.FindByCustomAttribute("DELETE"));
designatorsMethods.Add(All.FindByCustomAttribute("HEAD"));
designatorsMethods.Add(	All.FindByCustomAttribute("OPTIONS"));

designatorsMethods = designatorsMethods.GetAncOfType(typeof(MethodDecl));

CxList paramAnnotations = All.NewCxList();
paramAnnotations.Add(All.FindByCustomAttribute("MatrixParam"));
paramAnnotations.Add(All.FindByCustomAttribute("QueryParam"));
paramAnnotations.Add(All.FindByCustomAttribute("PathParam"));
paramAnnotations.Add(All.FindByCustomAttribute("CookieParam"));
paramAnnotations.Add(All.FindByCustomAttribute("HeaderParam"));
paramAnnotations.Add(All.FindByCustomAttribute("FormParam"));
paramAnnotations.Add(All.FindByCustomAttribute("Context"));

paramAnnotations = paramAnnotations.GetAncOfType(typeof(ParamDecl));

springAnnotations.Add(All.GetParameters(pathMethods * designatorsMethods));
springAnnotations.Add(paramAnnotations.GetParameters(pathMethods));
/* end spring */

/* jstl */
CxList jspCode = Find_Jsp_Code();
CxList jspParam = jspCode.FindByMemberAccess("param.*");
/* end jstl */

inputs.Add(All.FindByMemberAccess("SwingInputField.getText"));
inputs.Add(All.FindByMemberAccess("HyperlinkEvent.getURL"));

// GroovyInputs
CxList groovyInputs = All.FindByName("*showInputDialog");
CxList groovyInputsImpacts = All.DataInfluencedBy(groovyInputs);
CxList groovyInputsImpactsRefs = All.FindAllReferences(groovyInputsImpacts);
CxList groovyInputsImpactsRefsImpacts = All.DataInfluencedBy(groovyInputsImpactsRefs);
inputs.Add(groovyInputs);
inputs.Add(groovyInputsImpacts);
inputs.Add(groovyInputsImpactsRefs);
inputs.Add(groovyInputsImpactsRefsImpacts);
inputs.Add(All.FindAllReferences(groovyInputsImpactsRefsImpacts));
CxList socket = All.FindByMemberAccess("socket.get*");
CxList inputStreamReader = All.FindByShortName("InputStreamReader").GetAncOfType(typeof(ObjectCreateExpr));
CxList In = All.FindAllReferences(All.FindByType("BufferedReader"));
In = In.DataInfluencedBy(socket + inputStreamReader);
In = In.GetMembersOfTarget();
CxList bufferReader_read = In.FindByMemberAccess("BufferedReader.read*");
inputs -= inputs.GetByAncs(inputStreamReader).DataInfluencingOn(bufferReader_read);

inputs.Add(bufferReader_read);

/* dwr inputs */
CxList dwr = All.FindByFileName("*dwr.xml");
CxList strings = Find_Strings();
CxList dwrClasses = strings.DataInfluencingOn(dwr.FindByName("DWR.ALLOW.CREATE.PARAM.VALUE"));

CxList dwrInputs = All.NewCxList();
foreach (CxList cls in dwrClasses)
{
	CSharpGraph g = cls.data.GetByIndex(0) as CSharpGraph;
	string pathName = g.Text.Trim(new char[] {'"'});
	int lastDot = pathName.LastIndexOf(".");
	string className = pathName.Substring(lastDot + 1);	
	CxList relevantFile = All.FindByFileName("*" + pathName.Replace('.', '\\') + ".java");
	relevantFile.Add(All.FindByFileName("*" + pathName.Replace('.', '/') + ".java"));
	CxList dwrMethods = relevantFile.FindByType(typeof(MethodDecl));
	dwrMethods = dwrMethods.GetByAncs(relevantFile.FindByType(typeof(ClassDecl)).FindByShortName(className));
	dwrInputs.Add(relevantFile.GetParameters(dwrMethods));
}
/* end dwr inputs */

/* jersey */
// Find all attributes of Produces
CxList producesAttr = All.FindByCustomAttribute("Produces");
CxList produces = producesAttr.GetFathers();

// Simple case - if this is a method attribute
CxList producesMethod = produces.FindByType(typeof(MethodDecl));

// Complex case - if this is a class attribute, we need to take all of its methods
CxList producesClass = produces.FindByType(typeof(ClassDecl));
CxList classProducesMethod = All.GetByAncs(producesClass).FindByType(typeof(MethodDecl));
// Remove methods that are in sub-classes of the "produces"-class
classProducesMethod -= classProducesMethod.GetByAncs(classProducesMethod.GetAncOfType(typeof(ClassDecl)) - producesClass);
// Add the class methods
producesMethod.Add(classProducesMethod);

//Get the parameters of the relevant methods
CxList jersey = All.GetParameters(producesMethod);
/* end jersey */

// find all calls to JOptionPane.showInputDialog 
// and also references in Groovy to this call (JOptionPane.&showDialog)

// Get direct calls to JOptionPane.showInputDialog 
CxList method_calls = methods.FindByMemberAccess("JOptionPane.showInputDialog");
	method_calls.Add(methods.FindByMemberAccess("*.JOptionPane.showInputDialog"));

// Find references to JOptionPane.&showInputDialog 
CxList reference_calls = All.FindByMemberAccess("JOptionPane.showInputDialog");
reference_calls.Add(All.FindByMemberAccess("*.JOptionPane.showInputDialog"));

reference_calls -= method_calls;

// Get the variables assigned to these references
CxList variables_reference = reference_calls.GetAncOfType(typeof(Declarator));
variables_reference.Add(reference_calls.GetAncOfType(typeof(AssignExpr)));

CxList variables_calls = All.GetByAncs(variables_reference).FindByAssignmentSide(CxList.AssignmentSide.Left);
CxList find_calls = All.NewCxList();

// find method calls to these reference variables (closures)
foreach(CxList v in variables_calls)
{
	string name = "";
	
	if (v.data.GetByIndex(0) is Declarator)
	{
		name = (v.data.GetByIndex(0) as Declarator).Name;
	}
	else if (v.data.GetByIndex(0) is UnknownReference)
	{	
		name = (v.data.GetByIndex(0) as UnknownReference).VariableName;
	}
	find_calls.Add(methods.FindByShortName(name));
}

// find assigned input values
CxList variables = All.NewCxList();
variables.Add(method_calls.GetAncOfType(typeof(Declarator)));
variables.Add(method_calls.GetAncOfType(typeof(AssignExpr)));
variables.Add(find_calls.GetAncOfType(typeof(Declarator)));
variables.Add(find_calls.GetAncOfType(typeof(AssignExpr)));

CxList input_optionDialog = All.GetByAncs(variables).FindByAssignmentSide(CxList.AssignmentSide.Left);

result = All.NewCxList();
result.Add(inputs);
result.Add(input_optionDialog);
result.Add(actionFormParam);
//result.Add(remote_methods_Params);
result.Add(submitParam);
result.Add(ThrowAwayControllerFields);
result.Add(struts2Inputs);
result.Add(springAnnotations);
result.Add(dwrInputs);
result.Add(jersey);
result.Add(jspParam);

CxList toRemove = All.NewCxList();
toRemove.Add(Not_Interactive_Inputs());
toRemove.Add(Find_OSGI_Inputs());
toRemove.Add(Find_WebServices_Input());

result -= toRemove;

/* getAttribute */
result.Add(Add_Get_Attribute(result));
/* end getAttribute */