CxList cookies = All.FindByMemberAccess("document.cookie");
CxList url = Find_Inputs();

result = cookies.DataInfluencedBy(url);