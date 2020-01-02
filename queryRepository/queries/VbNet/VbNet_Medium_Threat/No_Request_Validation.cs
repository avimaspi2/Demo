if(AllMembers.All.FindByLanguage("CSharp").Count == 0){
	CxList allAssign = All.FindByType(typeof (AssignExpr));
	CxList allStrings = All.FindByType(typeof (StringLiteral));
	CxList allFalse = allStrings.FindByShortName("false", false);
	CxList booleanFalse = All.FindByType(typeof (BooleanLiteral)).FindByShortName("false", false);

/*	1.
	support for  PagesSection.ValidateRequest = false;
	ValidateRequest is a property of PagesSection object
*/
	CxList PagesSection = All.FindByName("PagesSection", false);
	CxList allPageSection = All.FindByType(PagesSection);
	CxList valRec = allPageSection.GetMembersOfTarget().FindByShortName("validateRequest", false);
	CxList valRecAssign = valRec.GetFathers() * allAssign;
	CxList valRecFalse = booleanFalse.GetByAncs(valRecAssign);
	result.Add(valRecFalse);
	//----------------------------------------------------------------------------------------
	/*	2.
	support for  ValidateRequest="false"
	in Web Page Code Model
	<%@ Page Language="C#" ValidateRequest="false" %>
	*/
	CxList valRecInClassDecl = All.FindByType(typeof(ClassDecl)).FindByRegex(@"validaterequest\s*=\s*""false");
	valRecInClassDecl = valRecInClassDecl.FindByRegex(@"page\s+language\s*=");
	result.Add(valRecInClassDecl);
/*	3.
	support for  validateRequest="false" in config files
	validateRequest should be an element of pages tag
	<pages validateRequest="false" />
*/
	CxList pages = All.FindByMemberAccess("*.pages", false);
	valRec = pages.GetMembersOfTarget().FindByShortName("validateRequest", false);
	valRecAssign = valRec.GetFathers() * allAssign;
	valRecFalse = allFalse.GetByAncs(valRecAssign);
	result.Add(valRecFalse);
}