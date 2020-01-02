CxList db = Find_DB();
CxList strings = Find_Strings();

// Find requests
CxList webInputs = Find_Interactive_Inputs();
webInputs -= webInputs.FindByName("*cookie*", false);

// Find a sanitizer for XSRF (clear_csrf_id, validate_token)
CxList clearXSRF = All.FindByShortName("clear_csrf_id") + All.FindByShortName("validate_token");
clearXSRF = clearXSRF.GetTargetOfMembers();
CxList xsrfMethods = All.GetMethod(clearXSRF);
CxList xsrfMethodsContents = All.GetByAncs(xsrfMethods);
clearXSRF = xsrfMethodsContents.FindAllReferences(clearXSRF);
clearXSRF = xsrfMethodsContents.GetByAncs(clearXSRF.GetFathers());

// Find db update/delete/insert
CxList write = strings.FindByName("*update*", false) +
	strings.FindByName("*delete*", false) +
	strings.FindByName("*insert*", false);

CxList dbWrite = db.DataInfluencedBy(write);
// add (heuristically) all methods to the relevant DB update etc.
CxList dbAndMethods = db;// +Find_Methods();
dbWrite.Add(dbAndMethods.FindByShortName("update*"));
dbWrite.Add(dbAndMethods.FindByShortName("delete*"));
dbWrite.Add(dbAndMethods.FindByShortName("insert*"));

// Add update of member
CxList memberAccess = All.FindByType(typeof(MemberAccess));
CxList dbWrite2 = 
	memberAccess.FindByShortName("update*", false) +
	memberAccess.FindByShortName("delete*", false) +
	memberAccess.FindByShortName("insert*", false);

CxList ref1 = All.FindAllReferences(dbWrite2.GetTargetOfMembers());
CxList ref2 = ref1.GetMembersOfTarget();
CxList parameters = All.GetParameters(ref2);

result = (dbWrite + parameters).InfluencedByAndNotSanitized(webInputs, clearXSRF);