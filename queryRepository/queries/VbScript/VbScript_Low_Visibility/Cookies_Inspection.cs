// Cookies_Inspection
// ------------------
// This query finds cases where sensitive data in stored in cookies.
//////////////////////////////////////////////////////////////////////////

CxList cookies = All.FindByMemberAccess("document.cookie");
CxList personal = Find_Personal_Info();

result = cookies.DataInfluencedBy(personal);