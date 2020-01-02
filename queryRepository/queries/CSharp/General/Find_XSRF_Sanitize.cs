//For ASP Web Forms, the main solution to prevent XSRF attacks is to 
//assign a unique token to the ViewStateUserKey property of the page.
CxList assignments_lhs = All.FindByAssignmentSide(CxList.AssignmentSide.Left);

CxList UnknownReferences = All.FindByType(typeof(UnknownReference));

CxList ViewStateUserKey_members = 
	All.FindByType(typeof(MemberAccess)).FindByShortName("ViewStateUserKey") +
	//the following provides support for older versions of visual studio, where the
	//developer should deal with xsrf by herself... It is allowed not to use
	//ViewStateUserKey property as a member of some class.
	UnknownReferences.FindByShortName("ViewStateUserKey");

CxList relevant_xsrf_protectors = assignments_lhs * ViewStateUserKey_members;
relevant_xsrf_protectors += 
	UnknownReferences.FindByShortName("AntiXsrfTokenKey");


//For ASP MVC, the main solution to prevent XSRF attacks is to use 
//HTMLHelpers AntiForgeryToken and the ValidateAntiForgeryToken custom
//attribute and / or the Validate util function from AntiForgery class. 

CxList anti_xsrf_mthd = All.FindByMemberAccess("AntiForgery.Validate");

CxList customAttributes = Find_CustomAttribute();
CxList anti_xsrf_attrs = customAttributes.FindByShortName("ValidateAntiForgeryToken");

CxList potentialAttributes = All.InheritsFrom("IAuthorizationFilter");
anti_xsrf_attrs.Add(customAttributes.FindByShortName(potentialAttributes));

//Filter by naming convention: 
//All attribute names end with the word "Attribute"
//[MyAuthorization] is equivalent to[MyAuthorizationAttribute]
foreach (CxList elem in potentialAttributes)
{
	var name = elem.GetName();
	if(name.EndsWith("Attribute"))
	{
		name = name.Substring(0, name.Length - 9);
		anti_xsrf_attrs.Add(customAttributes.FindByShortName(name));
	}
}


relevant_xsrf_protectors.Add(anti_xsrf_mthd);
relevant_xsrf_protectors.Add(anti_xsrf_attrs);

result = relevant_xsrf_protectors.GetAncOfType(typeof(MethodDecl));

//lead with net core
//getting the MinimumSameSitePolicy value
CxList paramConfigureService = Find_ParamDecl().GetParameters(Find_MethodDecls().FindByName("*Startup.ConfigureServices"));
CxList paramService = paramConfigureService.FindByType("IServiceCollection"); 
CxList configureMethod = All.FindAllReferences(paramService).GetMembersOfTarget().FindByShortName("Configure");
CxList flagMinimumSameSitePolicy = All.GetByAncs(configureMethod).FindByShortName("MinimumSameSitePolicy");
CxList flagValue = flagMinimumSameSitePolicy.GetAssigner();
result.Add(flagValue.FindByShortNames(new List<string> {"Lax","Strict"}));