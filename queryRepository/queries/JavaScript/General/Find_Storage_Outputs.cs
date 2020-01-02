result = Find_Members("localStorage.getItem");
result.Add(Find_Members("sessionStorage.getItem"));
result.Add(Find_Members("AsyncStorage.getItem"));
result.Add(Find_Members("AsyncStorage.multiGet"));

//navigator.userAgent can be manipulated by an attacker to contain a dirty value.
//we are considering it a stored value
result.Add(Find_Members("navigator.userAgent"));

CxList WhiteList = Find_Members("localStorage.length");
WhiteList.Add(Find_Members("localStorage.key"));
WhiteList.Add(Find_Members("localStorage.length"));
WhiteList.Add(Find_Members("sessionStorage.key"));
WhiteList.Add(Find_Members("sessionStorage.length"));
	
CxList allStorage = All.FindByMemberAccess("localStorage.*"); 
allStorage.Add(All.FindByMemberAccess("sessionStorage.*"));

CxList toRemove = allStorage.FindByType(typeof(MethodInvokeExpr));
toRemove.Add(allStorage.FindByType(typeof(MethodRef)));
toRemove.Add(WhiteList);
allStorage -= toRemove;

result.Add(allStorage);
CxList sapStorage = Find_SAP_Storage();
result.Add(sapStorage.GetMembersOfTarget().FindByShortName("get"));
result.Add(Find_SAPUI_OData_Reads());