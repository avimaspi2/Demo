/*
	This query searches for X-FRAME-OPTIONS in login contexts, wich are set to allow
	framing of their respective forms, therefore becoming vulnerable to clickjacking.

	It is possible to embed these forms into a frame, iframe or object HTML elements
	if the X-FRAME_OPTION header is not set or setted to allow such framing.

	If the header option is not set, the query highlights the method\class encapsulating
	the login routine. In case the x-frame-option is set to allow all the vulnerability
	is marked on the "ALLOW-ALL" parameter.
*/

CxList methods = Find_Methods();
CxList unknownRefs = Find_UnknownReference();
CxList strings = Find_Strings();

//Login methods
CxList loginClasses = Find_Class_Decl().FindByShortName("*login*", false);
CxList methodsInLoginClasses = Find_MethodDeclaration().GetByAncs(loginClasses);
CxList loginMethods = methods.FindByShortName("*login*", false);
loginMethods.Add(methods.FindAllReferences(methodsInLoginClasses));
//loginMethods -= loginMethods.FindByShortNames(new List<string>{"*get*", "set*"});

//HttpServletRequest objects
CxList httpServletRequests = unknownRefs.FindByType("HttpServletRequest");

//Logins influenced by HttpServletRequest
CxList loginReference = httpServletRequests.InfluencingOn(loginMethods).GetStartAndEndNodes(CxList.GetStartEndNodesType.EndNodesOnly);
CxList loginReferenceWrapper = methods.GetMethod(loginReference);

//HttpServletResponse in login wrapper
CxList httpServletResponses = unknownRefs.FindByType("HttpServletResponse");
foreach(CxList wrapper in loginReferenceWrapper){
	CxList httpServletResponsesInWrapper = httpServletResponses.GetByAncs(wrapper);
	if(httpServletResponsesInWrapper.Count > 0){
		CxList responsesInWrapperMembers = unknownRefs.FindAllReferences(httpServletResponsesInWrapper).GetMembersOfTarget();
		CxList addHeaderMethods = responsesInWrapperMembers.FindByShortNames(new List<string>{"setHeader", "addHeader"});
		//If the header is not added, by default framing is allowed.
		if(addHeaderMethods.Count == 0){
			result.Add(wrapper);
		}else{
			CxList xFrameOption = strings.GetParameters(addHeaderMethods, 1).FindByShortNames(new List<string>{"ALLOW-ALL", "ALLOWALL"});
			//If the X-FRAME-OPTIONS header is set to ALLOW ALL, framing is allowed.
			if(xFrameOption.Count > 0){
				result.Add(xFrameOption);
			}
		}
	}
}