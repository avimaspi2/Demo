CxList methods = Find_Methods();
CxList objects = Find_sObjects();
CxList strings = Find_Strings();
CxList pages = Find_VF_Pages();
CxList VF_encode = methods.FindByShortName("*encode*") - methods.FindByShortName("*unencode*");

result = Find_XSS_Replace() + Find_Integers() + All.GetByAncs(VF_encode) + Find_Id_Sanitizers();

// getters
CxList getters = (pages * methods).FindByShortName("get*");
getters -= getters.FindByShortName("*__c");

getters -= getters.GetTargetOfMembers();
CxList gettersMembers = getters.GetTargetOfMembers();

result.Add(getters 
	- getters.FindAllReferences(Find_Apex_Files().FindDefinition(getters))
	- (gettersMembers * objects.GetMembersOfTarget()).GetMembersOfTarget()
	- (gettersMembers * objects).GetMembersOfTarget());

// renderAs pdf
CxList renderedAsPDF = pages.FindByShortName("cx_renderaspdf__on", false);
renderedAsPDF = renderedAsPDF.DataInfluencedBy(strings.FindByShortName("'pdf'"))
	- renderedAsPDF.DataInfluencedBy(strings.FindByShortName("'html'"));
CxList PDFContainer = All.GetClass(renderedAsPDF);
result.Add(pages.GetByAncs(PDFContainer));

// getUrl does some encoding by default
CxList getUrl = methods.FindByShortName("geturl");

// replace 1st parameter and alike
CxList repl = methods.FindByMemberAccess("string.replace") + 
	methods.FindByMemberAccess("string.replaceall") +
	methods.FindByMemberAccess("string.split");
result.Add(All.GetParameters(repl, 0));

result.Add(getUrl - getUrl.FindAllReferences(All.FindDefinition(getUrl)));