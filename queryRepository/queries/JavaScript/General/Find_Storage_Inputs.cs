//fix find members
CxList localStorage = Find_Members("localStorage.setItem");
localStorage.Add(Find_Members("sessionStorage.setItem"));
localStorage.Add(Find_Members("AsyncStorage.setItem"));
localStorage.Add(Find_Members("AsyncStorage.multiSet"));
localStorage.Add(Find_Members("AsyncStorage.mergeItem"));
localStorage.Add(Find_Members("AsyncStorage.multiMerge"));
result.Add(localStorage.FindByType(typeof(MethodInvokeExpr)));

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
result.Add(sapStorage.GetMembersOfTarget().FindByShortName("put"));