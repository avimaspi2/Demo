//For ASP Web Forms, the main solution to prevent XSRF attacks is to 
//assign a unique token to the ViewStateUserKey property of the page.
CxList assignments_lhs = All.FindByAssignmentSide(CxList.AssignmentSide.Left);

CxList UnknownReferences = All.FindByType(typeof(UnknownReference));

CxList ViewStateUserKey_members = 
	All.FindByType(typeof(MemberAccess)).FindByShortName("ViewStateUserKey", false) +
	//the following provides support for older versions of visual studio, where the
	//developer should deal with xsrf by herself... It is allowed not to use
	//ViewStateUserKey property as a member of some class.
	UnknownReferences.FindByShortName("ViewStateUserKey", false);

CxList relevant_xsrf_protectors = assignments_lhs * ViewStateUserKey_members;
relevant_xsrf_protectors.Add(UnknownReferences.FindByShortName("AntiXsrfTokenKey", false));

//For ASP MVC, the main solution to prevent XSRF attacks is to use 
//HTMLHelpers AntiForgeryToken and the ValidateAntiForgeryToken custom
//attribute and / or the Validate util function from AntiForgery class. 

CxList anti_xsrf_mthd = 
	All.FindByType(typeof(MemberAccess)).FindByShortName("AntiForgery", false).
	GetAncOfType(typeof(MethodInvokeExpr)).FindByShortName("Validate", false);

CxList anti_xsrf_attrs = 
	All.FindByType(typeof(CustomAttribute)).FindByShortName("ValidateAntiForgeryToken", false);

relevant_xsrf_protectors.Add(anti_xsrf_mthd);
relevant_xsrf_protectors.Add(anti_xsrf_attrs);

result = relevant_xsrf_protectors.GetAncOfType(typeof(MethodDecl));