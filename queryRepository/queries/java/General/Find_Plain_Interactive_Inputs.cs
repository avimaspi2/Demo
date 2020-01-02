CxList methodDecl = Find_MethodDeclaration();

CxList requestGet = All.FindByMemberAccess("request.get*");

CxList inputs = All.NewCxList();
inputs.Add(requestGet.FindByMemberAccess("request.getCharacterEncoding"));
inputs.Add(requestGet.FindByMemberAccess("request.getContentType"));
inputs.Add(All.FindByMemberAccess("request.ContentType"));
inputs.Add(requestGet.FindByMemberAccess("request.getInputStream"));
inputs.Add(All.GetParameters(requestGet.FindByMemberAccess("request.getParameter"), 0));
inputs.Add(requestGet.FindByMemberAccess("request.getParameterValues"));
inputs.Add(All.FindByMemberAccess("request.ParameterValues"));
inputs.Add(requestGet.FindByMemberAccess("request.getReader"));
inputs.Add(requestGet.FindByMemberAccess("request.getParameterNames"));
inputs.Add(All.FindByMemberAccess("request.ParameterNames"));
inputs.Add(requestGet.FindByMemberAccess("request.getParameterMap"));				
inputs.Add(All.FindByMemberAccess("request.ParameterMap"));		
inputs.Add(requestGet.FindByMemberAccess("request.getHeader"));
inputs.Add(requestGet.FindByMemberAccess("request.getHeaders"));
inputs.Add(requestGet.FindByMemberAccess("request.getHeaderNames"));
inputs.Add(requestGet.FindByMemberAccess("request.getQueryString"));
inputs.Add(requestGet.FindByMemberAccess("request.getRequestedSessionId"));
inputs.Add(requestGet.FindByMemberAccess("request.getPathInfo"));
inputs.Add(requestGet.FindByMemberAccess("request.getRemoteUser"));
inputs.Add(requestGet.FindByMemberAccess("request.getRequestURI"));
inputs.Add(requestGet.FindByMemberAccess("request.getRequestURL"));
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

CxList console = All.FindByMemberAccess("Console.*");

inputs.Add(console.FindByShortName("readPassword"));
inputs.Add(console.FindByShortName("readLine"));


CxList scanner = All.FindByMemberAccess("Scanner.*");

inputs.Add(scanner.FindByShortName("next"));
inputs.Add(scanner.FindByShortName("nextBigDecimal"));
inputs.Add(scanner.FindByShortName("nextBigInteger"));
inputs.Add(scanner.FindByShortName("nextBoolean"));
inputs.Add(scanner.FindByShortName("nextByte"));
inputs.Add(scanner.FindByShortName("nextDouble"));
inputs.Add(scanner.FindByShortName("nextFloat"));
inputs.Add(scanner.FindByShortName("nextInt"));
inputs.Add(scanner.FindByShortName("nextLine"));
inputs.Add(scanner.FindByShortName("nextLong")); 	
inputs.Add(scanner.FindByShortName("nextShort")); 


CxList cookiesInput = All.FindByMemberAccess("Cookie.get*");
cookiesInput.Add(All.FindByMemberAccess("request.getCookies"));
cookiesInput.Add(All.FindByMemberAccess("request.cookies"));


CxList webSessionMethods = All.FindByMemberAccess("WebSession.getRequest");
cookiesInput.Add(webSessionMethods.GetMembersOfTarget().FindByShortName("getCookies"));

// GWT cookies inputs
cookiesInput.Add(Find_GWT_Cookie());
// GWT

cookiesInput.Add(Find_CookieValue_Annotation());

//inputs.Add(cookiesInput - cookiesInput.DataInfluencingOn(cookiesInput));
inputs.Add(cookiesInput);

CxList thisAndBaseRefs = base.Find_ThisRef();
thisAndBaseRefs.Add(base.Find_BaseRef());

CxList ThisBaseMembers = thisAndBaseRefs.GetMembersOfTarget();

inputs = inputs - ThisBaseMembers;

CxList methods = Find_Methods();
inputs.Add(methods.FindByShortName("getRawParameter"));

inputs.Add(All.GetParameters(methodDecl.FindByName("*.main")
	.FindByFieldAttributes(Modifiers.Public | Modifiers.Static)));

if(!All.isWebApplication){
	CxList inCommand = All.FindByName("System.in.*");
	CxList parameters = All.GetParameters(inCommand, 0);
	inputs.Add(parameters);
	inputs.Add(All.FindByName("System.in"));
	inputs.Add(All.FindByMemberAccess("URLConnection.getInputStream"));
}


/* struts */
CxList executes = Find_Execute();

CxList actionFormParam = All.GetParameters(executes);
actionFormParam = actionFormParam.FindByType("ActionForm");

CxList classes = Find_Class_Decl();
CxList actionFormClasses = classes.InheritsFrom("ActionForm");
CxList notResponseWrite = All - All.GetByAncs(All.FindByMemberAccess("response.write"));
foreach(CxList currFormClass in actionFormClasses)
{
	CSharpGraph gr = currFormClass.TryGetCSharpGraph<CSharpGraph>();
	actionFormParam.Add(notResponseWrite.FindByMemberAccess(gr.ShortName + ".get*"));
}
/* end struts */

/* struts 2 */
CxList jspTags = Find_Input_Tags();
CxList jspTagsInputs = All.NewCxList();
jspTagsInputs.Add(jspTags.FindByMemberAccess("_checkbox.name_", false));
jspTagsInputs.Add(jspTags.FindByMemberAccess("_checkboxlist.name_", false));
jspTagsInputs.Add(jspTags.FindByMemberAccess("_combobox.name_", false));
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
CxList struts2Inputs = All.NewCxList();
struts2Inputs.Add(jspTagsInputs);
/* end struts 2 */

/* spring */
CxList springInputs = Find_Spring_Inputs();
/* end spring */


/* GWT */
CxList public_methods = Find_GWT_Server_Input_Methods();
CxList remote_methods_Params = All.GetParameters(public_methods);
/* end GWT */

/* jstl */
CxList jspCode = Find_Jsp_Code();
CxList jspParam = jspCode.FindByMemberAccess("param.*");
CxList jspSpringView = jspCode.FindByMemberAccess("cx_jsptags.inputs*").GetMembersOfTarget();
/* end jstl */

// finds parameter index reference:<input type="hidden" name="BankingCenterId" value="${param['strBankingCenterId']}" /> 
CxList jspParamIndexers = jspCode.FindByShortName("param").FindByType(typeof(UnknownReference));
// filters by IndexerRef parents, to avoid other variables to be confused with 'param' jsp variable
CxList indexerRefs = Find_IndexerRefs();
jspParam.Add(jspParamIndexers.FindByFathers(indexerRefs));
//

inputs.Add(All.FindByMemberAccess("SwingInputField.getText"));
inputs.Add(All.FindByMemberAccess("HyperlinkEvent.getURL"));

CxList socket = All.FindByMemberAccess("socket.get*");
CxList inputStreamReader = Find_Object_Create().FindByShortName("InputStreamReader");
CxList inputStreamReaderFromFileParam = methods.GetParameters(inputStreamReader, 0).FindByShortName("getResourceAsStream");
inputStreamReader -= inputStreamReader.FindByParameters(inputStreamReaderFromFileParam);
CxList In = All.FindAllReferences(All.FindByType("BufferedReader"));

CxList socketInputs = All.NewCxList();
socketInputs.Add(socket);
socketInputs.Add(inputStreamReader);

In = In.DataInfluencedBy(socketInputs);
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
	CSharpGraph g = cls.TryGetCSharpGraph<CSharpGraph>();
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
// Add the public class methods
producesMethod.Add(classProducesMethod.FindByFieldAttributes(Checkmarx.Dom.Modifiers.Public));

//Get the parameters of the relevant methods
CxList jersey = All.GetParameters(producesMethod);
/* end jersey */

// properties
CxList properties = All.FindByMemberAccess("Properties.load");
properties.Add(All.FindByMemberAccess("Properties.get"));

CxList jsfFrameworksInputs = All.FindByShortName("CxJsfInput");

result = inputs;
result.Add(actionFormParam);
result.Add(remote_methods_Params);
//result.Add(submitParam);
//result.Add(ThrowAwayControllerFields);
result.Add(struts2Inputs);
result.Add(springInputs);
result.Add(properties);
result.Add(dwrInputs);
result.Add(jersey);
result.Add(jspParam);
result.Add(jspSpringView);
result.Add(Find_OSGI_Inputs());
result.Add(Find_WebServices_Input());
result.Add(Find_Citrus_Inputs());
result.Add(jsfFrameworksInputs);
result -= Not_Interactive_Inputs() ;

// If it is an Android project - Add Android interactive inputs
if(Find_Android_Settings().Count > 0)
{
	result.Add(Find_Android_Interactive_Inputs());
}