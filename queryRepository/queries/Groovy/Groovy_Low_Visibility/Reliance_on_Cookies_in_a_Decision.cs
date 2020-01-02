CxList cookies = 
	All.FindByMemberAccess("request.getCookies");

CxList cond = Find_Conditions();

result = cond.DataInfluencedBy(cookies);